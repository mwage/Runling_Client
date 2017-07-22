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
        public PhotonView PhotonView;
        public PhotonPlayer[] PlayerList;
        public SyncVarsSLA[] SyncVars;
        public bool Voting;


        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            PlayerList = new PhotonPlayer[PhotonNetwork.room.PlayerCount];
            SyncVars = new SyncVarsSLA[PhotonNetwork.room.PlayerCount];
            if (GameControl.GameState.Solo)
            {
                _votingSystem.gameObject.SetActive(false);
            }

            foreach (var player in PhotonNetwork.playerList)
            {
                PlayerList[player.ID - 1] = player;
            }
            foreach (var player in PlayerList)
            {
                SyncVars[player.ID - 1] = new SyncVarsSLA(player);
            }
            PhotonView.RPC("FinishedLoading", PhotonTargets.All, PhotonNetwork.player);
        }

        [PunRPC]
        private void FinishedLoading(PhotonPlayer player)
        {
            SyncVars[player.ID - 1].FinishedLoading = true;
        }

        private void Update()
        {
            if (Voting)
                return;

            if (SyncVars.Where(state => state != null).Any(state => !state.FinishedLoading))
            {
                return;
            }

            Voting = true;
            if (GameControl.GameState.Solo)
            {
                GameControl.PlayerState.IsDead = true;
                GameControl.GameState.CurrentLevel = 1;
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
            PlayerList[player.ID - 1] = null;
            SyncVars[player.ID - 1] = null;
        }
    }
}
