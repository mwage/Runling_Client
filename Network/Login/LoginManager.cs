using UnityEngine;
using DarkRift;
using UnityEngine.SceneManagement;

namespace Network.Login
{
    public class LoginManager : MonoBehaviour
    {
        public static int UserID { private set; get; }
        public static bool IsLoggedIn { private set; get; }

        public delegate void SuccessfulLoginEventHandler(int userIP);

        public static event SuccessfulLoginEventHandler onSuccessfulLogin;

        private void Start()
        {
            onSuccessfulLogin += LoadMainMenu;
        }

        public static void Login(string username, string password)
        {
            var writer = new DarkRiftWriter();
            writer.Write(username);
            writer.Write(HashPassword.ReturnHash(password));
            SendToServer(Tags.Login, Subjects.LoginUser, writer);
        }

        public static void AddUser(string username, string password)
        {
            var writer = new DarkRiftWriter();
            writer.Write(username);
            writer.Write(HashPassword.ReturnHash(password));
            SendToServer(Tags.Login, Subjects.AddUser, writer);
        }

        private static void SendToServer(byte tag, ushort subject, object data)
        {
            if (DarkRiftAPI.isConnected)
            {
                DarkRiftAPI.SendMessageToServer(tag, subject, data);
            }
            else
            {
                Debug.Log("Not connected to server");
            }
            BindToDataEvent();
        }

        private static void OnDataHandler(byte tag, ushort subject, object data)
        {
            Debug.Log(subject);
            if (tag == Tags.Login)
            {
                if (subject == Subjects.LoginSuccess)
                {
                    var reader = (DarkRiftReader)data;
                    UserID = reader.ReadInt32();
                    IsLoggedIn = true;

                    if (onSuccessfulLogin != null)
                        onSuccessfulLogin(UserID);
                }
            }
        }

        private static void BindToDataEvent()
        {
            if (DarkRiftAPI.isConnected)
            {
                DarkRiftAPI.onData -= OnDataHandler;
                DarkRiftAPI.onData += OnDataHandler;
            }
        }


        private static void LoadMainMenu(int userID)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}