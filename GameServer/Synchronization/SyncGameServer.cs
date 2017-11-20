using DarkRift;
using DarkRift.Server;
using Network.DarkRiftTags;
using UnityEngine;

namespace Server.Scripts.Synchronization
{
    public class SyncGameServer : MonoBehaviour
    {
        private void Awake()
        {
            ServerManager.Instance.Server.ClientManager.ClientConnected += OnClientConnected;
        }

        private void OnDestroy()
        {
            if (ServerManager.Instance != null)
            {
                ServerManager.Instance.Server.ClientManager.ClientConnected -= OnClientConnected;
            }
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnMessageReceived;
        }

        #region Network Calls

        public static void Countdown(ushort counter)
        {
            var writer = new DarkRiftWriter();
            writer.Write(counter);
            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncGame, SyncGameSubjects.Countdown, writer), SendMode.Reliable);
        }


        #endregion

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;
            if (message == null || message.Tag != Tags.SyncGame)
                return;

            var client = (Client)sender;

        }
    }
}
