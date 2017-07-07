using Launcher;
using RLR;
using UnityEngine;

namespace UI.RLR_Menus
{
    using OptionsMenu;

    public class InGameMenuManagerRLR : MonoBehaviour
    {
        public ControlRLR ControlRLR;
        public GameObject InGameMenu;
        public GameObject OptionsMenu;
        public GameObject ChooseLevel;
        public GameObject HighScoreMenu;
        public GameObject ChooseLevelMenu;
        public GameObject WinScreen;
        public GameObject PauseScreen;
        public GameObject RestartGame;

        private InGameMenuRLR _inGameMenu;
        private OptionsMenu _optionsMenu;
        private ChooseLevelMenuRLR _chooseLevelMenu;
        private HighScoreMenuRLR _highScoreMenuRLR;

        public bool MenuOn;
        private bool _pause;

        private void Awake()
        {
            _inGameMenu = InGameMenu.GetComponent<InGameMenuRLR>();
            _optionsMenu = OptionsMenu.GetComponent<OptionsMenu>();
            _chooseLevelMenu = ChooseLevelMenu.GetComponent<ChooseLevelMenuRLR>();
            _highScoreMenuRLR = HighScoreMenu.GetComponent<HighScoreMenuRLR>();

            MenuOn = false;
            _optionsMenu.OptionsMenuActive = false;
            _chooseLevelMenu.ChooseLevelMenuActive = false;
            _pause = false;
        }

        public void CloseMenus()
        {
            InGameMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            ChooseLevelMenu.SetActive(false);
        }

        private void Update()
        {
            // Navigate menu with esc
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.NavigateMenu))
            {
                if (!MenuOn && !WinScreen.gameObject.activeSelf)
                {
                    InGameMenu.SetActive(true);
                    Time.timeScale = 0;
                    MenuOn = true;
                    if (GameControl.State.SetGameMode == Gamemode.Practice && !ChooseLevel.activeSelf)
                    {
                        RestartGame.SetActive(false);
                        ChooseLevel.SetActive(true);
                    }
                }
                else if (MenuOn && _optionsMenu.OptionsMenuActive)
                {
                    _optionsMenu.DiscardChanges();
                }
                else if (MenuOn && _chooseLevelMenu.ChooseLevelMenuActive)
                {
                    _chooseLevelMenu.Back();
                }
                else if (MenuOn && _highScoreMenuRLR.HighScoreMenuActive)
                {
                    _highScoreMenuRLR.Back();
                }
                else
                {
                    _inGameMenu.BackToGame();
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