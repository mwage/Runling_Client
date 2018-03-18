using DarkRift;
using Game.Scripts.Network.DarkRiftTags;
using UnityEngine;

namespace Server.Scripts.Synchronization
{
    public class SyncGameServer : MonoBehaviour
    {
        #region Network Calls

        public static void Countdown(ushort counter)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(counter);

                using (var msg = Message.Create(SyncGameTags.Countdown, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        public static void PrepareLevel(byte currentLevel)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(currentLevel);

                using (var msg = Message.Create(SyncGameTags.PrepareLevel, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        public static void StartLevel()
        {
            using (var msg = Message.CreateEmpty(SyncGameTags.StartLevel))
            {
                ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
            }
        }

        public static void HidePanels()
        {
            using (var msg = Message.CreateEmpty(SyncGameTags.HidePanels))
            {
                ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
            }
        }

        #endregion
    }
}
