using Drones;
using Drones.DroneTypes;
using Drones.Movement;
using Launcher;
using SLA.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLA
{
    public class LevelManagerSLA : MonoBehaviour, ILevelManagerSLA
    {
        public GameObject Win;
        [SerializeField] private DroneFactory _droneFactory;

        public DroneFactory DroneFactory => _droneFactory;

        public const int NumLevels = 13;             //currently last level available in SLA

        private ControlSLA _controlSLA;
        private ScoreSLA _score;
        private List<ILevelSLA> _levels;
        

        public void Awake()
        {
            _score = GetComponent<ScoreSLA>();
            _controlSLA = GetComponent<ControlSLA>();

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
            drone.MovementType = new ChaserMovement(_controlSLA.PlayerManagers[0]);
            _controlSLA.PlayerManagers[0].Chaser.AddRange(DroneFactory.SpawnDrones(drone));
        }

        public float GetMovementSpeed(int level)
        {
            return _levels[level - 1].GetMovementSpeed();
        }

        //Load next level or end game
        public void EndLevel(float delay)
        {
            StartCoroutine(_controlSLA.CurrentLevel == _levels.Count && GameControl.GameState.SetGameMode != GameMode.Practice ? EndGameSLA(delay) : NextLevel(delay));
        }

        //load in all but the last level
        private IEnumerator NextLevel(float delay)
        {
            yield return new WaitForSeconds(delay);

            _score.NewHighScore.transform.parent.gameObject.SetActive(false);

            _droneFactory.StopAllCoroutines();
            foreach (Transform child in _droneFactory.transform)
            {
                Destroy(child.gameObject);
            }

            yield return new WaitForSeconds(delay);

            foreach (var playerManager in _controlSLA.PlayerManagers.Values)
            {
                _score.Scores[playerManager].ResetCurrent();
            }

            if (GameControl.GameState.SetGameMode != GameMode.Practice)
            {
                _controlSLA.CurrentLevel++;
            }

            _controlSLA.InitializeLevel();
        }

        //load win screen after the last level
        private IEnumerator EndGameSLA(float delay)
        {
            yield return new WaitForSeconds (delay);
            _score.NewHighScore.transform.parent.gameObject.SetActive(false);
            Win.gameObject.SetActive(true);
        }
    }
}
