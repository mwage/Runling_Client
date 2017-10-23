using System;
using DarkRift;
using Network;
using Network.DarkRiftTags;
using UnityEngine;
using DarkRift.Client;

namespace GameServer.Scripts
{
    public class NetworkManager : MonoBehaviour
    {
        private void Awake()
        {
            if (MainClient.Instance.Connect())
            {
                // Register as a game server
                MainClient.Instance.SendMessage(
                    new TagSubjectMessage(Tags.GameServerTag, GameServerSubjects.RegisterServer, new DarkRiftWriter()),
                    SendMode.Reliable);
                MainClient.Instance.MessageReceived += OnDataHandler;
                MainClient.Instance.Disconnected += OnDisconnect;
            }
            else
            {
                // Couldn't connect to main server
                Application.Quit();
            }
        }

        private void OnDestroy()
        {
            if (MainClient.Instance != null)
            {
                MainClient.Instance.MessageReceived -= OnDataHandler;
            }
        }

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.GameServerTag)
                return;

            // Successful register
            if (message.Subject == GameServerSubjects.RegisterServer)
            {
                var reader = message.GetReader();
                try
                {
                    ServerManager.Instance.Port = reader.ReadUInt16();
                    ServerManager.Instance.Create();
                }
                catch (Exception exception)
                {
                    Debug.Log(exception.Message + exception.StackTrace);
                }

                MainClient.Instance.SendMessage(new TagSubjectMessage(Tags.GameServerTag, GameServerSubjects.ServerAvailable, new DarkRiftWriter()), SendMode.Reliable);
            }
        }

        private static void OnDisconnect(object obj, DisconnectedEventArgs e)
        {
            ServerManager.Instance.Close();
            Application.Quit();
        }
    }
}





// How to receive messages as server

//                public void Initialize()
//        {
//            Server.ClientManager.ClientConnected += Hi;
//        }
//
//        private void Hi(object sender, ClientConnectedEventArgs e)
//        {
//            Debug.Log("client connected to main!");
//        }
//    }
//}
