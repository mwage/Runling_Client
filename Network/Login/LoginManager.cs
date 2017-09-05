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

        private void Awake()
        {
            DarkRiftAPI.onData += OnDataHandler;
        }

        private void OnDestroy()
        {
            DarkRiftAPI.onData -= OnDataHandler;
        }

        public static void Login(string username, string password)
        {
            using (var writer = new DarkRiftWriter())
            {
                writer.Write(username);
                writer.Write(HashPassword.ReturnHash(password));
                Helper.SendToServer(Tags.Login, Subjects.LoginUser, writer);
            } 
        }

        public static void AddUser(string username, string password)
        {
            using (var writer = new DarkRiftWriter())
            {
                writer.Write(username);
                writer.Write(HashPassword.ReturnHash(password));
                Helper.SendToServer(Tags.Login, Subjects.AddUser, writer);
            }
        }

        public static void Logout()
        {
            Helper.SendToServer(Tags.Login, Subjects.LogoutUser, new object[0]);
        }

        private static void OnDataHandler(byte tag, ushort subject, object data)
        {
            if (tag == Tags.Login)
            {
                if (subject == Subjects.LoginSuccess)
                {
                    using (var reader = (DarkRiftReader)data)
                    {
                        UserID = reader.ReadInt32();
                        IsLoggedIn = true;

                        onSuccessfulLogin?.Invoke(UserID);
                    } 
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
    }
}