using Launcher;
using Players.Camera;
using RLR;
using SLA;
using UI.RLR_Menus;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Players
{
    /// <inheritdoc />
    /// <summary>
    /// Class serve all user inputs: camera, player hotkeys, chat, etc
    /// </summary>
    public class InputServer : MonoBehaviour
    {
        public CameraHandleMovement CameraHandleMovement;

        private PlayerManager _playerManager;
        private InGameMenuManagerRLR _inGameMenuManagerRLR;
        private InGameMenuManagerSLA _inGameMenuManagerSLA;
        private bool _initialized;


        public void Init(GameObject ingameMenuManager, PlayerManager playerManager)
        {
            if (SceneManager.GetActiveScene().name == "RLR")
            {
                _inGameMenuManagerRLR = ingameMenuManager.GetComponent<InGameMenuManagerRLR>();
                _playerManager = GetComponent<ControlRLR>().PlayerManager;
            }
            else
            {
                _inGameMenuManagerSLA = ingameMenuManager.GetComponent<InGameMenuManagerSLA>();
                _playerManager = GetComponent<ControlSLA>().PlayerManager;
            }
            _initialized = true;
        }

        public void Update()
        {
            if (!_initialized)
                return;

            InputAutoClicker();
            InputGodMode();
            _playerManager.CharacterController.InputAbilities();
        }

        public void LateUpdate()
        {
            if (!_initialized)
                return;

            if (IsMenuActive())
                return;

            CameraHandleMovement.ServeInput();
        }

        private bool IsMenuActive()
        {
            if (_inGameMenuManagerRLR != null)
            {
                if (_inGameMenuManagerRLR.MenuOn)
                    return true;
            }
            else
            {
                if (_inGameMenuManagerSLA.MenuOn)
                    return true;
            }
            return false;
        }
       

        public void InputAutoClicker()
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

        private void InputGodMode()
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
    }
}