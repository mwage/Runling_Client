using Launcher;
using UnityEngine;
using UnityEngine.UI;

namespace MP.TSGame.SLA
{
    public class VoteGameModeSLA : MonoBehaviour
    {
        [SerializeField] private VotingManagerSLA _votingManager;
        [SerializeField] private GameObject _finishButton;
        [SerializeField] private Text _classicVoteText;
        [SerializeField] private Text _teamVoteText;
        [SerializeField] private Text _practiceVoteText;

        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        #region Buttons
        public void VoteModeClassic()
        {
            _photonView.RPC("SubmitVote", PhotonTargets.All, GameMode.Classic, PhotonNetwork.player.ID);
        }

        public void VoteModeTeam()
        {
            _photonView.RPC("SubmitVote", PhotonTargets.All, GameMode.Team, PhotonNetwork.player.ID);
        }

        public void VoteModePractice()
        {
            _photonView.RPC("SubmitVote", PhotonTargets.All, GameMode.Practice, PhotonNetwork.player.ID);
        }

        public void Finish()
        {
            _finishButton.SetActive(false);
            _photonView.RPC("FinishedVoting", PhotonTargets.AllViaServer, PhotonNetwork.player.ID);
        }
        #endregion

        [PunRPC]
        private void SubmitVote(GameMode mode, int playerID)
        {
            Debug.Log(PhotonNetwork.playerList[playerID - 1] + " voted for " + mode);
            _votingManager.Votes[playerID - 1] = mode;
            _votingManager.UpdateVotes();
        }

        public void SetText(int classicVotes, int teamVotes, int practiceVotes)
        {
            _classicVoteText.text = classicVotes.ToString();
            _teamVoteText.text = teamVotes.ToString();
            _practiceVoteText.text = practiceVotes.ToString();
        }

        [PunRPC]
        private void FinishedVoting(int playerID)
        {
            Debug.Log(PhotonNetwork.playerList[playerID - 1] + " is ready");
            _votingManager.FinishedVoting[playerID - 1] = true;
        }
    }
}