using Assets.Scripts.Launcher;
using Assets.Scripts.UI.RLR_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Main_Menu
{
    public class RLRMenu : MonoBehaviour {

        public HighScoreMenuRLR HighScoreMenuRLR;

        public GameObject MainMenu;
        public GameObject HighScoreMenu;

        public bool RLRMenuActive;

        public void StartGame()
        {
            GameControl.Dead = true;
            GameControl.TotalScore = 0;
            GameControl.CurrentLevel = 1;

            SceneManager.LoadScene("RLR");
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            RLRMenuActive = false;
            HighScoreMenu.gameObject.SetActive(true);
            HighScoreMenuRLR.HighScoreMenuActive = true;
        }

        public void BackToMenu()
        {
            RLRMenuActive = false;
            gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }
    }
}
