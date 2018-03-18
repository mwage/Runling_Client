using DarkRift.Client;
using Game.Scripts.Network.DarkRiftTags;
using UnityEngine;

namespace Client.Scripts.Network.Syncronization
{
    public class SyncGameManager : MonoBehaviour
    {
        #region Events

        public delegate void CountdownEventHandler(ushort counter);
        public delegate void PrepareLevelEventHandler(int currentLevel);
        public delegate void StartLevelEventHandler();
        public delegate void HidePanelsEventHandler();

        public static event CountdownEventHandler onCountdown;
        public static event PrepareLevelEventHandler onPrepareLevel;
        public static event StartLevelEventHandler onStartLevel;
        public static event HidePanelsEventHandler onHidePanels;

        #endregion

        private void Awake()
        {
            GameClient.Instance.MessageReceived += OnDataHandler;
        }

        #region NetworkCalls

        #endregion

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.SyncGame || message.Tag >= Tags.TagsPerPlugin * (Tags.SyncGame + 1))
                    return;

                switch (message.Tag)
                {
                    case SyncGameTags.Countdown:
                    {
                        using (var reader = message.GetReader())
                        {
                            var counter = reader.ReadUInt16();
                            onCountdown?.Invoke(counter);
                        }
                        break;
                    }
                    case SyncGameTags.PrepareLevel:
                    {
                        using (var reader = message.GetReader())
                        {
                            var currentLevel = reader.ReadByte();
                            onPrepareLevel?.Invoke(currentLevel);
                        }
                        break;
                    }
                    case SyncGameTags.StartLevel:
                        onStartLevel?.Invoke();
                        break;
                    case SyncGameTags.HidePanels:
                        onHidePanels?.Invoke();
                        break;
                }
            }
        }
    }
}
