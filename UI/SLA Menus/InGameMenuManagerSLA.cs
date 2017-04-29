using Assets.Scripts.SLA;
using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.UI.SLA_Menus
{
    public class InGameMenuManagerSLA : MonoBehaviour
    {
        public InGameMenuSLA InGameMenu;
        public ControlSLA ControlSLA;
        public OptionsMenu.OptionsMenu OptionsMenu;
        public OptionsMenu.SetHotkeys SetHotkeys;
        public HighScoreMenuSLA HighScoreMenuSLA;

        public GameObject InGameMenuObject;
        public GameObject OptionsMenuObject;
        public GameObject HighScoreMenuObject;
        public GameObject WinScreen;
        public GameObject PauseScreen;

        public bool MenuOn;
        private bool _pause;

        private void Awake()
        {
            MenuOn = false;
            OptionsMenu.OptionsMenuActive = false;
            HighScoreMenuSLA.HighScoreMenuActive = false;
            _pause = false;
        }

        public void CloseMenus()
        {
            InGameMenuObject.SetActive(false);
            OptionsMenuObject.SetActive(false);
            HighScoreMenuObject.SetActive(false);
        }

        void Update()
        {
            // Navigate menu with esc
            if (InputManager.Instance.GetButtonDown(HotkeyAction.NavigateMenu))
            {
                if (!MenuOn && !WinScreen.gameObject.activeSelf)
                {
                    InGameMenuObject.SetActive(true);
                    Time.timeScale = 0;
                    MenuOn = true;
                }
                else if (MenuOn == true && OptionsMenu.OptionsMenuActive)
                {
                    OptionsMenu.DiscardChanges();
                }
                else if (MenuOn == true && HighScoreMenuSLA.HighScoreMenuActive)
                {
                    HighScoreMenuSLA.Back();
                }
                else
                {
                    InGameMenu.BackToGame();
                }
            }

            //pause game
            if (InputManager.Instance.GetButtonDown(HotkeyAction.Pause))
            {
                if (!_pause)
                {
                    Time.timeScale = 0;
                    _pause = true;
                    PauseScreen.SetActive(true);
                }
                else if (_pause)
                {
                    Time.timeScale = 1;
                    _pause = false;
                    PauseScreen.SetActive(false);
                }
            }
        }
    }
}