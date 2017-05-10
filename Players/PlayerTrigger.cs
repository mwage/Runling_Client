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
            if (other.tag == "Finish" && !_finishedLevel)
            {
                GameControl.FinishedLevel = true;
                _finishedLevel = true;
            }

            // Enter Safezone
            if (other.tag == "SafeZone" && !GameControl.IsInvulnerable)
            {
                GameControl.IsInvulnerable = true;

            }

            // Death Trigger
            if ((other.tag == "Enemy" && !GameControl.IsInvulnerable || other.tag == "Strong Enemy") && !GameControl.GodModeActive)
            {
                GameControl.IsDead = true;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "SafeZone" && !_onPlatform)
            {
                SaveZone = other.transform.parent.parent.gameObject;
                EnterSaveZone = true;
                _onPlatform = true;
            }
        }


        // Leave Safezone
        void OnTriggerExit(Collider other)
        {
            if (other.tag == "SafeZone")
            {
                GameControl.IsInvulnerable = false;
                _onPlatform = false;
            }
        }
    }
}
