using Assets.Scripts.Launcher;
using Assets.Scripts.UI.SLA_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Main_Menu
{
    public class SLAMenu : MonoBehaviour {

        public HighScoreMenuSLA HighScoreMenuSLA;

        public GameObject MainMenu;
        public GameObject HighScoreMenu;

        public bool SLAMenuActive;

        public void StartGame()
        {
            GameControl.Dead = true;
            GameControl.TotalScore = 0;
            GameControl.CurrentLevel = 1;

            SceneManager.LoadScene("SLA");
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            SLAMenuActive = false;
            HighScoreMenu.gameObject.SetActive(true);
            HighScoreMenuSLA.HighScoreMenuActive = true;
        }

        public void BackToMenu()
        {
            SLAMenuActive = false;
            gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }
    }
}
