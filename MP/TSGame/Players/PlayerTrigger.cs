using Characters.Bars;
using Launcher;
using TrueSync;

namespace MP.TSGame.Players
{
    public class PlayerTrigger : TrueSyncBehaviour
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
        public void OnSyncedTriggerStay(TSCollision other)
        {
            // Enter Finishzone
            if (other.gameObject.CompareTag("Finish") && !GameControl.GameState.FinishedLevel)
            {
                GameControl.GameState.FinishedLevel = true;
            }

            // Safety SafeZoneCheck
            if (other.gameObject.CompareTag("SafeZone") && !_playerManager.IsSafe)
            {
                _playerManager.IsSafe = true;
            }

            // Safety Death Trigger
            if (((other.gameObject.CompareTag("Enemy") && !_playerManager.IsSafe || other.gameObject.CompareTag("Strong Enemy"))
                    && !_playerManager.IsInvulnerable) && !_playerManager.GodModeActive)
            {
                _playerManager.IsDead = true;
            }
        }

        public void OnSyncedTriggerEnter(TSCollision other)
        {

            // SafeZone Enter
            if (other.gameObject.CompareTag("SafeZone"))
            {
                _playerManager.IsSafe = true;
                if (owner != TrueSyncManager.LocalPlayer)
                    return;

                var currentSafeZone = other.gameObject.transform.parent.parent.gameObject;
                int currentSafeZoneIdx;
                if (_playerTriggerManager == null)
                {
                    InitializeTrigger();
                    return;
                }
                if (_playerTriggerManager.IsSafeZoneVisitedFirstTime(currentSafeZone, out currentSafeZoneIdx))
                {
                    _playerTriggerManager.MarkVisitedSafeZone(currentSafeZoneIdx);
                    _playerTriggerManager.AddExp(currentSafeZoneIdx);
                    _playerTriggerManager.CreateOrDestroyChaserIfNeed(currentSafeZone);
                    _playerBarsManager.UpdateLevelBar();
                }   
            }

            // Death Trigger
            if (((other.gameObject.CompareTag("Enemy") && !_playerManager.IsSafe || other.gameObject.CompareTag("Strong Enemy"))
                 && !_playerManager.IsInvulnerable) && !_playerManager.GodModeActive)
            {
                _playerManager.IsDead = true;
            }
        }

        // Leave Safezone
        public void OnSyncedTriggerExit(TSCollision other)
        {
            if (other.gameObject.CompareTag("SafeZone") && _playerManager.IsSafe)
            {
                _playerManager.IsSafe = false;
            }
        }
    }
}
