using Launcher;
using UnityEngine;

namespace UI.RLR_Menus
{
    using OptionsMenu;

    public class InGameMenuManagerRLR : MonoBehaviour
    {
        public InGameMenuRLR InGameMenu;
        public OptionsMenu OptionsMenu;
        public ChooseLevelMenuRLR ChooseLevelMenu;
        public HighScoreMenuRLR HighScoreMenuRLR;

        public GameObject ChooseLevel;
        public GameObject WinScreen;
        public GameObject PauseScreen;
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
            ChooseLevelMenu.gameObject.SetActive(false);
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
                    if (GameControl.GameState.SetGameMode == Gamemode.Practice && !ChooseLevel.activeSelf)
                    {
                        RestartGame.SetActive(false);
                        ChooseLevel.SetActive(true);
                    }
                }
                else if (MenuOn && OptionsMenu.gameObject.activeSelf)
                {
                    OptionsMenu.DiscardChanges();
                }
                else if (MenuOn && ChooseLevelMenu.gameObject.activeSelf)
                {
                    ChooseLevelMenu.Back();
                }
                else if (MenuOn && HighScoreMenuRLR.gameObject.activeSelf)
                {
                    HighScoreMenuRLR.Back();
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