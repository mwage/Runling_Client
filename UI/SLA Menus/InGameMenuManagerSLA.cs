using Launcher;
using SLA;
using UI.OptionsMenu;
using UnityEngine;

namespace UI.SLA_Menus
{
    public class InGameMenuManagerSLA : MonoBehaviour
    {
        public InGameMenuSLA InGameMenu;
        public ControlSLA ControlSLA;
        public OptionsMenu.OptionsMenu OptionsMenu;
        public SetHotkeys SetHotkeys;
        public HighScoreMenuSLA HighScoreMenuSLA;
        public ChooseLevelMenuSLA ChooseLevelMenu;

        public GameObject InGameMenuObject;
        public GameObject OptionsMenuObject;
        public GameObject HighScoreMenuObject;
        public GameObject ChooseLevelMenuObject;
        public GameObject WinScreen;
        public GameObject PauseScreen;
        public GameObject ChooseLevel;
        public GameObject RestartGame;

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

        private void Update()
        {
            // Navigate menu with esc
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.NavigateMenu))
            {
                if (!MenuOn && !WinScreen.gameObject.activeSelf)
                {
                    InGameMenuObject.SetActive(true);
                    Time.timeScale = 0;
                    MenuOn = true;
                    if (GameControl.State.SetGameMode == Gamemode.Practice && !ChooseLevel.activeSelf)
                    {
                        RestartGame.SetActive(false);
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
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.Pause))
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