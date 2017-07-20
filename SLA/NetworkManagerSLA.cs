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
        public PlayerStateSLA[] PlayerState;
        public bool Voting;


        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            PlayerList = new PhotonPlayer[PhotonNetwork.room.PlayerCount];
            PlayerState = new PlayerStateSLA[PhotonNetwork.room.PlayerCount];
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
                PlayerState[player.ID - 1] = new PlayerStateSLA(player);
            }
            PhotonView.RPC("FinishedLoading", PhotonTargets.All, PhotonNetwork.player);
        }

        [PunRPC]
        private void FinishedLoading(PhotonPlayer player)
        {
            PlayerState[player.ID - 1].FinishedLoading = true;
            Debug.Log(player.NickName + " joined the game");
        }

        private void Update()
        {
            if (Voting)
                return;
            foreach (var state in PlayerState)
            {
                if (!state.FinishedLoading)
                    Debug.Log(state.Owner.NickName);
            }

            if (PlayerState.Where(state => state != null).Any(state => !state.FinishedLoading))
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
                    _votingSystem.PhotonView.RPC("StartVoting", PhotonTargets.All);
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            Debug.Log(player.NickName + " has left the game.");
            PlayerList[player.ID - 1] = null;
            PlayerState[player.ID - 1] = null;
        }
    }
}
