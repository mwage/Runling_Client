using Assets.Scripts.UI.SLA_Menu;
using UnityEngine;

namespace Assets.Scripts.UI.Main_Menu
{
    public class MainMenuManager : MonoBehaviour
    {

        public MainMenu MainMenu;
        public OptionsMenu OptionsMenu;
        public SLAMenu SLAMenu;
        public HighScoreMenuSLA HighScoreMenuSLA;

        public GameObject MainMenuObject;
        public GameObject OptionsMenuObject;
        public GameObject SLAMenuObject;
        public GameObject HighScoreMenuObject;
 
        private void Awake()
        {
            OptionsMenu.OptionsMenuActive = false;
            SLAMenu.SLAMenuActive = false;
            HighScoreMenuSLA.HighScoreMenuActive = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                if (OptionsMenu.OptionsMenuActive)
                {
                    OptionsMenuObject.SetActive(false);
                    OptionsMenu.OptionsMenuActive = false;
                    MainMenuObject.SetActive(true);
                }
                else if (SLAMenu.SLAMenuActive)
                {
                    SLAMenuObject.SetActive(false);
                    SLAMenu.SLAMenuActive = false;
                    MainMenuObject.SetActive(true);
                }
                else if (HighScoreMenuSLA.HighScoreMenuActive)
                {
                    HighScoreMenuObject.SetActive(false);
                    HighScoreMenuSLA.HighScoreMenuActive = false;
                    SLAMenuObject.SetActive(true);
                    SLAMenu.SLAMenuActive = true;
                }
            }
        }
    }
}