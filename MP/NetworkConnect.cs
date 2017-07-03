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
        public Button ConnectButton;
        public GameObject CancelButton;
        public Toggle AutoConnect;

        private bool _autoConnect;

        private void Update()
        {
            if (PhotonNetwork.playerName.Length >= 2 && !ConnectButton.IsInteractable())
            {
                ConnectButton.interactable = true;
            }
            if (PhotonNetwork.playerName.Length < 2 && ConnectButton.IsInteractable())
            {
                ConnectButton.interactable = false;
            }
        }

        private void Awake()
        {
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = true;
            _autoConnect = PlayerPrefs.GetInt("AutoConnect") != 0;
            AutoConnect.isOn = _autoConnect;
            CancelButton.SetActive(false);
        }
        
        private void Start()
        {
            if (AutoConnect.isOn)
            {
                Connect();
            }
        }

        public void Connect()
        {
            if (PhotonNetwork.playerName.Length < 2)
            {
                return;
            }
            FeedbackText.text = "";
            ControlPanel.SetActive(false);
            CancelButton.SetActive(true);
            LoadingAnimation.StartLoaderAnimation();

            if (PhotonNetwork.connected)
            {
                FeedbackText.text = "Connected!";
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                FeedbackText.text = "Connecting...";
                PhotonNetwork.ConnectUsingSettings(GameControl.Version.ToString());
            }
        }

        #region Buttons

        public void OfflineMode()
        {
            PhotonNetwork.offlineMode = true;
            SceneManager.LoadScene("MainMenu");
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
            FeedbackText.text = "";
            CancelButton.SetActive(false);
            LoadingAnimation.StopLoaderAnimation();
            ControlPanel.SetActive(true);
        }

        public void AutoConnectValue(bool selected)
        {
            _autoConnect = selected;
            PlayerPrefs.SetInt("AutoConnect", selected ? 1 : 0);
        }
        #endregion

        public override void OnConnectedToMaster()
        {
            CancelButton.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        }
    }
}