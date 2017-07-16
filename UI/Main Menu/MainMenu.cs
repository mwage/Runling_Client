using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Main_Menu
{
    using OptionsMenu;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;
        [SerializeField] private Text _multiplayerButtonText;

        private OptionsMenu _optionsMenu;
        private SoloMenu _soloMenu;
        private MultiplayerMenu _multiplayerMenu;

        private void Awake()
        {
            _optionsMenu = _mainMenuManager.OptionsMenu;
            _soloMenu = _mainMenuManager.SoloMenu;
            _multiplayerMenu = _mainMenuManager.MultiplayerMenu;

            if (PhotonNetwork.room != null)
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        private void Update()
        {
            if (PhotonNetwork.connected && !PhotonNetwork.offlineMode)
            {
                _multiplayerButtonText.text = PhotonNetwork.room != null ? "Back to Lobby" : "Multiplayer";
            }
            else
            {
                _multiplayerButtonText.text = "Connect";
            }
        }

        #region Buttons

        public void Multiplayer()
        {
            if (PhotonNetwork.connected && !PhotonNetwork.offlineMode)
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