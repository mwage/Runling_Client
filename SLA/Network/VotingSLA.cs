using DarkRift;
using DarkRift.Client;
using Launcher;
using Network.DarkRiftTags;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SLA.Network
{
    public class VotingSLA : MonoBehaviour
    {
        [SerializeField] private NetworkManagerSLA _networkManager;
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
            _networkManager.GameClient.MessageReceived += OnDataHandler;
        }

        private void OnDisable()
        {
            _networkManager.GameClient.MessageReceived -= OnDataHandler;
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

            Debug.Log("Sending: " + gameMode);
            var writer = new DarkRiftWriter();
            writer.Write((byte)gameMode);

            _networkManager.GameClient.SendMessage(new TagSubjectMessage(Tags.Voting, VotingSubjects.SubmitVote, writer), SendMode.Reliable);
            _currentGameMode = gameMode;
        }

        public void Finish()
        {
            _finishButton.gameObject.SetActive(false);
            _networkManager.GameClient.SendMessage(new TagSubjectMessage(Tags.Voting, VotingSubjects.FinishVoting, new DarkRiftWriter()), SendMode.Reliable);
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
                Debug.Log("received votes");
                var reader = message.GetReader();
                var classicVotes = reader.ReadByte();
                var teamVotes = reader.ReadByte();
                var practiceVotes = reader.ReadByte();

                Debug.Log(classicVotes + " - " + teamVotes + " - " + practiceVotes);

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
                _networkManager.Game.SetActive(true);
            }
            // Countdown update
            else if (message.Subject == VotingSubjects.Countdown)
            {
                var reader = message.GetReader();
                var newNumber = reader.ReadByte();
                if (_currentCountdown != null)
                {
                    Destroy(_currentCountdown.gameObject);
                }
                _currentCountdown = Instantiate(_countdownPrefab, _votingCanvas.transform);
                _currentCountdown.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 150);
                _currentCountdown.GetComponent<TextMeshProUGUI>().text = newNumber.ToString();
            }
        }
    }
}
