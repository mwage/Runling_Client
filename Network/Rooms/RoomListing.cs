using Launcher;
using UnityEngine;
using UnityEngine.UI;

namespace Network.Rooms
{
    public class RoomListing : MonoBehaviour
    {
        public Text RoomNameText;
        public Text GameModeText;
        public Text PlayerCountText;

        public Room Room { get; private set; }

        private Button _button;

        public void Initialize(Room room)
        {
            Room = room;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => RoomManager.JoinRoom(Room.Id, PlayerColor.Green));
            RoomNameText.text = Room.Name;
            SetGameModeText();
            PlayerCountText.text = Room.CurrentPlayers + "/" + Room.MaxPlayers;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            _button.interactable = RoomManager.CurrentRoom == null;
        }

        public void SetGameModeText()
        {
            // TODO: use friendlyname extension method

            switch (Room.GameType)
            {
                case GameType.Runling:
                    GameModeText.text = "Runling Run";
                    break;
                case GameType.Arena:
                    GameModeText.text = "Arena";
                    break;
                default:
                    GameModeText.text = "Unknown";
                    Debug.Log("Unknown GameType");
                    break;
            }
        }
    }
}