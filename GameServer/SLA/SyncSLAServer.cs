using DarkRift;
using Game.Scripts.Network.DarkRiftTags;
using Game.Scripts.SLA;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Scripts.SLA
{
    public class SyncSLAServer : MonoBehaviour
    {
        #region Network Calls

        public static void UpdateScore(List<ScoreDataSLA> scoreDatas)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                foreach (var data in scoreDatas)
                {
                    writer.Write(data);
                }

                using (var msg = Message.Create(SLATags.UpdateScore, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        #endregion
    }
}
