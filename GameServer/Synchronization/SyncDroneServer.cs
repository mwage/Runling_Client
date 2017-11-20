using DarkRift;
using DarkRift.Server;
using Network.DarkRiftTags;
using Network.Synchronization.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Scripts.Synchronization
{
    public class SyncDroneServer : MonoBehaviour
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

        #region NetworkCalls

        public static void SpawnDrones(List<SpawnDroneData> droneDatas)
        {
            var writer = new DarkRiftWriter();
            foreach (var droneData in droneDatas)
            {
                writer.Write(droneData);
            }

            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncDrone, SyncDroneSubjects.SpawnDrone, writer), SendMode.Reliable);
        }

        public static void UpdateDroneData(List<DroneState> droneStates)
        {
            var writer = new DarkRiftWriter();
            foreach (var state in droneStates)
            {
                writer.Write(state);
            }

            // TODO: Change sendmode to unreliable
            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncDrone, SyncDroneSubjects.UpdateDroneState, writer), SendMode.Reliable);
        }

        public static void DestroyDrone(ushort droneId)
        {
            var writer = new DarkRiftWriter();
            writer.Write(droneId);

            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncDrone, SyncDroneSubjects.DestroyDrone, writer), SendMode.Reliable);
        }

        #endregion

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;
            if (message == null || message.Tag != Tags.SyncDrone)
                return;

            var client = (Client) sender;

        }
    }
}
