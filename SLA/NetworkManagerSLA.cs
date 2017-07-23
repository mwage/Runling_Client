using System.Linq;
using Launcher;
using Photon;
using UnityEngine;

namespace SLA
{
    public class NetworkManagerSLA : PunBehaviour
    {
        [SerializeField] private VoteGameModeSLA _votingSystem;
        public GameObject Game;
        private PhotonView _photonView;
        public bool Voting;


        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            GameControl.PlayerState.SyncVars = new SyncVarsSLA[PhotonNetwork.room.PlayerCount];
            if (GameControl.GameState.Solo)
            {
                _votingSystem.gameObject.SetActive(false);
            }

            foreach (var player in PhotonNetwork.playerList)
            {
                GameControl.PlayerState.SyncVars[player.ID - 1] = new SyncVarsSLA(player);
            }
            _photonView.RPC("FinishedLoading", PhotonTargets.All, PhotonNetwork.player);
        }

        [PunRPC]
        private void FinishedLoading(PhotonPlayer player)
        {
            GameControl.PlayerState.SyncVars[player.ID - 1].FinishedLoading = true;
        }

        private void Update()
        {
            if (Voting)
                return;

            if (GameControl.PlayerState.SyncVars.Where(state => state != null).Any(state => !state.FinishedLoading))
            {
                return;
            }

            Voting = true;
            if (GameControl.GameState.Solo)
            {
                GameControl.PlayerState.IsDead = true;
                _votingSystem.transform.parent.gameObject.SetActive(false);
                Game.SetActive(true);
            }
            else
            {
                if (PhotonNetwork.isMasterClient)
                    _votingSystem.PhotonView.RPC("StartVoting", PhotonTargets.AllViaServer);
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            Debug.Log(player.NickName + " has left the game.");
            GameControl.PlayerState.SyncVars[player.ID - 1] = null;
        }
    }
}
