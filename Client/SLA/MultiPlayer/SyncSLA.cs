using Client.Scripts.Launcher;
using Client.Scripts.Network;
using DarkRift.Client;
using Game.Scripts.Network.DarkRiftTags;
using System.Collections.Generic;
using Game.Scripts.SLA;
using UnityEngine;

namespace Client.Scripts.SLA.MultiPlayer
{
    public class SyncSLA : MonoBehaviour
    {
        [SerializeField] private ControlSLA _controlSLA;

        private ScoreSLA _score;

        private void Awake()
        {
            if (GameControl.GameState.Solo)
                return;

            _score = _controlSLA.gameObject.GetComponent<ScoreSLA>();

            GameClient.Instance.MessageReceived += OnDataHandler;
        }

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.SLA || message.Tag >= Tags.TagsPerPlugin * (Tags.SLA + 1))
                    return;

                // Initialize Players
                if (message.Tag == SLATags.UpdateScore)
                {
                    using (var reader = message.GetReader())
                    {
                        var scoreDatas = new List<ScoreDataSLA>();

                        while (reader.Position < reader.Length)
                        {
                            scoreDatas.Add(reader.ReadSerializable<ScoreDataSLA>());
                        }

                        foreach (var scoreData in scoreDatas)
                        {
                            var playerManager = _controlSLA.PlayerManagers[scoreData.PlayerId];
                            _score.Scores[playerManager].UpdateScore(scoreData.CurrentScore, scoreData.TotalScore);
                        }
                        _score.SortScores();
                    }
                }
            }
        }
    }
}
