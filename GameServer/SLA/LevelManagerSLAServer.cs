using Game.Scripts.Drones;
using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.Drones.Movement;
using Game.Scripts.GameSettings;
using Game.Scripts.SLA;
using Game.Scripts.SLA.Levels;
using Game.Scripts.SLA.Levels.Normal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server.Scripts.SLA
{
    public class LevelManagerSLAServer : MonoBehaviour, ILevelManagerSLA
    {
        [SerializeField] private DroneFactory _droneFactory;

        public DroneFactory DroneFactory => _droneFactory;

        public const int NumLevels = 13;             //currently last level available in SLA

        private ControlSLAServer _controlSLA;
        private List<ILevelSLA> _levels;
        private ScoreSLAServer _score;

        public void Awake()
        {
            _controlSLA = GetComponent<ControlSLAServer>();
            _score = GetComponent<ScoreSLAServer>();

            _droneFactory.IsServer = true;
            InitializeLevels();
        }

        private void InitializeLevels()
        {
            _levels = new List<ILevelSLA>
            {
                new Level1SLA(this),
                new Level2SLA(this),
                new Level3SLA(this),
                new Level4SLA(this),
                new Level5SLA(this),
                new Level6SLA(this),
                new Level7SLA(this),
                new Level8SLA(this),
                new Level9SLA(this),
                new Level10SLA(this),
                new Level11SLA(this),
                new Level12SLA(this),
                new Level13SLA(this)
            };
        }

        //Spawn Drones according to what level is active
        public void LoadDrones(int level)
        {
            try
            {
                _levels[level - 1].CreateDrones();
            }
            catch (Exception e)
            {
                Debug.Log("Failed to load level " + level + ": " + e.Message + " - " + e.StackTrace);
            }
        }

        public void SpawnChaser(IDrone drone, Vector3? position = null)
        {
            foreach (var playerManager in _controlSLA.PlayerManagers.Values)
            {
                drone.MovementType = new ChaserMovement(playerManager);
                playerManager.Chaser.AddRange(DroneFactory.SpawnDrones(drone));
            }
        }

        public float GetMovementSpeed(int level)
        {
            return _levels[level - 1].GetMovementSpeed();
        }

        //Load next level or end game
        public void EndLevel(float delay)
        {
            ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
            {
                StartCoroutine(_controlSLA.CurrentLevel == _levels.Count && _controlSLA.GameMode != GameMode.Practice ? EndGameSLA(delay) : NextLevel(delay));
            });
        }

        //load in all but the last level
        private IEnumerator NextLevel(float delay)
        {
            yield return new WaitForSeconds(delay);

            DroneFactory.StopAllCoroutines();
            foreach (Transform child in DroneFactory.transform)
            {
                Destroy(child.gameObject);
            }

            yield return new WaitForSeconds(delay);

            foreach (var playerManager in _controlSLA.PlayerManagers.Values)
            {
                _score.Scores[playerManager].ResetCurrent();
                SyncSLAServer.UpdateScore(_score.Scores.Values.ToList());
            }

            if (_controlSLA.GameMode != GameMode.Practice)
            {
                _controlSLA.CurrentLevel++;
            }

            _controlSLA.InitializeLevel();
        }

        //load win screen after the last level
        private static IEnumerator EndGameSLA(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}
