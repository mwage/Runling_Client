using UnityEngine;

namespace UI.Main_Menu
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject OptionsMenuObject;
        public RLRMenus.Characters.OptionsMenu OptionsMenu;
        public GameObject SLAMenuObject;
        public SLAMenu SLAMenu;
        public GameObject RLRMenuObject;
        public RLRMenu RLRMenu;
        public MainMenuManager MainMenuManager;

        public void SLA()
        {
            gameObject.SetActive(false);
            RLRMenuObject.gameObject.SetActive(false);
            SLAMenuObject.gameObject.SetActive(true);
            SLAMenu.SLAMenuActive = true;
            MainMenuManager.MoveCamera(MainMenuManager.CameraPosSLA, MainMenuManager.CameraRotSLA);
        }

        public void RLR()
        {
            gameObject.SetActive(false);
            SLAMenuObject.gameObject.SetActive(false);
            RLRMenuObject.gameObject.SetActive(true);
            RLRMenu.RLRMenuActive = true;
            MainMenuManager.MoveCamera(MainMenuManager.CameraPosRLR, MainMenuManager.CameraRotRLR);
        }

        public void Options()
        {
            gameObject.SetActive(false);
            RLRMenuObject.gameObject.SetActive(false);
            OptionsMenuObject.gameObject.SetActive(true);
            OptionsMenu.OptionsMenuActive = true;
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
