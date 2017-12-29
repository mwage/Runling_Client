using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using Network.DarkRiftTags;
using Network.Synchronization.Data;
using SLA;
using UnityEngine;

namespace Network.Synchronization
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
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.SyncGame)
                return;

            if (message.Subject == SyncGameSubjects.Countdown)
            {
                var reader = message.GetReader();
                var counter = reader.ReadUInt16();

                onCountdown?.Invoke(counter);
            }
            else if (message.Subject == SyncGameSubjects.PrepareLevel)
            {
                var reader = message.GetReader();
                var currentLevel = reader.ReadByte();

                onPrepareLevel?.Invoke(currentLevel);
            }
            else if (message.Subject == SyncGameSubjects.StartLevel)
            {
                onStartLevel?.Invoke();
            }
            else if (message.Subject == SyncGameSubjects.HidePanels)
            {
                onHidePanels?.Invoke();
            }
        }
    }
}
