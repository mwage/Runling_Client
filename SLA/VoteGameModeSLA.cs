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
        [SerializeField] private GameObject _finishButton;
        [SerializeField] private Text _classicVoteText;
        [SerializeField] private Text _teamVoteText;
        [SerializeField] private Text _practiceVoteText;

        public PhotonView PhotonView;
        private GameMode?[] _votes;
        private int _classicVotes;
        private int _teamVotes;
        private int _practiceVotes;
        private Dictionary<GameMode, int> _votesPerMode;
        private bool _starting;

        public VoteGameModeSLA(bool starting)
        {
            _starting = starting;
        }

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            _votes = new GameMode?[PhotonNetwork.room.PlayerCount];
            _votesPerMode = new Dictionary<GameMode, int>
            {
                {GameMode.Classic, _classicVotes},
                {GameMode.Team, _teamVotes},
                {GameMode.Practice, _practiceVotes}
            };
        }

        [PunRPC]
        private void StartVoting()
        {
            _networkManagerSLA.Voting = true;
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
            if (GameControl.PlayerState.SyncVars == null || _starting)
                return;
            if (GameControl.PlayerState.SyncVars.Where(state => state != null).Any(state => !state.FinishedVoting))
                return;

            _starting = true;
            if (PhotonNetwork.isMasterClient)
                PhotonView.RPC("StartGame", PhotonTargets.AllViaServer, SetGameMode());
        }

        #region Buttons
        public void VoteModeClassic()
        {
            ChangeVote(GameMode.Classic, PhotonNetwork.player.ID);
        }

        public void VoteModeTeam()
        {
            ChangeVote(GameMode.Team, PhotonNetwork.player.ID);
        }

        public void VoteModePractice()
        {
            ChangeVote(GameMode.Practice, PhotonNetwork.player.ID);
        }
        #endregion

        private void ChangeVote(GameMode mode, int playerID)
        {
            PhotonView.RPC("SubmitVote", PhotonTargets.All, mode, playerID);
        }

        [PunRPC]
        private void SubmitVote(GameMode mode, int playerID)
        {
            Debug.Log(GameControl.PlayerState.SyncVars[playerID - 1].Owner.NickName + " voted for " + mode);
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
                    case GameMode.Classic:
                        _classicVotes++;
                        break;
                    case GameMode.Team:
                        _teamVotes++;
                        break;
                    case GameMode.Practice:
                        _practiceVotes++;
                        break;
                }
            }

            _classicVoteText.text = _classicVotes.ToString();
            _teamVoteText.text = _teamVotes.ToString();
            _practiceVoteText.text = _practiceVotes.ToString();

            _votesPerMode[GameMode.Classic] = _classicVotes;
            _votesPerMode[GameMode.Team] = _teamVotes;
            _votesPerMode[GameMode.Practice] = _practiceVotes;
        }

        public void Finish()
        {
            _finishButton.SetActive(false);
            PhotonView.RPC("FinishedVoting", PhotonTargets.AllViaServer, PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void FinishedVoting(int playerID)
        {
            Debug.Log(GameControl.PlayerState.SyncVars[playerID - 1].Owner.NickName + " is ready");
            GameControl.PlayerState.SyncVars[playerID - 1].FinishedVoting = true;
        }

        [PunRPC]
        private void StartGame(GameMode gameMode)
        {
            _starting = true;
            GameControl.GameState.SetGameMode = gameMode;
            transform.parent.gameObject.SetActive(false);
            _networkManagerSLA.Game.SetActive(true);
        }

        private GameMode SetGameMode()
        {
            var random = new System.Random();
            var max = new[] { _classicVotes, _teamVotes, _practiceVotes }.Max();
            var mostVotes = _votesPerMode.Keys.Where(mode => _votesPerMode[mode] == max).ToList();
            var idx = mostVotes.Count == 1 ? 0 : random.Next(0, mostVotes.Count);
            return mostVotes[idx];
        }
    }
}