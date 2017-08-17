using UnityEngine;
using DarkRift;

namespace Network.Login
{
    public class LoginManager : MonoBehaviour
    {
        public static int UserID { private set; get; }
        public static bool IsLoggedIn { private set; get; }

        public delegate void SuccessfulLoginEventHandler(int userIP);
        public delegate void FailedLoginEventHandler(int reason);
        public delegate void SuccessfulAddUserEventHandler();
        public delegate void FailedAddUserEventHandler(int reason);
        public delegate void SuccessfulLogoutEventHandler();

        public static event SuccessfulLoginEventHandler onSuccessfulLogin;
        public static event FailedLoginEventHandler onFailedLogin;
        public static event SuccessfulAddUserEventHandler onSuccessfulAddUser;
        public static event FailedAddUserEventHandler onFailedAddUser;
        public static event SuccessfulLogoutEventHandler onSuccessfulLogout;


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

        public static void Logout()
        {
            SendToServer(Tags.Login, Subjects.LogoutUser, new object[0]);
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
            if (tag == Tags.Login)
            {
                if (subject == Subjects.LoginSuccess)
                {
                    var reader = (DarkRiftReader)data;
                    UserID = reader.ReadInt32();
                    IsLoggedIn = true;

                    onSuccessfulLogin?.Invoke(UserID);
                }
                if (subject == Subjects.LoginFailed)
                {
                    var reason = (int)data;
                    onFailedLogin?.Invoke(reason);
                }
                if (subject == Subjects.AddUserSuccess)
                {
                    onSuccessfulAddUser?.Invoke();
                }
                if (subject == Subjects.AddUserFailed)
                {
                    var reason = (int)data;
                    onFailedAddUser?.Invoke(reason);
                }
                if (subject == Subjects.LogoutSuccess)
                    onSuccessfulLogout?.Invoke();
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
    }
}