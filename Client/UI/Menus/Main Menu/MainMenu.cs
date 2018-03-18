using Client.Scripts.Network;
using Client.Scripts.Network.Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.UI.Menus.Main_Menu
{
    public class MainMenu : MonoBehaviour
    {
        public Text MultiplayerButtonText;
        public Button MultiplayerButton;
        public Text LogoutButtonText;

        private MainMenuManager _mainMenuManager;
        private SoloMenu _soloMenu;
        private MultiplayerMenu _multiplayerMenu;

        private void Awake()
        {
            _mainMenuManager = transform.parent.GetComponent<MainMenuManager>();
            _soloMenu = _mainMenuManager.SoloMenu;
            _multiplayerMenu = _mainMenuManager.MultiplayerMenu;
        }

        private void Update()
        {
            MultiplayerButtonText.text = RoomManager.Instance.CurrentRoom == null ? "Multiplayer" : "Back to Lobby";
            LogoutButtonText.text = MainClient.Instance.Connected ? "Logout" : "Login";
        }

        #region Buttons

        public void Multiplayer()
        {
            _mainMenuManager.CloseAll();
            _multiplayerMenu.gameObject.SetActive(true);
            _mainMenuManager.MoveCamera(_mainMenuManager.CameraPosMainMenu, _mainMenuManager.CameraRotMainMenu);
        }

        public void Solo()
        {
            _mainMenuManager.CloseAll();
            _soloMenu.NormalSolo.SetActive(RoomManager.Instance.CurrentRoom == null);
            _soloMenu.Error.SetActive(RoomManager.Instance.CurrentRoom != null);
            _soloMenu.gameObject.SetActive(true);
            _mainMenuManager.MoveCamera(_mainMenuManager.CameraPosMainMenu, _mainMenuManager.CameraRotMainMenu);
        }

        #endregion
    }
}