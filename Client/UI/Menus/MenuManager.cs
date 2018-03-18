using Client.Scripts.Launcher;
using Client.Scripts.PlayerInput;
using UnityEngine;

namespace Client.Scripts.UI.Menus
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject Menu;
        public InputManager InputManager;
        public GameObject Greyout;

        public AMenu ActiveMenu { get; set; }
        public bool GamePaused { get; set; }

        private void Awake()
        {
            InputManager.onNavigateMenu += NavigateMenu;
        }

        public void NavigateMenu()
        {
            if (ActiveMenu == null)
            {
                OpenMenu();
            }
            else
            {
                ActiveMenu.Back();
            }
        }

        public void OpenMenu()
        {
            Menu.SetActive(true);
            Greyout?.SetActive(true);

            if (GameControl.GameState.Solo)
            {
                Time.timeScale = 0;
            }
        }

        public void CloseMenu(AMenu menu)
        {
            ActiveMenu = null;
            InputManager.InMenu = false;
            menu.gameObject.SetActive(false);

            if (!GamePaused)
            {
                Greyout?.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
