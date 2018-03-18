using Client.Scripts.Launcher;
using Client.Scripts.Network;
using Client.Scripts.Network.Syncronization;
using DarkRift;
using DarkRift.Client;
using Game.Scripts.GameSettings;
using Game.Scripts.Network.DarkRiftTags;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.SLA.MultiPlayer
{
    public class VotingSLA : MonoBehaviour
    {
        [SerializeField] private SetupNetworkSLA _setupNetwork;
        [SerializeField] private GameObject _votingCanvas;
        [SerializeField] private Button _finishButton;
        [SerializeField] private Text _classicVoteText;
        [SerializeField] private Text _teamVoteText;
        [SerializeField] private Text _practiceVoteText;
        [SerializeField] private GameObject _countdownPrefab;

        private GameObject _currentCountdown;
        private GameMode? _currentGameMode;

        private void Awake()
        {
            GameClient.Instance.MessageReceived += OnDataHandler;
            SyncGameManager.onCountdown += OnCountdown;
        }

        private void OnDestroy()
        {
            SyncGameManager.onCountdown -= OnCountdown;
        }

        #region Buttons

        public void VoteModeClassic()
        {
            SubmitVote(GameMode.Classic);
        }

        public void VoteModeTeam()
        {
            SubmitVote(GameMode.Team);
        }

        public void VoteModePractice()
        {
            SubmitVote(GameMode.Practice);
        }

        #endregion

        #region Network Calls

        private void SubmitVote(GameMode gameMode)
        {
            // Only send selection if value changed.
            if (gameMode == _currentGameMode)
                return;

            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write((byte)gameMode);

                using (var msg = Message.Create(VotingTags.SubmitVote, writer))
                {
                    GameClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
            _currentGameMode = gameMode;
        }

        public void Finish()
        {
            _finishButton.gameObject.SetActive(false);

            using (var msg = Message.CreateEmpty(VotingTags.FinishVoting))
            {
                GameClient.Instance.SendMessage(msg, SendMode.Reliable);
            }
        }

        #endregion

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.Voting || message.Tag >= Tags.TagsPerPlugin * (Tags.Voting + 1))
                    return;
                
                switch (message.Tag)
                {
                    // Start Voting
                    case VotingTags.StartVoting:
                    {
                        _votingCanvas.SetActive(true);
                        break;
                    }
                    // Votes changed
                    case VotingTags.VoteUpdate:
                    {
                        using (var reader = message.GetReader())
                        {
                            var classicVotes = reader.ReadByte();
                            var teamVotes = reader.ReadByte();
                            var practiceVotes = reader.ReadByte();

                            _classicVoteText.text = classicVotes.ToString();
                            _teamVoteText.text = teamVotes.ToString();
                            _practiceVoteText.text = practiceVotes.ToString();
                        }
                        break;
                    }
                    // Voting finished, start game
                    case VotingTags.StartGame:
                    {
                        if (_currentCountdown != null)
                        {
                            Destroy(_currentCountdown.gameObject);
                        }

                            // Set GameMode
                        using (var reader = message.GetReader())
                        {
                            GameControl.GameState.SetGameMode = (GameMode)reader.ReadByte();
                        }

                        // Start Game
                        gameObject.SetActive(false);
                        _setupNetwork.Game.SetActive(true);
                        break;
                    }
                }
            }
        }

        private void OnCountdown(ushort counter)
        {
            if (_currentCountdown != null)
            {
                Destroy(_currentCountdown.gameObject);
            }
            if (counter == 0)
                return;

            _currentCountdown = Instantiate(_countdownPrefab, _votingCanvas.transform);
            _currentCountdown.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 150);
            _currentCountdown.GetComponent<TextMeshProUGUI>().text = counter.ToString();
        }
    }
}
