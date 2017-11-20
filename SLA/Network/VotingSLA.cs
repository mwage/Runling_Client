using DarkRift;
using DarkRift.Client;
using Launcher;
using Network;
using Network.DarkRiftTags;
using Network.Synchronization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SLA.Network
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

            var writer = new DarkRiftWriter();
            writer.Write((byte)gameMode);

            GameClient.Instance.SendMessage(new TagSubjectMessage(Tags.Voting, VotingSubjects.SubmitVote, writer), SendMode.Reliable);
            _currentGameMode = gameMode;
        }

        public void Finish()
        {
            _finishButton.gameObject.SetActive(false);
            GameClient.Instance.SendMessage(new TagSubjectMessage(Tags.Voting, VotingSubjects.FinishVoting, new DarkRiftWriter()), SendMode.Reliable);
        }

        #endregion

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.Voting)
                return;

            // Start Voting
            if (message.Subject == VotingSubjects.StartVoting)
            {
                _votingCanvas.SetActive(true);
            }
            // Votes changed
            else if (message.Subject == VotingSubjects.VoteUpdate)
            {
                var reader = message.GetReader();
                var classicVotes = reader.ReadByte();
                var teamVotes = reader.ReadByte();
                var practiceVotes = reader.ReadByte();

                _classicVoteText.text = classicVotes.ToString();
                _teamVoteText.text = teamVotes.ToString();
                _practiceVoteText.text = practiceVotes.ToString();
            }
            // Voting finished, starting the game
            else if (message.Subject == VotingSubjects.StartGame)
            {
                if (_currentCountdown != null)
                {
                    Destroy(_currentCountdown.gameObject);
                }

                // Set GameMode
                var reader = message.GetReader();
                GameControl.GameState.SetGameMode = (GameMode) reader.ReadByte();

                // Start Game
                gameObject.SetActive(false);
                _setupNetwork.Game.SetActive(true);
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
