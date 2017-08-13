using UnityEngine;
using DarkRift;

namespace Launcher
{
    public class NetworkManager : MonoBehaviour
    {
        public static string IP = "127.0.0.1";
        public static int Port = 4296;

        private void Start()
        {
            DarkRiftAPI.workInBackground = true;
            DarkRiftAPI.Connect(IP, Port);
        }

        private void OnApplicationQuit()
        {
            DarkRiftAPI.Disconnect();
        }
    }
}
