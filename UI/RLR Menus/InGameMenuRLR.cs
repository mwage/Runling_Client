using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.RLR_Menus
{
    public class InGameMenuRLR : MonoBehaviour {

        public GameObject InGameMenuObject;
        public GameObject OptionsMenuObject;
        public GameObject HighScoreMenuObject;
        public GameObject ChooseLevelObject;
        public InGameMenuManagerRLR InGameMenuManagerSla;
        public OptionsMenu.OptionsMenu OptionsMenu;
        public ChooseLevelMenuRLR ChooseLevelMenu;

        public void BackToGame()
        {
            gameObject.SetActive(false);
            InGameMenuManagerSla.MenuOn = false;
            Time.timeScale = 1;
        }

        public void RestartGame()
        {
            GameControl.Instance.State.IsDead = true;
            GameControl.Instance.State.TotalScore = 0;
            GameControl.Instance.State.AutoClickerActive = false;
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
            GameControl.Instance.State.GameActive = false;
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
