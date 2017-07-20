using System.Collections;
using Launcher;
using TMPro;
using UnityEngine;

namespace SLA
{
    public class VoteGameModeSLA : MonoBehaviour
    {
        [SerializeField] private GameObject _countdownPrefab;
        [SerializeField] private GameObject _mode;
        private Gamemode? _voteGameMode;
        private bool _finished;

        private void Start()
        {
            StartCoroutine(Voting());
        }

        public void VoteModeClassic()
        {
            _voteGameMode = Gamemode.Classic;
        }

        public void VoteModeTeam()
        {
        //    _voteGameMode = Gamemode.Team;
        }

        public void VoteModePractice()
        {
            _voteGameMode = Gamemode.Practice;
        }

        public void Finish()
        {
            _mode.SetActive(false);
            _finished = true;
        }

        private void StartGame()
        {

            GameControl.GameState.SetGameMode = _voteGameMode ?? Gamemode.Classic;
            PhotonNetwork.LoadLevel(5);
        }

        private void Update()
        {
            if (_finished)
                StartGame();

        }

        private IEnumerator Voting()
        {
            const int countDownFrom = 5;
            // Countdown
            for (var i = 0; i < countDownFrom; i++)
            {
                var countdown = Instantiate(_countdownPrefab, GameObject.Find("Canvas").transform);
                countdown.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 150);
                //countdown.GetComponent<RectTransform>().localScale = new Vector2(0.7f, 0.7f);
                countdown.GetComponent<TextMeshProUGUI>().text = (countDownFrom - i).ToString();
                yield return new WaitForSeconds(1);
                Destroy(countdown);
            }

            _mode.SetActive(false);
            _finished = true;
        }
    }
}