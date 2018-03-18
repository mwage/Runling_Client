using Client.Scripts.Network;
using Client.Scripts.Network.Login;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.UI.Menus.Main_Menu
{
    public class MainMenuMenu : AMenu
    {
        private MenuManager _menuManager;

        private void Awake()
        {
            _menuManager = transform.parent.GetComponent<MenuManager>();
        }

        private void OnEnable()
        {
            _menuManager.ActiveMenu?.gameObject.SetActive(false);
            _menuManager.ActiveMenu = this;
        }

        #region Buttons

        public override void Back()
        {
            _menuManager.CloseMenu(this);
        }

        public void Logout()
        {
            if (MainClient.Instance.Connected)
            {
                LoginManager.Logout();
            }
            else
            {
                SceneManager.LoadScene("Login");
            }
        }

        public void Quit()
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
        #endregion
    }
}
