using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerTrigger : MonoBehaviour
    {
        private bool _finishedLevel;
        private bool _onPlatform;
        public bool EnterSaveZone;
        public GameObject SaveZone;

        // Trigger
        void OnTriggerStay(Collider other)
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

            // Death Trigger
            if (((other.CompareTag("Enemy") && !GameControl.State.IsSafe || other.CompareTag("Strong Enemy")) && !GameControl.State.IsInvulnerable) && !GameControl.State.GodModeActive)
            {
                GameControl.State.IsDead = true;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("SafeZone") && !_onPlatform)
            {
                SaveZone = other.transform.parent.parent.gameObject;
                EnterSaveZone = true;
                _onPlatform = true;
            }
        }


        // Leave Safezone
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("SafeZone"))
            {
                GameControl.State.IsSafe = false;
                _onPlatform = false;
            }
        }
    }
}
