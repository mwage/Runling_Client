using System;
using System.Collections;
using System.Collections.Generic;
using Drones;
using Drones.DroneTypes;
using Drones.Movement;
using Launcher;
using SLA;
using SLA.Levels;
using UnityEngine;

namespace Server.Scripts.SLA
{
    public class LevelManagerSLAServer : MonoBehaviour, ILevelManagerSLA
    {
        [SerializeField] private DroneFactory _droneFactory;

        public DroneFactory DroneFactory => _droneFactory;

        public const int NumLevels = 13;             //currently last level available in SLA

//        private ScoreSLA _score;

        private ControlSLAServer _controlSLA;
        private List<ILevelSLA> _levels;

        public void Awake()
        {
//            _score = GetComponent<ScoreSLA>();
            _controlSLA = GetComponent<ControlSLAServer>();
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

//            _score.NewHighScore.transform.parent.gameObject.SetActive(false);

            DroneFactory.StopAllCoroutines();
            foreach (Transform child in DroneFactory.transform)
            {
                Destroy(child.gameObject);
            }

            yield return new WaitForSeconds(delay);

//            _score.CurrentScoreText.text = "0";
            if (_controlSLA.GameMode != GameMode.Practice)
            {
                _controlSLA.CurrentLevel++;
            }

            _controlSLA.InitializeLevel();
        }

        //load win screen after the last level
        private IEnumerator EndGameSLA(float delay)
        {
            yield return new WaitForSeconds(delay);
//            _score.NewHighScore.transform.parent.gameObject.SetActive(false);
        }
    }
}
