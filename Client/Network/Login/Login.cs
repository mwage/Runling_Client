using Client.Scripts.Launcher;
using Client.Scripts.Network.Chat;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Client.Scripts.Network.Login
{
    public class Login : MonoBehaviour
    {
        public InputField UsernameInput;
        public InputField PasswordInput;

        public GameObject ControlPanel;
        public Text ConnectionText;
        public Text FeedbackText1;
        public Text FeedbackText2;
        public LoadingAnimation LoadingAnimation;
        public Button LoginButton;
        public Button AddUserButton;
        public Button OfflineButton;
        public GameObject CancelButton;
        public Toggle RememberPassword;

        private bool _rememberPassword;

        private void Awake()
        {
            _rememberPassword = PlayerPrefs.GetInt("RememberPassword") != 0;
            RememberPassword.isOn = _rememberPassword;
            CancelButton.SetActive(false);

            if (PlayerPrefs.HasKey("username"))
                UsernameInput.text = PlayerPrefs.GetString("username");
            if (PlayerPrefs.HasKey("password") && _rememberPassword)
                PasswordInput.text = PlayerPrefs.GetString("password");

            LoginManager.onSuccessfulLogin += LoadMainMenu;
            LoginManager.onFailedLogin += FailedLogin;
            LoginManager.onSuccessfulAddUser += SuccessfulRegister;
            LoginManager.onFailedAddUser += FailedAddUser;

            if (!MainClient.Instance.Connect())
            {
                Debug.Log("TODO: Failed to connect handling");
            }
        }

        private void OnDestroy()
        {
            LoginManager.onSuccessfulLogin -= LoadMainMenu;
            LoginManager.onFailedLogin -= FailedLogin;
            LoginManager.onSuccessfulAddUser -= SuccessfulRegister;
            LoginManager.onFailedAddUser -= FailedAddUser;
        }

        #region Buttons

        private void Update()
        {
            var condition = MainClient.Instance.Connected
                            && UsernameInput.text.Length >= 2 && PasswordInput.text.Length >= 2
                            && GameControl.Rsa.Key != null;

            LoginButton.interactable = condition;
            AddUserButton.interactable = condition;
            OfflineButton.interactable = UsernameInput.text.Length >= 2;
        }

        public void OfflineMode()
        {
            ConnectingScreen("Starting...");
            if (MainClient.Instance.Connected)
            {
                MainClient.Instance.Disconnect();
            }
            SceneManager.LoadScene("MainMenu");
        }

        public void Cancel()
        {
            LoginScreen("Runling Login", "Please enter Username and Password!", Color.white);
        }

        public void RememberPasswordToggle()
        {
            _rememberPassword = RememberPassword.isOn;
            PlayerPrefs.SetInt("RememberPassword", _rememberPassword ? 1 : 0);
        }

        public void LoginButtonFunction()
        {
            PlayerPrefs.SetString("username", UsernameInput.text);
            if (_rememberPassword)
                PlayerPrefs.SetString("password", PasswordInput.text);

            ConnectingScreen("Connecting...");
            LoginManager.Login(UsernameInput.text, PasswordInput.text);
        }

        public void AddUser()
        {
            ConnectingScreen("Creating User...");
            LoginManager.AddUser(UsernameInput.text, PasswordInput.text);
        }
        #endregion


        #region Network Callback

        private void FailedLogin(byte errorId)
        {
            if (errorId == 1)
            {
                PasswordInput.text = "";
                LoginScreen("Login Failed!","Username/Password Combination unknown. Make sure you enter the right username and password.", Color.red);
            }
            else if (errorId == 3)
            {
                LoginScreen("Already Logged In",
                    "This shouldn't happen. Please try again and let us know if the problem persists!", Color.red);
            }
            else
            {
                LoginScreen("Server Error",
                    "This shouldn't happen. Please try again and let us know if the problem persists!", Color.red);
            }
        }

        private void FailedAddUser(byte errorId)
        {
            if (errorId == 1)
            {
                LoginScreen("Failed to Register!", "Username already taken. Please choose a different one!", Color.red);
            }
            else
            {
                LoginScreen("Server Error", "This shouldn't happen. Please try again and let us know if the problem persists!", Color.red);
            }
        }

        private void SuccessfulRegister()
        {
            LoginButtonFunction();
        }
        #endregion

        private void ConnectingScreen(string connectionText)
        {
            ControlPanel.SetActive(false);
            CancelButton.SetActive(true);
            ConnectionText.text = connectionText;

            if (!LoadingAnimation.Particles.activeSelf)
                LoadingAnimation.StartLoaderAnimation();
        }

        private void LoginScreen(string text1, string text2, Color textColor)
        {
            ConnectionText.text = "";
            FeedbackText1.text = text1;
            FeedbackText1.color = textColor;
            FeedbackText2.text = text2;
            LoadingAnimation.StopLoaderAnimation();
            CancelButton.SetActive(false);
            ControlPanel.SetActive(true);
        }

        private static void LoadMainMenu()
        {
            foreach (var group in ChatManager.Instance.SavedChatGroups)
            {
                ChatManager.Instance.JoinChatGroup(group);
            }

            SceneManager.LoadScene("MainMenu");
        }
    }
}