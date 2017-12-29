using DarkRift;
using DarkRift.Client;
using Launcher;
using Network;
using Network.DarkRiftTags;
using System.Collections.Generic;
using UnityEngine;

namespace SLA.Network
{
    public class NetworkManagerSLA : MonoBehaviour
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
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.SLA)
                return;

            // Initialize Players
            if (message.Subject == SLASubjects.UpdateScore)
            {
                var reader = message.GetReader();
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
