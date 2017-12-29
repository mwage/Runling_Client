using Launcher;
using Players;
using UnityEngine;

namespace UI
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject Menu;
        public InputServer InputServer;
        public GameObject Greyout;

        public AMenu ActiveMenu { get; set; }
        public bool GamePaused { get; set; }

        private void Awake()
        {
            InputServer.onNavigateMenu += NavigateMenu;
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
            InputServer.InMenu = false;
            menu.gameObject.SetActive(false);

            if (!GamePaused)
            {
                Greyout?.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
