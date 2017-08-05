using Characters.Bars;
using Launcher;
using UnityEngine.SceneManagement;
using UnityEngine;


namespace Players
{
    public class PlayerTrigger : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private PlayerTriggerManager _playerTriggerManager;
        private PlayerBarsManager _playerBarsManager;

        private void Awake()
        {
            _playerManager = transform.parent.GetComponent<PlayerManager>();
        }

        public void InitializeTrigger()
        {
            _playerTriggerManager = _playerManager.transform.parent.GetComponent<PlayerTriggerManager>();
            _playerBarsManager = _playerManager.transform.parent.GetComponent<PlayerBarsManager>();
        }

        // Trigger
        private void OnTriggerStay(Collider other)
        {
            // Enter Finishzone
            if (other.CompareTag("Finish"))
            {
                GameControl.GameState.FinishedLevel = true;
            }

            // Enter Safezone
            if (other.CompareTag("SafeZone"))
            {
                _playerManager.IsSafe = true;
            }

            // Safety Death Trigger
            if (((other.CompareTag("Enemy") && !_playerManager.IsSafe || other.CompareTag("Strong Enemy"))
                 && !_playerManager.IsInvulnerable) && !_playerManager.GodModeActive)
            {
                _playerManager.IsDead = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("SafeZone"))
            {
                var currentSafeZone = other.transform.parent.parent.gameObject;
                int currentSafeZoneIdx;

                if (_playerTriggerManager == null || _playerBarsManager == null)
                {
                    InitializeTrigger();
                }
                if (_playerTriggerManager.IsSafeZoneVisitedFirstTime(currentSafeZone, out currentSafeZoneIdx))
                {
                    _playerTriggerManager.MarkVisitedSafeZone(currentSafeZoneIdx);
                    _playerTriggerManager.AddExp(currentSafeZoneIdx);
                    _playerTriggerManager.CreateOrDestroyChaserIfNeed(currentSafeZone);
                    _playerBarsManager.UpdateLevelBar();
                }
            }

            if (((other.CompareTag("Enemy") && !_playerManager.IsSafe || other.CompareTag("Strong Enemy"))
                 && !_playerManager.IsInvulnerable) && !_playerManager.GodModeActive)
            {
                _playerManager.IsDead = true;
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
    }
}