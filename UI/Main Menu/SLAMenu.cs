using Assets.Scripts.Launcher;
using Assets.Scripts.UI.SLA_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Main_Menu
{
    public class SLAMenu : MonoBehaviour {

        public HighScoreMenuSLA _highScoreMenuSLA;

        public GameObject mainMenu;
        public GameObject highScoreMenu;

        public bool SLAMenuActive;

        public void StartGame()
        {
            GameControl.GameActive = true;
            GameControl.Dead = true;
            HighScoreSLA.totalScoreSLA = 0;
            GameControl.CurrentLevel = 0;

            SceneManager.LoadScene("SLA");
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            SLAMenuActive = false;
            highScoreMenu.gameObject.SetActive(true);
            _highScoreMenuSLA.highScoreMenuActive = true;
        }

        public void BackToMenu()
        {
            SLAMenuActive = false;
            gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
    }
}
