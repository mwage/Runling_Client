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

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void SetRoomNameText(string text)
        {
            RoomName = text;
            _roomNameText.text = RoomName;
        }
    }
}