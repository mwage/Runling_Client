using Launcher;
using UI.RLR_Menus;
using UI.SLA_Menus;
using UnityEngine;

namespace UI.Main_Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        public MainMenu MainMenu;
        public OptionsMenu.OptionsMenu OptionsMenu;
        public SLAMenu SLAMenu;
        public HighScoreMenuSLA HighScoreMenuSLA;
        public RLRMenu RLRMenu;
        public HighScoreMenuRLR HighScoreMenuRLR;

        public GameObject SLAMenuObject;
        public GameObject RLRMenuObject;
        public GameObject HighScoreMenuSLAObject;
        public GameObject HighScoreMenuRLRObject;

        private void Awake()
        {
            OptionsMenu.OptionsMenuActive = false;
            SLAMenu.SLAMenuActive = false;
            HighScoreMenuSLA.HighScoreMenuActive = false;
        }

        private void Update()
        {
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.NavigateMenu))
            {
                if (OptionsMenu.OptionsMenuActive)
                {
                    OptionsMenu.DiscardChanges();
                }
                else if (SLAMenu.SLAMenuActive)
                {
                    SLAMenu.BackToMenu();
                }
                else if (RLRMenu.RLRMenuActive)
                {
                    RLRMenu.BackToMenu();
                }
                else if (HighScoreMenuSLA.HighScoreMenuActive)
                {
                    HighScoreMenuSLAObject.SetActive(false);
                    HighScoreMenuSLA.HighScoreMenuActive = false;
                    SLAMenuObject.SetActive(true);
                    SLAMenu.SLAMenuActive = true;
                }
                else if (HighScoreMenuRLR.HighScoreMenuActive)
                {
                    HighScoreMenuRLRObject.SetActive(false);
                    HighScoreMenuRLR.HighScoreMenuActive = false;
                    RLRMenuObject.SetActive(true);
                    RLRMenu.RLRMenuActive = true;
                }
            }
        }
    }
}