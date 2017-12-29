using DarkRift;
using DarkRift.Server;
using Network.DarkRiftTags;
using SLA;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Scripts.SLA
{
    public class NetworkManagerSLAServer : MonoBehaviour
    {

        private void Awake()
        {
            ServerManager.Instance.Server.ClientManager.ClientConnected += OnClientConnected;
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnMessageReceived;
        }

        #region Network Calls

        public static void UpdateScore(List<ScoreDataSLA> scoreDatas)
        {
            var writer = new DarkRiftWriter();
            foreach (var data in scoreDatas)
            {
                writer.Write(data);
            }

            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SLA, SLASubjects.UpdateScore, writer), SendMode.Reliable);
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
