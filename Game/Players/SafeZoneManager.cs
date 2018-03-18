using Client.Scripts.Launcher;
using Client.Scripts.RLR;
using Client.Scripts.UI.StatusBars;
using Game.Scripts.Characters;
using Game.Scripts.GameSettings;
using Game.Scripts.RLR;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Players
{
    public class SafeZoneManager : MonoBehaviour
    {
        public RunlingChaser RunlingChaser;
        private PlayerManager _playerManager;
        private ScoreRLR _score;
        private ControlRLR _controlRLR;
        private PlayerBarsManager _playerBarsManager;
        public bool[,] ReachedChaserPlatform;
        public List<GameObject> Chasers;
        public bool[] VisitedSafeZones;

        private void Awake()
        {
            _playerManager = transform.parent.GetComponent<PlayerManager>();
        }

        public void InitializeTrigger(PlayerBarsManager playerBarsManager, RunlingChaser runlingChaser)
        {
            _playerBarsManager = playerBarsManager;
            RunlingChaser = runlingChaser;
            _score = RunlingChaser.GetComponent<ScoreRLR>();
            _controlRLR = RunlingChaser.GetComponent<ControlRLR>();
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
                    VisitedSafeZones[currentSafeZoneIdx] = true;
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
            if (GameControl.GameState.SafeZones.Contains(currentSafeZone)) // always should contain
            {
                currentSafeZoneIdx = GameControl.GameState.SafeZones.IndexOf(currentSafeZone);
                if (VisitedSafeZones[currentSafeZoneIdx])
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
            _playerManager.CharacterManager.Stats.AddExp(LevelingSystem.CalculateExp(currentSafeZoneIdx,
                _controlRLR.CurrentLevel, GameControl.GameState.SetDifficulty,
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