using Assets.Scripts.Launcher;
using Assets.Scripts.UI.SLA_Menus;
using UnityEngine;

namespace Assets.Scripts.UI.Main_Menu
{
    public class SLAMenu : MonoBehaviour {

        public HighScoreMenuSLA HighScoreMenuSLA;
        public SceneLoader SceneLoader;

        public GameObject MainMenu;
        public GameObject HighScoreMenu;
        public GameObject Menus;

        public bool SLAMenuActive;

        public void StartGame()
        {
            GameControl.Instance.State.IsDead = true;
            GameControl.Instance.State.CurrentLevel = 1;
            GameControl.Instance.State.SetGameMode = Gamemode.Classic;

            SceneLoader.LoadScene("SLA", 1);
            Menus.SetActive(false);
        }

        public void Practice()
        {
            GameControl.Instance.State.IsDead = true;
            GameControl.Instance.State.CurrentLevel = 1;
            GameControl.Instance.State.SetGameMode = Gamemode.Practice;

            SceneLoader.LoadScene("SLA", 1);
            Menus.SetActive(false);
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
