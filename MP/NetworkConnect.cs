
using Launcher;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MP
{
    public class NetworkConnect : Photon.PunBehaviour
    {
        public GameObject ControlPanel;
        public Text FeedbackText;
        public LoadingAnimation LoadingAnimation;

        private void Awake()
        {
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = true;
        }

        public void Connect()
        {
            FeedbackText.text = "";
            ControlPanel.SetActive(false);

            LoadingAnimation.StartLoaderAnimation();

            if (PhotonNetwork.connected)
            {
                FeedbackText.text = "Connected!";
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
                PhotonNetwork.JoinRandomRoom(null, (byte)(2));
            }
            else
            {
                FeedbackText.text = "Connecting...";
                PhotonNetwork.ConnectUsingSettings(GameControl.Version.ToString());
            }
        }


        #region Network CallBacks

        public override void OnConnectedToMaster()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public override void OnDisconnectedFromPhoton()
        {
            FeedbackText.text = "<Color=Red>Failed to Connect</Color>";

            LoadingAnimation.StopLoaderAnimation();
            ControlPanel.SetActive(true);
        }

        #endregion
    }
}