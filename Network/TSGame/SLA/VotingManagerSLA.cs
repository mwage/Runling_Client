using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Launcher;
using TMPro;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.SLA
{
    public class VotingManagerSLA : TrueSyncBehaviour
    {
        [SerializeField] private VoteGameModeSLA _voteGameMode;
        [SerializeField] private GameObject _countdownPrefab;

        public GameMode?[] Votes;
        public bool?[] FinishedVoting;
        private int _classicVotes;
        private int _teamVotes;
        private int _practiceVotes;
        private Dictionary<GameMode, int> _votesPerMode;
        private bool _starting;

        private void Awake()
        {
            Votes = new GameMode?[PhotonNetwork.room.PlayerCount];

            FinishedVoting = new bool?[PhotonNetwork.room.PlayerCount];
            for (var i = 0; i < FinishedVoting.Length; i++)
            {
                FinishedVoting[i] = false;
            }

            _votesPerMode = new Dictionary<GameMode, int>
            {
                {GameMode.Classic, _classicVotes},
                {GameMode.Team, _teamVotes},
                {GameMode.Practice, _practiceVotes}
            };
        }

        public override void OnSyncedStart()
        {
            _voteGameMode.gameObject.SetActive(true);
            TrueSyncManager.SyncedStartCoroutine(Countdown(30));
        }

        public void UpdateVotes()
        {
            _classicVotes = 0;
            _teamVotes = 0;
            _practiceVotes = 0;

            foreach (var vote in Votes)
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

            _voteGameMode.SetText(_classicVotes, _teamVotes, _practiceVotes);

            _votesPerMode[GameMode.Classic] = _classicVotes;
            _votesPerMode[GameMode.Team] = _teamVotes;
            _votesPerMode[GameMode.Practice] = _practiceVotes;
        }

        public override void OnSyncedUpdate()
        {
            if (_starting)
                return;
            if (FinishedVoting.Any(x => x == false))
            {
                return;
            }

            _starting = true;
            GameControl.GameState.SetGameMode = SetGameMode();
            PhotonNetwork.LoadLevel(6);
        }

        private IEnumerator Countdown(int countDownFrom)
        {
            for (var i = 0; i < countDownFrom; i++)
            {
                var countdown = Instantiate(_countdownPrefab, _voteGameMode.transform);
                countdown.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 150);
                countdown.GetComponent<TextMeshProUGUI>().text = (countDownFrom - i).ToString();
                yield return 1;
                Destroy(countdown);
            }

            _voteGameMode.Finish();
        }

        private GameMode SetGameMode()
        {
            var max = new[] { _classicVotes, _teamVotes, _practiceVotes }.Max();
            var mostVotes = _votesPerMode.Keys.Where(mode => _votesPerMode[mode] == max).ToList();
            var idx = mostVotes.Count == 1 ? 0 : GameControl.GameState.Random.Next(0, mostVotes.Count);
            return mostVotes[idx];
        }

        private void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            FinishedVoting[player.ID - 1] = null;
            Debug.Log(player.NickName + " has left the game.");
        }
    }
}