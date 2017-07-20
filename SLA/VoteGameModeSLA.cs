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
        [SerializeField] private NetworkManagerSLA _networkManagerSLA;

        [SerializeField] private GameObject _countdownPrefab;
        [SerializeField] private GameObject _mode;
        [SerializeField] private GameObject _finishButton;
        [SerializeField] private GameObject _game;
        [SerializeField] private Text _classicVoteText;
        [SerializeField] private Text _teamVoteText;
        [SerializeField] private Text _practiceVoteText;

        public PhotonView PhotonView;
        private Gamemode?[] _votes;
        private int _classicVotes;
        private int _teamVotes;
        private int _practiceVotes;
        private Dictionary<Gamemode, int> _votesPerMode;
        private bool _starting;

        public VoteGameModeSLA(bool starting)
        {
            _starting = starting;
        }

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            _votes = new Gamemode?[PhotonNetwork.room.PlayerCount];
            _votesPerMode = new Dictionary<Gamemode, int>();
            _votesPerMode.Add(Gamemode.Classic, _classicVotes);
            _votesPerMode.Add(Gamemode.Team, _teamVotes);
            _votesPerMode.Add(Gamemode.Practice, _practiceVotes);
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
                var countdown = Instantiate(_countdownPrefab, transform);
                countdown.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 150);
                countdown.GetComponent<TextMeshProUGUI>().text = (countDownFrom - i).ToString();
                yield return new WaitForSeconds(1);
                Destroy(countdown);
            }

            Finish();
        }

        private void Update()
        {
            if (!PhotonNetwork.isMasterClient || _networkManagerSLA.PlayerState == null || _starting)
                return;
            if (_networkManagerSLA.PlayerState.Any(state => !state.FinishedVoting))
                return;
            PhotonView.RPC("StartGame", PhotonTargets.All);
            _starting = true;
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
            PhotonView.RPC("SubmitVote", PhotonTargets.All, mode, playerID);
        }

        [PunRPC]
        private void SubmitVote(Gamemode mode, int playerID)
        {
            Debug.Log(_networkManagerSLA.PlayerList[playerID - 1].NickName + " voted for " + mode);
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
            PhotonView.RPC("FinishedVoting", PhotonTargets.All, PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void FinishedVoting(int playerID)
        {
            Debug.Log(_networkManagerSLA.PlayerList[playerID - 1].NickName + " is ready");
            _networkManagerSLA.PlayerState[playerID - 1].FinishedVoting = true;
        }

        [PunRPC]
        private void StartGame()
        {
            //_mode.SetActive(false);
            var random = new System.Random();
            var max = new[] {_classicVotes, _teamVotes, _practiceVotes}.Max();
            var mostVotes = _votesPerMode.Keys.Where(mode => _votesPerMode[mode] == max).ToList();
            var idx = mostVotes.Count == 1 ? 0 : random.Next(0, mostVotes.Count);
            GameControl.GameState.SetGameMode = mostVotes[idx];
            GameControl.PlayerState.IsDead = true;
            GameControl.GameState.CurrentLevel = 1;
            transform.parent.gameObject.SetActive(false);
            _game.SetActive(true);
        }
    }
}