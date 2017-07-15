using UnityEngine;

namespace UI.Main_Menu.MP
{
    public class LobbyCanvas : MonoBehaviour
    {
        [SerializeField] private RoomLayoutGroup _roomLayoutGroup;

        public void JoinRoom(string roomName)
        {
            
        }

        private void OnEnable()
        {
            //_roomLayoutGroup.OnReceivedRoomListUpdate();
        }
    }
}