using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.MP
{
    public class RoomListing : MonoBehaviour
    {
        [SerializeField] private Text _roomNameText;

        public string RoomName { get; private set; }
        public bool Updated;
        private Button _button;


        private void Start()
        {
            var lobbyObject = transform.parent.parent.parent.parent;
            var lobby = lobbyObject.GetComponent<Lobby>();

            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => lobby.JoinRoom(RoomName));
        }

        private void Update()
        {
            _button.interactable = PhotonNetwork.room == null;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void SetRoomNameText(string text)
        {
            RoomName = text;
            _roomNameText.text = BuildRoomName(RoomName);
        }

        private static string BuildRoomName(string roomName)
        {
            var newRoomName = "";
            foreach (var ch in roomName)
            {
                if (ch == '#')
                {
                    break;
                }
                newRoomName += ch;
            }
            return newRoomName;
        }
    }
}