using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main_Menu
{
    using OptionsMenu;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;

        private OptionsMenu _optionsMenu;
        private SoloMenu _soloMenu;
        private MultiplayerMenu _multiplayerMenu;

        private void Awake()
        {
            _optionsMenu = _mainMenuManager.OptionsMenu;
            _soloMenu = _mainMenuManager.SoloMenu;
            _multiplayerMenu = _mainMenuManager.MultiplayerMenu;
        }

        #region Buttons

        public void Multiplayer()
        {
            if (PhotonNetwork.connected)
            {
                gameObject.SetActive(false);
                _multiplayerMenu.gameObject.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("Connect");
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

        public void Quit()
        {
            Application.Quit();
        }
        #endregion
    }
}