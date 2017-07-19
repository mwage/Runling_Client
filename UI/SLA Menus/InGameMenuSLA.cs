using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;
using UI.OptionsMenu;

namespace UI.SLA_Menus
{
    using OptionsMenu;

    public class InGameMenuSLA : MonoBehaviour
    {
        [SerializeField] private InGameMenuManagerSLA _inGameMenuManagerSLA;

        private OptionsMenu _optionsMenu;
        private HighScoreMenuSLA _highScoreMenuSLA;
        private ChooseLevelMenuSLA _chooseLevelMenu;

        private void Awake()
        {
            _optionsMenu = _inGameMenuManagerSLA.OptionsMenu;
            _highScoreMenuSLA = _inGameMenuManagerSLA.HighScoreMenuSLA;
            _chooseLevelMenu = _inGameMenuManagerSLA.ChooseLevelMenu;
        }

        #region Buttons

        public void BackToGame()
        {
            gameObject.SetActive(false);
            _inGameMenuManagerSLA.MenuOn = false;
            Time.timeScale = 1;
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            _highScoreMenuSLA.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            GameControl.PlayerState.IsDead = true;
            GameControl.GameState.TotalScore = 0;
            GameControl.GameState.CurrentLevel = 1;
            GameControl.GameState.AutoClickerActive = false;
            Time.timeScale = 1;

            SceneManager.LoadScene("SLA");
        }

        public void Options()
        {
            gameObject.SetActive(false);
            _optionsMenu.gameObject.SetActive(true);
        }

        public void BackToMenu()
        {
            GameControl.GameState.GameActive = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        public void ChooseLevel()
        {
            gameObject.SetActive(false);
            _chooseLevelMenu.gameObject.SetActive(true);
        }
        #endregion
    }
}
