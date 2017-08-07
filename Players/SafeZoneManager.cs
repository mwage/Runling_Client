using System.Collections.Generic;
using Characters;
using Characters.Bars;
using Launcher;
using RLR;
using RLR.Levels;
using UnityEngine;

namespace Players
{
    public class SafeZoneManager : MonoBehaviour
    {
        public RunlingChaser RunlingChaser;
        private PlayerManager _playerManager;
        private ScoreRLR _score;
        private PlayerBarsManager _playerBarsManager;
        public bool[,] ReachedChaserPlatform;
        public List<GameObject> Chasers;


        private void Awake()
        {
            _playerManager = transform.parent.GetComponent<PlayerManager>();
        }

        public void InitializeTrigger(PlayerBarsManager playerBarsManager, RunlingChaser runlingChaser)
        {
            _playerBarsManager = playerBarsManager;
            RunlingChaser = runlingChaser;
            _score = RunlingChaser.GetComponent<ScoreRLR>();
        }

        #region Trigger

        private void OnTriggerStay(Collider other)
        {
            // Enter Safezone
            if (other.CompareTag("SafeZone"))
            {
                _playerManager.IsSafe = true;
            }
        }

        // Enter SafeZone (check for xp/score)
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("SafeZone"))
            {
                var currentSafeZone = other.transform.parent.parent.gameObject;
                int currentSafeZoneIdx;

                if (_playerBarsManager == null)
                    return;

                if (IsSafeZoneVisitedFirstTime(currentSafeZone, out currentSafeZoneIdx))
                {
                    GameControl.MapState.VisitedSafeZones[currentSafeZoneIdx] = true;
                    AddExp(currentSafeZoneIdx);
                    AddScore(currentSafeZoneIdx);
                    RunlingChaser.CreateOrDestroyChaserIfNeed(currentSafeZone, _playerManager, this, currentSafeZoneIdx);
                    _playerBarsManager.UpdateLevelBar();
                }
            }
        }

        // Leave Safezone
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("SafeZone"))
            {
                _playerManager.IsSafe = false;
            }
        }
        #endregion


        public bool IsSafeZoneVisitedFirstTime(GameObject currentSafeZone, out int currentSafeZoneIdx)
        {
            if (GameControl.MapState.SafeZones.Contains(currentSafeZone)) // always should contain
            {
                currentSafeZoneIdx = GameControl.MapState.SafeZones.IndexOf(currentSafeZone);
                if (GameControl.MapState.VisitedSafeZones[currentSafeZoneIdx])
                    return false; // you have been here, no exp for you

                return true;
            }
            else
            {
                Debug.Log("Error: SafeZone not found.");
                currentSafeZoneIdx = -1;
                return false;
            }
        }

        private void AddExp(int currentSafeZoneIdx)
        {
            _playerManager.CharacterController.AddExp(LevelingSystem.CalculateExp(currentSafeZoneIdx,
                GameControl.GameState.CurrentLevel, GameControl.GameState.SetDifficulty,
                GameControl.GameState.SetGameMode));
        }

        private void AddScore(int currentSafeZoneIdx)
        {
            if (currentSafeZoneIdx == 0)
                return;

            if (GameControl.GameState.SetGameMode == GameMode.TimeMode)
            {
                _score.AddScore();
            }

//            TODO: Add score to character if we want to track total score, etc.
        }
    }
}