using Client.Scripts.Launcher;
using Client.Scripts.Network.Chat;
using DarkRift;
using DarkRift.Client;
using Game.Scripts.Network.DarkRiftTags;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Client.Scripts.Network.Login
{
    public class LoginManager : MonoBehaviour
    {
        public delegate void SuccessfulLoginEventHandler();
        public delegate void FailedLoginEventHandler(byte errorId);
        public delegate void SuccessfulAddUserEventHandler();
        public delegate void FailedAddUserEventHandler(byte errorId);

        public static event SuccessfulLoginEventHandler onSuccessfulLogin;
        public static event FailedLoginEventHandler onFailedLogin;
        public static event SuccessfulAddUserEventHandler onSuccessfulAddUser;
        public static event FailedAddUserEventHandler onFailedAddUser;

        private void Awake()
        {
            MainClient.Instance.MessageReceived += OnDataHandler;
        }

        private void OnDestroy()
        {
            if (MainClient.Instance == null)
                return;

            MainClient.Instance.MessageReceived -= OnDataHandler;
        }
        
        #region Network Calls

        public static void Login(string username, string password)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(username);
                writer.Write(GameControl.Rsa.Encrypt(Encoding.UTF8.GetBytes(password)));

                using (var msg = Message.Create(LoginTags.LoginUser, writer))
                {
                    MainClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        public static void AddUser(string username, string password)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(username);
                writer.Write(GameControl.Rsa.Encrypt(Encoding.UTF8.GetBytes(password)));

                using (var msg = Message.Create(LoginTags.AddUser, writer))
                {
                    MainClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        public static void Logout()
        {
            ChatManager.Instance.Messages = new List<ChatMessage>();

            using (var msg = Message.CreateEmpty(LoginTags.LogoutUser))
            {
                MainClient.Instance.SendMessage(msg, SendMode.Reliable);
            }
        }

        #endregion

        private static void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag >= Tags.TagsPerPlugin * (Tags.Login + 1))
                    return;

                switch (message.Tag)
                {
                    case LoginTags.LoginSuccess:
                        onSuccessfulLogin?.Invoke();
                        break;
                    case LoginTags.LoginFailed:
                    {
                        var reader = message.GetReader();

                        if (reader.Length != 1)
                        {
                            Debug.LogWarning("Invalid LoginFailed Error data received.");
                            return;
                        }

                        onFailedLogin?.Invoke(reader.ReadByte());
                        break;
                    }
                    case LoginTags.AddUserSuccess:
                        onSuccessfulAddUser?.Invoke();
                        break;
                    case LoginTags.AddUserFailed:
                    {
                        var reader = message.GetReader();

                        if (reader.Length != 1)
                        {
                            Debug.LogWarning("Invalid LoginFailed Error data received.");
                            return;
                        }

                        onFailedAddUser?.Invoke(reader.ReadByte());
                        break;
                    }
                }
            }
        }
    }
}