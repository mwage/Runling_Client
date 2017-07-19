using Characters.Types;
using Players.Camera;
using RLR;
using UI.RLR_Menus;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Launcher
{
    /// <summary>
    /// Class serve all user inputs: camera, player hotkeys, chat, etc
    /// </summary>
    public class InputServer : MonoBehaviour
    {
        public GameObject Menus; // use to check if menu is active - then InputServer doesnt work
        public CameraHandleMovement CameraHandleMovement;
        public GameObject Player; // lacking for SLA ;(

        private ACharacter _character;

        private InGameMenuManagerRLR inGameMenuManagerRLR;
        private InGameMenuManagerSLA inGameMenuManagerSLA;


        public void Init()
        {
            if (Player != null) _character = Player.gameObject.GetComponentInChildren<ACharacter>();
            if (SceneManager.GetActiveScene().name == "RLR")
            {
                inGameMenuManagerRLR = Menus.GetComponent<InGameMenuManagerRLR>(); // takes first script - InGameMenuMangaerRLR/SLA
            }
            else
            {
                inGameMenuManagerSLA = Menus.GetComponent<InGameMenuManagerSLA>(); // takes first script - InGameMenuMangaerRLR/SLA
            }
            
        }

        public void Update()
        {
            if (inGameMenuManagerRLR == null && inGameMenuManagerSLA == null)
            {
                Init();
            }
            InputAutoClicker();
        }

        public void LateUpdate()
        {
            if (inGameMenuManagerRLR == null && inGameMenuManagerSLA == null)
            {
                Init();
            }
            if (IsMenuActive()) return; // options menu serve all inputs
            if (_character != null)
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
            if (inGameMenuManagerRLR != null)
            {
                if (inGameMenuManagerRLR.MenuOn) return true;
            }
            else
            {
                if (inGameMenuManagerSLA.MenuOn) return true;
            }
            return false;
        }







        public void InputAutoClicker()
        {
            // Start autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateClicker))
            {
                if (!GameControl.State.AutoClickerActive)
                    GameControl.State.AutoClickerActive = true;
            }

            // Stop autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactivateClicker))
            {
                if (GameControl.State.AutoClickerActive)
                    GameControl.State.AutoClickerActive = false;
            }
        }
    }
}