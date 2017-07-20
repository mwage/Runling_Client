using Characters;
using Characters.Bars;
using Launcher;
using RLR.Levels;
using UnityEngine;

namespace Players
{
    public class PlayerTrigger : MonoBehaviour
    {
        public PlayerTriggerManager PlayerTriggerManager;
        public PlayerBarsManager PlayerBarsManager;

        private bool _finishedLevel;

        public void InitializeTrigger()
        {
            PlayerTriggerManager = gameObject.transform.parent.parent.GetComponent<PlayerTriggerManager>();
            PlayerBarsManager = gameObject.transform.parent.parent.GetComponent<PlayerBarsManager>();
        }

        // Trigger
        private void OnTriggerStay(Collider other)
        {
            // Enter Finishzone
            if (other.CompareTag("Finish") && !_finishedLevel)
            {
                GameControl.GameState.FinishedLevel = true;
                _finishedLevel = true;
            }

            // Enter Safezone
            if (other.CompareTag("SafeZone") && !GameControl.PlayerState.IsInvulnerable)
            {
                GameControl.PlayerState.IsSafe = true;

            }

            // Safety Death Trigger
            if (((other.CompareTag("Enemy") && !GameControl.PlayerState.IsSafe || other.CompareTag("Strong Enemy")) && !GameControl.PlayerState.IsInvulnerable) && !GameControl.PlayerState.GodModeActive)
            {
                GameControl.PlayerState.IsDead = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("SafeZone")) // is _onPlatform really needed? // case when player moves on platform
            {
                var currentSafeZone = other.transform.parent.parent.gameObject;
                int currentSafeZoneIdx;
                if (PlayerTriggerManager == null)
                {
                    InitializeTrigger();
                    return;
                }
                if (PlayerTriggerManager.IsSafeZoneVisitedFirstTime(currentSafeZone, out currentSafeZoneIdx))
                {
                    PlayerTriggerManager.MarkVisitedSafeZone(currentSafeZoneIdx);
                    PlayerTriggerManager.AddExp(currentSafeZoneIdx);
                    PlayerTriggerManager.CreateOrDestroyChaserIfNeed(currentSafeZone);
                    PlayerBarsManager.UpdateLevelBar();
                }   
            }

            // Death Trigger
            if (((other.CompareTag("Enemy") && !GameControl.PlayerState.IsSafe || other.CompareTag("Strong Enemy")) && !GameControl.PlayerState.IsInvulnerable) && !GameControl.PlayerState.GodModeActive)
            {
                GameControl.PlayerState.IsDead = true;
            }
        }

        // Leave Safezone
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("SafeZone"))
            {
                GameControl.PlayerState.IsSafe = false;
            }
        }
    }
}
