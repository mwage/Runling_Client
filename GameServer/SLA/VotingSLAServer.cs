using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Launcher;
using DarkRift;
using DarkRift.Server;
using Game.Scripts.GameSettings;
using Game.Scripts.Network.DarkRiftTags;
using Server.Scripts.Synchronization;
using UnityEngine;
using UnityEngine.UI;

namespace Server.Scripts.SLA
{
    public class VotingSLAServer : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private ControlSLAServer _controlSLA;
        private const byte CountdownFrom = 30;
        public GameObject Game;

        private readonly Dictionary<GameMode, int> _votesPerMode = new Dictionary<GameMode, int>();

        private void Awake()
        {
            ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
            {
                _text.text = "Votes: 0-0-0";
            });

            _votesPerMode[GameMode.Classic] = 0;
            _votesPerMode[GameMode.Team] = 0;
            _votesPerMode[GameMode.Practice] = 0;

            // Send everyone to start voting
            using (var msg = Message.CreateEmpty(VotingTags.StartVoting))
            {
                ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
            }

            ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
            {
                StartCoroutine(Countdown());
            });

            // Subscribe to all clients
            foreach (var client in ServerManager.Instance.Players.Keys)
            {
                client.MessageReceived += OnMessageReceived;
            }
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.Voting || message.Tag >= Tags.TagsPerPlugin * (Tags.Voting + 1))
                    return;

                var client = e.Client;

                // Vote got submitted
                if (message.Tag == VotingTags.SubmitVote)
                {
                    using (var reader = message.GetReader())
                    {
                        ServerManager.Instance.Players[client].Vote = (GameMode)reader.ReadByte();
                    }
                    UpdateVotes();
                }
                else if (message.Tag == VotingTags.FinishVoting)
                {
                    Debug.Log("Player finished voting.");
                    ServerManager.Instance.Players[client].FinishedVoting = true;

                    if (ServerManager.Instance.Players.Values.Any(player => !player.FinishedVoting))
                        return;

                    Finish();
                }
            }
        }

        public void UpdateVotes()
        {
            byte classicVotes = 0;
            byte teamVotes = 0;
            byte practiceVotes = 0;

            foreach (var player in ServerManager.Instance.Players.Values)
            {
                switch (player.Vote)
                {
                    case GameMode.Classic:
                        classicVotes++;
                        break;
                    case GameMode.Team:
                        teamVotes++;
                        break;
                    case GameMode.Practice:
                        practiceVotes++;
                        break;
                }
            }

            _votesPerMode[GameMode.Classic] = classicVotes;
            _votesPerMode[GameMode.Team] = teamVotes;
            _votesPerMode[GameMode.Practice] = practiceVotes;

            ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
            {
                _text.text = "Arena - Votes: " + classicVotes + "-" + teamVotes + "-" + practiceVotes;
            });

            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(classicVotes);
                writer.Write(teamVotes);
                writer.Write(practiceVotes);

                using (var msg = Message.Create(VotingTags.VoteUpdate, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }
        }

        private IEnumerator Countdown()
        {
            for (ushort i = CountdownFrom; i > 0; i--)
            {
                SyncGameServer.Countdown(i);
                yield return new WaitForSeconds(1);
            }
            SyncGameServer.Countdown(0);
            Finish();
        }

        private void Finish()
        {
            _controlSLA.GameMode = SetGameMode();

            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write((byte)_controlSLA.GameMode);

                using (var msg = Message.Create(VotingTags.StartGame, writer))
                {
                    ServerManager.Instance.SendToAll(msg, SendMode.Reliable);
                }
            }

            ServerManager.Instance.Server.Dispatcher.InvokeWait(() =>
            {
                gameObject.SetActive(false);
                Game.SetActive(true);
            });
        }

        private GameMode SetGameMode()
        {
            var max = new[] { _votesPerMode[GameMode.Classic], _votesPerMode[GameMode.Team], _votesPerMode[GameMode.Practice] }.Max();
            var mostVotes = _votesPerMode.Keys.Where(mode => _votesPerMode[mode] == max).ToList();
            var idx = mostVotes.Count == 1 ? 0 : ServerManager.Instance.Random.Next(0, mostVotes.Count);
            return mostVotes[idx];
        }
    }
}
