using UnityEngine;
using Assets.Scripts.Launcher;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Main_Menu
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject OptionsMenuObject;
        public GameObject SLAMenuObject;
        public OptionsMenu.OptionsMenu OptionsMenu;
        public SLAMenu SLAMenu;

        public void SLA()
        {
            gameObject.SetActive(false);
            SLAMenuObject.gameObject.SetActive(true);
            SLAMenu.SLAMenuActive = true;
        }

        public void RLR()
        {
            GameControl.Dead = true;
            GameControl.TotalScore = 0;
            GameControl.CurrentLevel = 1;

            SceneManager.LoadScene("RLR");
        }

        public void Options()
        {
            gameObject.SetActive(false);
            OptionsMenuObject.gameObject.SetActive(true);
            OptionsMenu.OptionsMenuActive = true;
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
