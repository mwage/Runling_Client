using DarkRift;
using Network.DarkRiftTags;
using UnityEngine;

namespace Server.Scripts.Synchronization
{
    public class SyncGameServer : MonoBehaviour
    {
        #region Network Calls

        public static void Countdown(ushort counter)
        {
            var writer = new DarkRiftWriter();
            writer.Write(counter);
            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncGame, SyncGameSubjects.Countdown, writer), SendMode.Reliable);
        }

        public static void PrepareLevel(byte currentLevel)
        {
            var writer = new DarkRiftWriter();
            writer.Write(currentLevel);

            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncGame, SyncGameSubjects.PrepareLevel, writer), SendMode.Reliable);
        }

        public static void StartLevel()
        {
            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncGame, SyncGameSubjects.StartLevel, new DarkRiftWriter()), SendMode.Reliable);
        }

        public static void HidePanels()
        {
            ServerManager.Instance.SendToAll(new TagSubjectMessage(Tags.SyncGame, SyncGameSubjects.HidePanels, new DarkRiftWriter()), SendMode.Reliable);
        }

        #endregion
    }
}
