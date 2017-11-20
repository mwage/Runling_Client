using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.RLR_Menus
{
    using OptionsMenu;

    public class InGameMenuRLR : MonoBehaviour
    {
        [SerializeField] private InGameMenuManagerRLR _inGameMenuManagerRLR;

        private OptionsMenu _optionsMenu;
        private ChooseLevelMenuRLR _chooseLevelMenu;
        private HighScoreMenuRLR _highScoreMenuRLR;

        private void Awake()
        {
            _optionsMenu = _inGameMenuManagerRLR.OptionsMenu;
            _chooseLevelMenu = _inGameMenuManagerRLR.ChooseLevelMenu;
            _highScoreMenuRLR = _inGameMenuManagerRLR.HighScoreMenuRLR;
        }

        #region Buttons

        public void BackToGame()
        {
            gameObject.SetActive(false);
            _inGameMenuManagerRLR.MenuOn = false;
            Time.timeScale = 1;
        }

        public void RestartGame()
        {
            GameControl.GameState.CurrentLevel = 1;
            Time.timeScale = 1;

            SceneManager.LoadScene("RLR");
        }

        public void Options()
        {
            gameObject.SetActive(false);
            _optionsMenu.gameObject.SetActive(true);
        }

        public void BackToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        public void ChooseLevel()
        {
            gameObject.SetActive(false);
            _chooseLevelMenu.gameObject.SetActive(true);
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            _highScoreMenuRLR.gameObject.SetActive(true);
        }
        #endregion
    }
}
