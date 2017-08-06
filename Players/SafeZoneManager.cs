using Characters;
using Characters.Bars;
using Launcher;
using RLR.Levels;
using UnityEngine;

namespace Players
{
    public class SafeZoneManager : MonoBehaviour
    {
        public RunlingChaser RunlingChaser;
        private PlayerManager _playerManager;
        private PlayerBarsManager _playerBarsManager;

        private void Awake()
        {
            _playerManager = transform.parent.GetComponent<PlayerManager>();
        }

        public void InitializeTrigger(PlayerBarsManager playerBarsManager, RunlingChaser runlingChaser)
        {
            _playerBarsManager = playerBarsManager;
            RunlingChaser = runlingChaser;
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
                    MarkVisitedSafeZone(currentSafeZoneIdx);
                    AddExp(currentSafeZoneIdx);
                    CreateOrDestroyChaserIfNeed(currentSafeZone);
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

        public void MarkVisitedSafeZone(int currentSafeZoneIdx)
        {
            GameControl.MapState.VisitedSafeZones[currentSafeZoneIdx] = true;
        }

        public void AddExp(int currentSafeZoneIdx)
        {
            _playerManager.CharacterController.AddExp(LevelingSystem.CalculateExp(currentSafeZoneIdx,
                GameControl.GameState.CurrentLevel, GameControl.GameState.SetDifficulty,
                GameControl.GameState.SetGameMode));
        }

        public void CreateOrDestroyChaserIfNeed(GameObject currentSafeZone)
        {
            RunlingChaser.CreateOrDestroyChaserIfNeed(currentSafeZone);
        }
    }
}