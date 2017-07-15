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

        public override void OnJoinedRoom()
        {
            var photonPlayers = PhotonNetwork.playerList;
            for (var i = 0; i < photonPlayers.Length; i++)
            {
                PlayerJoinedRoom(photonPlayers[i]);
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
        {
            PlayerLeftRoom(photonPlayer);
        }
        #endregion
    }
}
