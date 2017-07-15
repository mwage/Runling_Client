using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.MP
{
    public class RoomListing : MonoBehaviour
    {
        [SerializeField] private Text _roomNameText;

        public string RoomName { get; private set; }
        public bool Updated;

        private void Start()
        {
            var lobbyCanvasObject = CanvasManager.Instance.LobbyCanvas.gameObject;
            var lobbyCanvas = lobbyCanvasObject.GetComponent<LobbyCanvas>();

            var button = GetComponent<Button>();
            button.onClick.AddListener(() => lobbyCanvas.JoinRoom(_roomNameText.text));
        }

        private void OnDestroy()
        {
            var button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }

        public void SetRoomNameText(string text)
        {
            RoomName = text;
            _roomNameText.text = RoomName;
        }
    }
}