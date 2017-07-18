using Characters;
using Characters.Bars;
using Launcher;
using RLR.Levels;
using UnityEngine;

namespace Players
{
    public class PlayerTrigger : MonoBehaviour
    {
        public RunlingChaser RunlingChaser; // initializded in InitializeGameRLR
        public PlayerBarsManager PlayerBarsManager;
        private bool _finishedLevel;
        public bool EnteredOnNewPlatform;

        private void Awake()
        {
        }

        // Trigger
        private void OnTriggerStay(Collider other)
        {
            // Enter Finishzone
            if (other.CompareTag("Finish") && !_finishedLevel)
            {
                GameControl.State.FinishedLevel = true;
                _finishedLevel = true;
            }

            // Enter Safezone
            if (other.CompareTag("SafeZone") && !GameControl.State.IsInvulnerable)
            {
                GameControl.State.IsSafe = true;

            }

            // Safety Death Trigger
            if (((other.CompareTag("Enemy") && !GameControl.State.IsSafe || other.CompareTag("Strong Enemy")) && !GameControl.State.IsInvulnerable) && !GameControl.State.GodModeActive)
            {
                GameControl.State.IsDead = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("SafeZone")) // is _onPlatform really needed? // case when player moves on platform
            {
                var currentSafeZone = other.transform.parent.parent.gameObject;

                if (GameControl.MapState.SafeZones.Contains(currentSafeZone)) // always should contain
                {
                    var currentSafeZoneIdx = GameControl.MapState.SafeZones.IndexOf(currentSafeZone);
                    if (GameControl.MapState.VisitedSafeZones[currentSafeZoneIdx])
                        return; // you have been here, no exp for you

                    GameControl.MapState.VisitedSafeZones[currentSafeZoneIdx] = true;

                    GameControl.PlayerState.CharacterController.AddExp(LevelingSystem.CalculateExp(currentSafeZoneIdx,
                        GameControl.State.CurrentLevel, GameControl.State.SetDifficulty,
                        GameControl.State.SetGameMode));

                    RunlingChaser.CreateOrDestroyChaserIfNeed(currentSafeZone);
                }
                else
                {
                    Debug.Log("donest have safezone");
                }
                
                
            }

            // Death Trigger
            if (((other.CompareTag("Enemy") && !GameControl.State.IsSafe || other.CompareTag("Strong Enemy")) && !GameControl.State.IsInvulnerable) && !GameControl.State.GodModeActive)
            {
                GameControl.State.IsDead = true;
            }
        }

        // Leave Safezone
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("SafeZone"))
            {
                GameControl.State.IsSafe = false;
            }
        }
    }
}
