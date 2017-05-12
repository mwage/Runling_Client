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
        public RLRMenu RLRMenu;

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
            if (InputManager.Instance.GetButtonDown(HotkeyAction.NavigateMenu))
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
                    HighScoreMenuObject.SetActive(false);
                    HighScoreMenuSLA.HighScoreMenuActive = false;
                    SLAMenuObject.SetActive(true);
                    SLAMenu.SLAMenuActive = true;
                }
            }
        }
    }
}