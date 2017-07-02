using Launcher;
using UI.SLA_Menus;
using UnityEngine;

namespace UI.Main_Menu
{
    public class SLAMenu : MonoBehaviour {

        public HighScoreMenuSLA HighScoreMenuSLA;
        public SceneLoader SceneLoader;
        public MainMenuManager MainMenuManager;

        public GameObject MainMenu;
        public GameObject HighScoreMenu;
        public GameObject Menus;

        public bool SLAMenuActive;

        public void StartGame()
        {
            GameControl.State.IsDead = true;
            GameControl.State.CurrentLevel = 1;
            GameControl.State.SetGameMode = Gamemode.Classic;

            SceneLoader.LoadScene("SLA", 1);
            Menus.SetActive(false);
        }

        public void Practice()
        {
            GameControl.State.IsDead = true;
            GameControl.State.CurrentLevel = 1;
            GameControl.State.SetGameMode = Gamemode.Practice;

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
            MainMenuManager.MoveCamera(MainMenuManager.CameraPosMainMenu, MainMenuManager.CameraRotMainMenu);
        }
    }
}
