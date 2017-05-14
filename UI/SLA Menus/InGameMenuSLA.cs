using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.SLA_Menus
{
    public class InGameMenuSLA : MonoBehaviour {

        public GameObject InGameMenuObject;
        public GameObject OptionsMenuObject;
        public GameObject HighScoreMenuObject;
        public GameObject ChooseLevelObject;

        public InGameMenuManagerSLA InGameMenuManagerSla;
        public OptionsMenu.OptionsMenu OptionsMenu;
        public HighScoreMenuSLA HighScoreMenuSLA;
        public ChooseLevelMenuSLA ChooseLevelMenu;

        public void BackToGame()
        {
            gameObject.SetActive(false);
            InGameMenuManagerSla.MenuOn = false;
            Time.timeScale = 1;
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            HighScoreMenuObject.gameObject.SetActive(true);
            HighScoreMenuSLA.HighScoreMenuActive = true;
        }

        public void RestartGame()
        {
            GameControl.IsDead = true;
            GameControl.TotalScore = 0;
            GameControl.AutoClickerActive = false;
            Time.timeScale = 1;

            SceneManager.LoadScene("SLA");
        }

        public void Options()
        {
            InGameMenuObject.gameObject.SetActive(false);
            OptionsMenuObject.gameObject.SetActive(true);
            OptionsMenu.OptionsMenuActive = true;
        }

        public void BackToMenu()
        {
            GameControl.GameActive = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        public void ChooseLevel()
        {
            InGameMenuObject.gameObject.SetActive(false);
            ChooseLevelObject.gameObject.SetActive(true);
            ChooseLevelMenu.ChooseLevelMenuActive = true;
        }
    }
}
