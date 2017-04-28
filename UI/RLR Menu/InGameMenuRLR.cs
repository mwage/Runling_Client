using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.RLR_Menu
{
    public class InGameMenuRLR : MonoBehaviour {

        public GameObject InGameMenuObject;
        public GameObject OptionsMenuObject;
        public GameObject HighScoreMenuObject;
        public InGameMenuManagerRLR InGameMenuManagerSla;
        public OptionsMenu OptionsMenu;
        public HighScoreMenuRLR HighScoreMenuRLR;

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
            HighScoreMenuRLR.HighScoreMenuActive = true;
        }

        public void RestartGame()
        {
            GameControl.Dead = true;
            GameControl.TotalScore = 0;
            GameControl.AutoClickerActive = false;
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
            GameControl.GameActive = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
