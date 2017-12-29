using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.SLA_Menus
{
    public class InGameMenuSLA : AMenu
    {
        public GameObject RestartGameButton;
        public GameObject ChooseLevelButton;

        private MenuManager _menuManager;

        private void Awake()
        {
            _menuManager = transform.parent.GetComponent<MenuManager>();
        }
        
        private void OnEnable()
        {
            _menuManager.InputServer.InMenu = true;
            _menuManager.ActiveMenu?.gameObject.SetActive(false);
            _menuManager.ActiveMenu = this;

            var isPractice = GameControl.GameState.SetGameMode == GameMode.Practice;
            RestartGameButton.SetActive(!isPractice);
            ChooseLevelButton.SetActive(isPractice);
        }

        #region Buttons

        public override void Back()
        {
            _menuManager.CloseMenu(this);
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void BackToMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        #endregion
    }
}
