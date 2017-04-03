using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.SLA_Menu
{
    public class InGameMenuSLA : MonoBehaviour {

        public GameObject inGameMenu;
        public GameObject optionsMenu;
        public GameObject highScoreMenu;
        public InGameMenuManagerSLA _inGameMenuManagerSLA;
        public OptionsMenu _optionsMenu;
        public HighScoreMenuSLA _highScoreMenuSLA;

        public void BackToGame()
        {
            gameObject.SetActive(false);
            _inGameMenuManagerSLA.menuOn = false;
            Time.timeScale = 1;
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            highScoreMenu.gameObject.SetActive(true);
            _highScoreMenuSLA.highScoreMenuActive = true;
        }

        public void RestartGame()
        {
            GameControl.gameActive = true;
            GameControl.dead = true;
            HighScoreSLA.totalScoreSLA = 0;
            GameControl.currentLevel = 0;
            Time.timeScale = 1;

            SceneManager.LoadScene("SLA");
        }

        public void Options()
        {
            inGameMenu.gameObject.SetActive(false);
            optionsMenu.gameObject.SetActive(true);
            _optionsMenu.optionsMenuActive = true;
        }

        public void BackToMenu()
        {
            GameControl.gameActive = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
