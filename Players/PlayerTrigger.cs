using Characters.Bars;
using Launcher;
using UnityEngine.SceneManagement;
using UnityEngine;


namespace Players
{
    public class PlayerTrigger : MonoBehaviour
    {
        public PlayerTriggerManager PlayerTriggerManager;
        public PlayerBarsManager PlayerBarsManager;

        public bool FinishedLevel;
        private SetPlayerState _setPlayerState;


        private void Awake()
        {
            _setPlayerState = transform.parent.GetComponent<SetPlayerState>();
        }

        public void InitializeTrigger()
        {
            PlayerTriggerManager = gameObject.transform.parent.parent.GetComponent<PlayerTriggerManager>();
            PlayerBarsManager = gameObject.transform.parent.parent.GetComponent<PlayerBarsManager>();
        }

        // Trigger
        private void OnTriggerStay(Collider other)
        {
            if (!PhotonNetwork.isMasterClient)
                return;

            if (SceneManager.GetActiveScene().name == "SLA")
            {
                // Enter Finishzone
                if (other.CompareTag("Finish") && !FinishedLevel)
                {
                    _setPlayerState.PhotonView.RPC("SetFinished", PhotonTargets.AllViaServer);
                }

                // Enter Safezone
                if (other.CompareTag("SafeZone") && !GameControl.PlayerState.SyncVars[_setPlayerState.PhotonView.owner.ID - 1].IsSafe)
                {
                    _setPlayerState.PhotonView.RPC("SetSafe", PhotonTargets.All, _setPlayerState.PhotonView.owner.ID,true);
                }

                // Safety Death Trigger
                if (((other.CompareTag("Enemy") && !GameControl.PlayerState.SyncVars[_setPlayerState.PhotonView.owner.ID - 1].IsSafe || other.CompareTag("Strong Enemy"))
                     && !GameControl.PlayerState.SyncVars[_setPlayerState.PhotonView.owner.ID - 1].IsInvulnerable) 
                     && !GameControl.PlayerState.SyncVars[_setPlayerState.PhotonView.owner.ID - 1].GodModeActive)
                {
                    _setPlayerState.PhotonView.RPC("SetDead", PhotonTargets.All, _setPlayerState.PhotonView.owner.ID);
                }
            }
            else
            {
                // Enter Finishzone
                if (other.CompareTag("Finish") && !FinishedLevel)
                {
                    _setPlayerState.PhotonView.RPC("SetFinished", PhotonTargets.AllViaServer);
                }

                // Enter Safezone
                if (other.CompareTag("SafeZone") && !GameControl.PlayerState.IsSafe)
                {
                    GameControl.PlayerState.IsSafe = true;
                }

                // Safety Death Trigger
                if (((other.CompareTag("Enemy") && !GameControl.PlayerState.IsSafe || other.CompareTag("Strong Enemy"))
                     && !GameControl.PlayerState.IsInvulnerable) && !GameControl.PlayerState.GodModeActive)
                {
                    _setPlayerState.PhotonView.RPC("SetDead", PhotonTargets.All, PhotonNetwork.player.ID);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_setPlayerState.PhotonView.isMine)
                return;

            if (other.CompareTag("SafeZone"))
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
            if (!PhotonNetwork.isMasterClient)
                return;

            if (SceneManager.GetActiveScene().name == "SLA")
            {
                if (((other.CompareTag("Enemy") && !GameControl.PlayerState.SyncVars[_setPlayerState.PhotonView.owner.ID - 1].IsSafe || other.CompareTag("Strong Enemy"))
                     && !GameControl.PlayerState.SyncVars[_setPlayerState.PhotonView.owner.ID - 1].IsInvulnerable)
                    && !GameControl.PlayerState.SyncVars[_setPlayerState.PhotonView.owner.ID - 1].GodModeActive)
                {
                    _setPlayerState.PhotonView.RPC("SetDead", PhotonTargets.All, _setPlayerState.PhotonView.owner.ID);
                }
            }
            else
            {
                if (((other.CompareTag("Enemy") && !GameControl.PlayerState.IsSafe || other.CompareTag("Strong Enemy"))
                     && !GameControl.PlayerState.IsInvulnerable) && !GameControl.PlayerState.GodModeActive)
                {
                    _setPlayerState.PhotonView.RPC("SetDead", PhotonTargets.All, PhotonNetwork.player.ID);
                }
            }
        }

        // Leave Safezone
        private void OnTriggerExit(Collider other)
        {
            if (!PhotonNetwork.isMasterClient)
                return;

            if (SceneManager.GetActiveScene().name == "SLA")
            {
                if (other.CompareTag("SafeZone") && !GameControl.PlayerState.SyncVars[_setPlayerState.PhotonView.owner.ID - 1].IsSafe)
                {
                    _setPlayerState.PhotonView.RPC("SetSafe", PhotonTargets.All, _setPlayerState.PhotonView.owner.ID, false);
                }
            }
            else
            {
                Debug.Log("checking for safe");
                Debug.Log(other.tag);
                if (other.CompareTag("SafeZone") && GameControl.PlayerState.IsSafe)
                {
                    GameControl.PlayerState.IsSafe = false;
                }
            }
        }
    }
}
