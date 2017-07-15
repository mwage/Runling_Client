using Photon;
using UnityEngine;

namespace UI.Main_Menu.MP
{
    public class Lobby : PunBehaviour
    {
        [SerializeField] private RoomLayoutGroup _roomLayoutGroup;
        [SerializeField] private GameObject _inLobby;
        [SerializeField] private GameObject _showLobby;
        [SerializeField] private GameObject _createLobby;

        public void JoinRoom(string roomName)
        {
            if (PhotonNetwork.room == null)
            {
                PhotonNetwork.JoinRoom(roomName);
            }
            else
            {
                Debug.Log("Already in a room");
            }
        }

        private void OnEnable()
        {
            if (PhotonNetwork.room == null)
            {
                _showLobby.SetActive(false);
                _createLobby.SetActive(true);
            }
            else
            {
                _createLobby.SetActive(false);
                _showLobby.SetActive(true);
            }
            _roomLayoutGroup.OnReceivedRoomListUpdate();
        }
    }
}