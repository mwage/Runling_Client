using UnityEngine;
using UnityEngine.UI;

namespace Network.Login
{
    public class LoginData : MonoBehaviour
    {
        public InputField UsernameInput;
        public InputField PasswordInput;

        private void Start()
        {
            if (PlayerPrefs.HasKey("username"))
            {
                UsernameInput.text = PlayerPrefs.GetString("username");
            }
        }

        public void LoginButton()
        {
            PlayerPrefs.SetString("username", UsernameInput.text);
            LoginManager.Login(UsernameInput.text, PasswordInput.text);
        }

        public void AddUser()
        {
            PlayerPrefs.SetString("username", UsernameInput.text);
            LoginManager.AddUser(UsernameInput.text, PasswordInput.text);
        }
    }
}
