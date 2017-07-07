using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;
using UI.OptionsMenu;

namespace UI.SLA_Menus
{
    using OptionsMenu;

    public class InGameMenuSLA : MonoBehaviour {

        public GameObject InGameMenu;
        public GameObject OptionsMenu;
        public GameObject HighScoreMenu;
        public GameObject ChooseLevelMenu;
        public InGameMenuManagerSLA InGameMenuManagerSLA;

        private OptionsMenu _optionsMenu;
        private HighScoreMenuSLA _highScoreMenuSLA;
        private ChooseLevelMenuSLA _chooseLevelMenu;

        private void Awake()
        {
            _optionsMenu = OptionsMenu.GetComponent<OptionsMenu>();
            _highScoreMenuSLA = HighScoreMenu.GetComponent<HighScoreMenuSLA>();
            _chooseLevelMenu = ChooseLevelMenu.GetComponent<ChooseLevelMenuSLA>();
        }

        #region Buttons

        public void BackToGame()
        {
            gameObject.SetActive(false);
            InGameMenuManagerSLA.MenuOn = false;
            Time.timeScale = 1;
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            HighScoreMenu.gameObject.SetActive(true);
            _highScoreMenuSLA.HighScoreMenuActive = true;
        }

        public void RestartGame()
        {
            GameControl.State.IsDead = true;
            GameControl.State.TotalScore = 0;
            GameControl.State.CurrentLevel = 1;
            GameControl.State.AutoClickerActive = false;
            Time.timeScale = 1;

            SceneManager.LoadScene("SLA");
        }

        public void Options()
        {
            InGameMenu.gameObject.SetActive(false);
            OptionsMenu.gameObject.SetActive(true);
            _optionsMenu.OptionsMenuActive = true;
        }

        public void BackToMenu()
        {
            GameControl.State.GameActive = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        public void ChooseLevel()
        {
            InGameMenu.gameObject.SetActive(false);
            ChooseLevelMenu.gameObject.SetActive(true);
            _chooseLevelMenu.ChooseLevelMenuActive = true;
        }
        #endregion
    }
}
