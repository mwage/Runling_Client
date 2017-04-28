using Assets.Scripts.UI.SLA_Menus;
using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.UI.Main_Menu
{
    public class MainMenuManager : MonoBehaviour
    {

        public MainMenu MainMenu;
        public OptionsMenu.OptionsMenu OptionsMenu;
        public SLAMenu SLAMenu;
        public HighScoreMenuSLA HighScoreMenuSLA;

        public GameObject SLAMenuObject;
        public GameObject HighScoreMenuObject;
 
        private void Awake()
        {
            InputManager.LoadHotkeys();
            OptionsMenu.OptionsMenuActive = false;
            SLAMenu.SLAMenuActive = false;
            HighScoreMenuSLA.HighScoreMenuActive = false;
        }

        void Update()
        {
            if (InputManager.GetButtonDown("Navigate Menu"))
            {
                if (OptionsMenu.OptionsMenuActive)
                {
                    OptionsMenu.DiscardChanges();
                }
                else if (SLAMenu.SLAMenuActive)
                {
                    SLAMenu.BackToMenu();
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