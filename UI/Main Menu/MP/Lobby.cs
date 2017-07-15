using Photon;
using UnityEngine;

namespace UI.Main_Menu.MP
{
    public class Lobby : PunBehaviour
    {
        [SerializeField] private RoomLayoutGroup _roomLayoutGroup;

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
            _roomLayoutGroup.OnReceivedRoomListUpdate();
        }
    }
}