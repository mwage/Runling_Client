using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.RLR_Menus
{
    public class InGameMenuRLR : MonoBehaviour {

        public GameObject InGameMenuObject;
        public GameObject OptionsMenuObject;
        public GameObject HighScoreMenuObject;
        public GameObject ChooseLevelObject;
        public InGameMenuManagerRLR InGameMenuManagerSla;
        public RLRMenus.Characters.OptionsMenu OptionsMenu;
        public ChooseLevelMenuRLR ChooseLevelMenu;
        public HighScoreMenuRLR HighScoreMenuRLR;

        public void BackToGame()
        {
            gameObject.SetActive(false);
            InGameMenuManagerSla.MenuOn = false;
            Time.timeScale = 1;
        }

        public void RestartGame()
        {
            GameControl.State.IsDead = true;
            GameControl.State.TotalScore = 0;
            GameControl.State.AutoClickerActive = false;
            GameControl.State.CurrentLevel = 1;
            Time.timeScale = 1;

            SceneManager.LoadScene("RLR");
        }

        public void Options()
        {
            InGameMenuObject.gameObject.SetActive(false);
            OptionsMenuObject.gameObject.SetActive(true);
            OptionsMenu.OptionsMenuActive = true;
        }

        public void BackToMenu()
        {
            GameControl.State.GameActive = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        public void ChooseLevel()
        {
            InGameMenuObject.gameObject.SetActive(false);
            ChooseLevelObject.gameObject.SetActive(true);
            ChooseLevelMenu.ChooseLevelMenuActive = true;
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            HighScoreMenuObject.gameObject.SetActive(true);
            HighScoreMenuRLR.HighScoreMenuActive = true;
        }
    }
}
