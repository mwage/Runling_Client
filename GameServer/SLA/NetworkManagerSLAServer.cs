using DarkRift;
using DarkRift.Server;
using Network.DarkRiftTags;
using UnityEngine;

namespace Server.Scripts.SLA
{
    public class NetworkManagerSLAServer : MonoBehaviour {

        private void Awake()
        {
            ServerManager.Instance.Server.ClientManager.ClientConnected += OnClientConnected;
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnMessageReceived;
        }

        #region Network Calls

        public static void InitializePlayers()
        {
            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SLA, SLASubjects.InitializePlayers, new DarkRiftWriter()), SendMode.Reliable);
        }

        public static void PrepareLevel(byte currentLevel)
        {
            var writer = new DarkRiftWriter();
            writer.Write(currentLevel);

            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SLA, SLASubjects.PrepareLevel, writer), SendMode.Reliable);
        }

        public static void StartLevel()
        {
            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SLA, SLASubjects.StartLevel, new DarkRiftWriter()), SendMode.Reliable);
        }

        public static void HidePanels()
        {
            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SLA, SLASubjects.HidePanels, new DarkRiftWriter()), SendMode.Reliable);
        }

        #endregion

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;
            if (message == null || message.Tag != Tags.SLA)
                return;

            var client = (Client)sender;

        }
    }
}
