using DarkRift;
using DarkRift.Client;
using Network.DarkRiftTags;
using UnityEngine;

namespace Network.Synchronization
{
    public class SyncGameManager : MonoBehaviour
    {
        #region Events

        public delegate void CountdownEventHandler(ushort counter);

        public static event CountdownEventHandler onCountdown;

        #endregion

        private void Awake()
        {
            GameClient.Instance.MessageReceived += OnDataHandler;
        }

        #region NetworkCalls

        #endregion

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.SyncGame)
                return;

            if (message.Subject == SyncGameSubjects.Countdown)
            {
                var reader = message.GetReader();
                var counter = reader.ReadUInt16();
                onCountdown?.Invoke(counter);
            }
        }
    }
}
