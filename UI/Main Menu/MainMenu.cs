using Network;
using Network.Login;
using Network.Rooms;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Main_Menu
{
    using OptionsMenu;

    public class MainMenu : MonoBehaviour
    {
        public Text MultiplayerButtonText;
        public Button MultiplayerButton;
        public Text LogoutButtonText;

        private MainMenuManager _mainMenuManager;
        private OptionsMenu _optionsMenu;
        private SoloMenu _soloMenu;
        private MultiplayerMenu _multiplayerMenu;

        private void Awake()
        {
            _mainMenuManager = transform.parent.GetComponent<MainMenuManager>();
            _optionsMenu = _mainMenuManager.OptionsMenu;
            _soloMenu = _mainMenuManager.SoloMenu;
            _multiplayerMenu = _mainMenuManager.MultiplayerMenu;
        }

        private void Update()
        {
            MultiplayerButtonText.text = RoomManager.Instance.CurrentRoom == null ? "Multiplayer" : "Back to Lobby";
            MultiplayerButton.interactable = MainClient.Instance.Connected;
            LogoutButtonText.text = MainClient.Instance.Connected ? "Logout" : "Login";
        }

        #region Buttons

        public void Multiplayer()
        {
            if (MainClient.Instance.Connected)
            {
                gameObject.SetActive(false);
                _multiplayerMenu.gameObject.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("Login");
            }
        }

        public void Solo()
        {
            gameObject.SetActive(false);
            _soloMenu.gameObject.SetActive(true);
        }

        public void Options()
        {
            gameObject.SetActive(false);
            _optionsMenu.gameObject.SetActive(true);
        }

        public void Logout()
        {
            if (MainClient.Instance.Connected)
            {
                LoginManager.Logout();
            }
            else
            {
                SceneManager.LoadScene("Login");
            }
        }

        public void Quit()
        {
            Application.Quit();
        }
        #endregion
    }
}