using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Launcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SLA
{
    public class VoteGameModeSLA : MonoBehaviour
    {
        [SerializeField] private GameObject _countdownPrefab;
        [SerializeField] private GameObject _mode;
        [SerializeField] private GameObject _finishButton;
        public Text _classicVoteText;
        public Text _teamVoteText;
        public Text _practiceVoteText;

        private PhotonView _photonView;
        private bool[] _finishedVoting;
        private Gamemode?[] _votes;
        private PhotonPlayer[] _playerList;
        private int _classicVotes;
        private int _teamVotes;
        private int _practiceVotes;
        private Dictionary<Gamemode, int> _votesPerMode = new Dictionary<Gamemode, int>();

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _finishedVoting = new bool[PhotonNetwork.room.PlayerCount];
            _votes = new Gamemode?[PhotonNetwork.room.PlayerCount];
            _playerList = new PhotonPlayer[PhotonNetwork.room.PlayerCount];
            _votesPerMode.Add(Gamemode.Classic, _classicVotes);
            _votesPerMode.Add(Gamemode.Team, _teamVotes);
            _votesPerMode.Add(Gamemode.Practice, _practiceVotes);
        }

        private void Start()
        {
            foreach (var player in PhotonNetwork.playerList)
            {
                _playerList[player.ID - 1] = player;
            }
            foreach (var player in _playerList)
            {
                Debug.Log(player.NickName + ": " + player.ID);
            }

            if (PhotonNetwork.isMasterClient)
            {
                _photonView.RPC("StartVoting", PhotonTargets.All);
            }
        }

        [PunRPC]
        private void StartVoting()
        {
            StartCoroutine(Voting());
        }

        private IEnumerator Voting()
        {
            const int countDownFrom = 30;
            // Countdown
            for (var i = 0; i < countDownFrom; i++)
            {
                var countdown = Instantiate(_countdownPrefab, GameObject.Find("Canvas").transform);
                countdown.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 150);
                countdown.GetComponent<TextMeshProUGUI>().text = (countDownFrom - i).ToString();
                yield return new WaitForSeconds(1);
                Destroy(countdown);
            }

            Finish();
        }

        private void Update()
        {
            if (_finishedVoting.Any(finished => !finished))
                return;

            if (PhotonNetwork.isMasterClient)
                StartGame();
        }

        private void StartGame()
        {
            _photonView.RPC("SetGameMode", PhotonTargets.All);
            PhotonNetwork.LoadLevel(5);
        }

        #region Buttons
        public void VoteModeClassic()
        {
            ChangeVote(Gamemode.Classic, PhotonNetwork.player.ID);
        }

        public void VoteModeTeam()
        {
            ChangeVote(Gamemode.Team, PhotonNetwork.player.ID);
        }

        public void VoteModePractice()
        {
            ChangeVote(Gamemode.Practice, PhotonNetwork.player.ID);
        }
        #endregion

        private void ChangeVote(Gamemode mode, int playerID)
        {
            _photonView.RPC("SubmitVote", PhotonTargets.All, mode, playerID);
        }

        [PunRPC]
        private void SubmitVote(Gamemode mode, int playerID)
        {
            Debug.Log(_playerList[playerID - 1].NickName + " voted for " + mode);
            _votes[playerID - 1] = mode;
            UpdateVotes();
        }

        private void UpdateVotes()
        {
            _classicVotes = 0;
            _teamVotes = 0;
            _practiceVotes = 0;

            foreach (var vote in _votes)
            {
                switch (vote)
                {
                    case Gamemode.Classic:
                        _classicVotes++;
                        break;
                    case Gamemode.Team:
                        _teamVotes++;
                        break;
                    case Gamemode.Practice:
                        _practiceVotes++;
                        break;
                }
            }

            _classicVoteText.text = _classicVotes.ToString();
            _teamVoteText.text = _teamVotes.ToString();
            _practiceVoteText.text = _practiceVotes.ToString();

            _votesPerMode[Gamemode.Classic] = _classicVotes;
            _votesPerMode[Gamemode.Team] = _teamVotes;
            _votesPerMode[Gamemode.Practice] = _practiceVotes;
        }

        public void Finish()
        {
            _finishButton.SetActive(false);
            _photonView.RPC("FinishedVoting", PhotonTargets.All, PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void FinishedVoting(int playerID)
        {
            Debug.Log(_playerList[playerID - 1].NickName + " is ready");
            _finishedVoting[playerID - 1] = true;
        }

        [PunRPC]
        private void SetGameMode()
        {
            _mode.SetActive(false);
            var random = new System.Random();
            var max = new[] {_classicVotes, _teamVotes, _practiceVotes}.Max();
            var mostVotes = _votesPerMode.Keys.Where(mode => _votesPerMode[mode] == max).ToList();
            var idx = mostVotes.Count == 1 ? 0 : random.Next(0, mostVotes.Count);
            GameControl.GameState.SetGameMode = mostVotes[idx];
            GameControl.PlayerState.IsDead = true;
            GameControl.GameState.CurrentLevel = 1;
            Debug.Log(GameControl.GameState.SetGameMode + " selected!");
        }
    }
}