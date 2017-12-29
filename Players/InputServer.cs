using Launcher;
using Players.Camera;
using UnityEngine;

namespace Players
{
    /// <inheritdoc />
    /// <summary>
    /// Class serve all user inputs: camera, player hotkeys, chat, etc
    /// </summary>
    public class InputServer : MonoBehaviour
    {
        public CameraHandleMovement CameraHandleMovement;
        public GameObject MouseClickPrefab;
        public GameObject PauseScreen;

        public bool InMenu { get; set; }

        private PlayerManager _playerManager;
        private MouseInput _mouseInput;
        private bool _initialized;
        private bool _pause;

        public delegate void NavigateMenuEventHandler();

        public event NavigateMenuEventHandler onNavigateMenu;

        public void Initialize (PlayerManager playerManager)
        {
            _playerManager = playerManager;
            _mouseInput = new MouseInput(playerManager, this);
            _initialized = true;
        }

        public void Update()
        {
            MenuInput();

            if (!_initialized || InMenu)
                return;

            _mouseInput.ProcessInput();
            AutoClickerInput();
            GodModeInput();
            PauseInput();
            _playerManager.CharacterController.InputAbilities();
        }

        public void LateUpdate()
        {
            if (!_initialized || InMenu)
                return;
            
            CameraHandleMovement.ServeInput();
        }
       

        public void AutoClickerInput()
        {
            // Start autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateClicker))
            {
                _playerManager.AutoClickerActive = true;
            }

            // Stop autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactivateClicker))
            {
                _playerManager.AutoClickerActive = false;
            }
        }

        private void GodModeInput()
        {
            // Become invulnerable
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateGodmode))
            {
                _playerManager.GodModeActive = true;
                _playerManager.GodMode.SetActive(true);
            }

            // Become vulnerable
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactiveGodmode))
            {
                _playerManager.GodModeActive = false;
                _playerManager.GodMode.SetActive(false);
            }
        }

        private void MenuInput()
        {
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.NavigateMenu))
            {
                onNavigateMenu?.Invoke();
            }
        }

        private void PauseInput()
        {
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