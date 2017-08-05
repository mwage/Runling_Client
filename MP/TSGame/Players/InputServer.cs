using Characters.Types;
using Launcher;
using MP.TSGame.Players.Camera;
using UI.RLR_Menus;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MP.TSGame.Players
{
    /// <summary>
    /// Class serve all user inputs: camera, player hotkeys, chat, etc
    /// </summary>
    public class InputServer : MonoBehaviour
    {
        public GameObject Menus; // use to check if menu is active - then InputServer doesnt work
        public CameraHandleMovement CameraHandleMovement;
        public GameObject Player; // lacking for SLA ;(

        private ACharacter _characterController;

        private InGameMenuManagerRLR _inGameMenuManagerRLR;
        private InGameMenuManagerSLA _inGameMenuManagerSLA;


        public void Init()
        {
            if (Player != null) _characterController = GameControl.PlayerState.CharacterController;
            if (SceneManager.GetActiveScene().name == "RLR")
            {
                _inGameMenuManagerRLR = Menus.GetComponent<InGameMenuManagerRLR>();
            }
            else
            {
                _inGameMenuManagerSLA = Menus.GetComponent<InGameMenuManagerSLA>();
            }
        }

        public void Update()
        {
            if (_inGameMenuManagerRLR == null && _inGameMenuManagerSLA == null)
            {
                Init();
            }
            InputAutoClicker();
            InputGodMode();
            if (_characterController != null)
            {
                _characterController.InputAbilities();
            }
        }

        public void LateUpdate()
        {
            if (_inGameMenuManagerRLR == null && _inGameMenuManagerSLA == null)
            {
                Init();
            }
            if (IsMenuActive()) return; // options menu serve all inputs
            if (_characterController != null)
            {
                // serve character RLR
            }
            else
            {
                // SLA
            }
            CameraHandleMovement.ServeInput();
        }

        private bool IsMenuActive()
        {
            if (_inGameMenuManagerRLR != null)
            {
                if (_inGameMenuManagerRLR.MenuOn) return true;
            }
            else
            {
                if (_inGameMenuManagerSLA.MenuOn) return true;
            }
            return false;
        }


        public void InputAutoClicker()
        {
            // Start autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateClicker))
            {
                if (!GameControl.PlayerState.AutoClickerActive)
                    GameControl.PlayerState.AutoClickerActive = true;
            }

            // Stop autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactivateClicker))
            {
                if (GameControl.PlayerState.AutoClickerActive)
                    GameControl.PlayerState.AutoClickerActive = false;
            }
        }

        private void InputGodMode()
        {
            // Become invulnerable
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateGodmode) &&
                !GameControl.PlayerState.GodModeActive)
            {
                GameControl.PlayerState.GodModeActive = true;
                if (GameControl.PlayerState.Player != null)
                {
                    GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(true);
                }
            }

            // Become vulnerable
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactiveGodmode) &&
                GameControl.PlayerState.GodModeActive)
            {
                GameControl.PlayerState.GodModeActive = false;
                if (GameControl.PlayerState.Player != null)
                {
                    GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(false);
                }
            }
        }
    }
}