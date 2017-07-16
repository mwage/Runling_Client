using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.MP
{
    public class RoomListing : MonoBehaviour
    {
        [SerializeField] private Text _roomNameText;
        [SerializeField] private Text _gameModeText;
        [SerializeField] private Text _playerCountText;

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

        public void SetGameModeText(string text)
        {
            switch (text)
            {
                case "RR":
                    _gameModeText.text = "Runling Run";
                    break;
                case "AR":
                    _gameModeText.text = "Arena";
                    break;
                default:
                    _gameModeText.text = "Unknown";
                    Debug.Log(text + " - unknown key");
                    break;
            }
        }

        public void SetPlayerCountText(int current, int max)
        {
            _playerCountText.text = current + "/" + max;
        }
    }
}