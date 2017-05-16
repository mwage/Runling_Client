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
        public ChooseLevelMenuSLA ChooseLevelMenu;

        public GameObject InGameMenuObject;
        public GameObject OptionsMenuObject;
        public GameObject HighScoreMenuObject;
        public GameObject ChooseLevelMenuObject;
        public GameObject WinScreen;
        public GameObject PauseScreen;
        public GameObject ChooseLevel;

        public bool MenuOn;
        private bool _pause;

        private void Awake()
        {
            MenuOn = false;
            OptionsMenu.OptionsMenuActive = false;
            ChooseLevelMenu.ChooseLevelMenuActive = false;
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
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.NavigateMenu))
            {
                if (!MenuOn && !WinScreen.gameObject.activeSelf)
                {
                    InGameMenuObject.SetActive(true);
                    Time.timeScale = 0;
                    MenuOn = true;
                    if (GameControl.Instance.State.SetGameMode == Gamemode.Practice)
                    {
                        ChooseLevel.SetActive(true);
                    }
                }
                else if (MenuOn && OptionsMenu.OptionsMenuActive)
                {
                    OptionsMenu.DiscardChanges();
                }
                else if (MenuOn && HighScoreMenuSLA.HighScoreMenuActive)
                {
                    HighScoreMenuSLA.Back();
                }
                else if (MenuOn && ChooseLevelMenu.ChooseLevelMenuActive)
                {
                    ChooseLevelMenu.Back();
                }
                else
                {
                    InGameMenu.BackToGame();
                }
            }

            //pause game
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.Pause))
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