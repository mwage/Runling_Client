using Launcher;
using SLA;
using UI.OptionsMenu;
using UnityEngine;

namespace UI.SLA_Menus
{
    using OptionsMenu;

    public class InGameMenuManagerSLA : MonoBehaviour
    {
        public InGameMenuSLA InGameMenu;
        public OptionsMenu OptionsMenu;
        public HighScoreMenuSLA HighScoreMenuSLA;
        public ChooseLevelMenuSLA ChooseLevelMenu;

        public GameObject WinScreen;
        public GameObject PauseScreen;
        public GameObject ChooseLevelButton;
        public GameObject RestartGame;

        public bool MenuOn;
        private bool _pause;

        private void Awake()
        {
            MenuOn = false;
            _pause = false;
        }

        public void CloseMenus()
        {
            InGameMenu.gameObject.SetActive(false);
            OptionsMenu.gameObject.SetActive(false);
            HighScoreMenuSLA.gameObject.SetActive(false);
        }

        private void Update()
        {
            #region NavigateMenu
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.NavigateMenu))
            {
                if (!MenuOn && !WinScreen.gameObject.activeSelf)
                {
                    InGameMenu.gameObject.SetActive(true);
                    Time.timeScale = 0;
                    MenuOn = true;
                    if (GameControl.State.SetGameMode == Gamemode.Practice && !ChooseLevelButton.activeSelf)
                    {
                        RestartGame.SetActive(false);
                        ChooseLevelButton.SetActive(true);
                    }
                }
                else if (MenuOn && OptionsMenu.gameObject.activeSelf)
                {
                    OptionsMenu.DiscardChanges();
                }
                else if (MenuOn && HighScoreMenuSLA.gameObject.activeSelf)
                {
                    HighScoreMenuSLA.Back();
                }
                else if (MenuOn && ChooseLevelMenu.gameObject.activeSelf)
                {
                    ChooseLevelMenu.Back();
                }
                else
                {
                    InGameMenu.BackToGame();
                }
            }
            #endregion

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