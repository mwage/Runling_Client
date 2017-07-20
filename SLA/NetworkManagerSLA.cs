using System.Linq;
using UnityEngine;

namespace SLA
{
    public class NetworkManagerSLA : MonoBehaviour
    {
        [SerializeField] private VoteGameModeSLA _votingSystem;

        public PhotonView PhotonView;
        public PhotonPlayer[] PlayerList;
        public PlayerStateSLA[] PlayerState;

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            PlayerList = new PhotonPlayer[PhotonNetwork.room.PlayerCount];
            PlayerState = new PlayerStateSLA[PhotonNetwork.room.PlayerCount];
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
            Debug.Log(player.NickName + " loaded successfully!");
            
            if (PlayerState.Any(state => !state.FinishedLoading))
                return;

            if (PhotonNetwork.isMasterClient)
            _votingSystem.PhotonView.RPC("StartVoting", PhotonTargets.All);
        }
    }
}
