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
            if (LoginManager.IsLoggedIn)
            {
                MultiplayerButtonText.text = RoomManager.CurrentRoom == null ? "Multiplayer" : "Back to Lobby";
            }
            else
            {
                MultiplayerButtonText.text = "Login";
            }
        }

        #region Buttons

        public void Multiplayer()
        {
            if (LoginManager.IsLoggedIn)
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
            LoginManager.Logout();
        }

        public void Quit()
        {
            Application.Quit();
        }
        #endregion
    }
}


