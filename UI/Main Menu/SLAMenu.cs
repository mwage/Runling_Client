using Launcher;
using UI.SLA_Menus;
using UnityEngine;

namespace UI.Main_Menu
{
    public class SLAMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;

        public GameObject PrevMenu;
        
        private SceneLoader _sceneLoader;
        private HighScoreMenuSLA _highScoreMenu;

        private void Awake()
        {
            _highScoreMenu = _mainMenuManager.HighScoreMenuSLA;
            _sceneLoader = _mainMenuManager.SceneLoader;
        }

        #region Buttons

        public void StartGame()
        {
            GameControl.State.IsDead = true;
            GameControl.State.CurrentLevel = 1;
            GameControl.State.SetGameMode = Gamemode.Classic;

            _sceneLoader.LoadScene("SLA", 1);
            _mainMenuManager.gameObject.SetActive(false);
        }

        public void Practice()
        {
            GameControl.State.IsDead = true;
            GameControl.State.CurrentLevel = 1;
            GameControl.State.SetGameMode = Gamemode.Practice;

            _sceneLoader.LoadScene("SLA", 1);
            _mainMenuManager.gameObject.SetActive(false);
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            _highScoreMenu.gameObject.SetActive(true);
        }

        public void BackToMenu()
        {
            gameObject.SetActive(false);
            PrevMenu.gameObject.SetActive(true);
            _mainMenuManager.MoveCamera(_mainMenuManager.CameraPosMainMenu, _mainMenuManager.CameraRotMainMenu);
        }
        #endregion
    }
}
