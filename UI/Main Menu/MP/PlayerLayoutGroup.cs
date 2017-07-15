using UnityEngine;
using System.Collections.Generic;
using Photon;

namespace UI.Main_Menu.MP
{
    public class PlayerLayoutGroup : PunBehaviour
    {
        [SerializeField] private GameObject _playerListingPrefab;

        private List<PlayerListing> _playerList = new List<PlayerListing>();

        private void PlayerLeftRoom(PhotonPlayer photonPlayer)
        {
            var index = _playerList.FindIndex(x => x.PhotonPlayer == photonPlayer);
            if (index != -1)
            {
                Destroy(_playerList[index].gameObject);
                _playerList.RemoveAt(index);
            }
        }

        private void PlayerJoinedRoom(PhotonPlayer photonPlayer)
        {
            PlayerLeftRoom(photonPlayer);

            var playerListingObject = Instantiate(_playerListingPrefab, transform, false);
            var playerListing = playerListingObject.GetComponent<PlayerListing>();
            playerListing.ApplyPhotonPlayer(photonPlayer);

            _playerList.Add(playerListing);
        }

        #region PUN Callbacks

        public void JoinedRoom()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var photonPlayers = PhotonNetwork.playerList;
            foreach (var player in photonPlayers)
            {
                PlayerJoinedRoom(player);
            }
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            PlayerJoinedRoom(newPlayer);
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
        {
            PlayerLeftRoom(photonPlayer);
        }
        #endregion
    }
}
