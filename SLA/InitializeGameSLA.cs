using System.Collections;
using System.IO;
using Characters;
using Characters.Types;
using Launcher;
using Players.Camera;
using TMPro;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SLA
{
    public class InitializeGameSLA : MonoBehaviour
    {
        public ControlSLA ControlSLA;

        public InGameMenuManagerSLA InGameMenuManager;
        public CameraHandleMovement CameraHandleMovement;

        public GameObject PlayerPrefab;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public GameObject CurrentPrWindow;
        public Text CurrentPr;
        public PlayerFactory PlayerFactory;

        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            StartCoroutine(PrepareLevel());
        }

        private IEnumerator PrepareLevel()
        {
            // Set current movespeed and cameraposition

            // TODO: GetMovespeed from SLA character
            GameControl.PlayerState.MoveSpeed = ControlSLA.LevelManager.GetMovementSpeed(GameControl.GameState.CurrentLevel);
            CameraHandleMovement.SetCameraHandlePosition(Vector3.zero);

            // Show level highscore and current level
            CurrentPr.text = GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel].ToString();
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.GameState.CurrentLevel;
            LevelTextObject.SetActive(true);
            CurrentPrWindow.SetActive(true);
            yield return new WaitForSeconds(2);
            LevelTextObject.SetActive(false);
            CurrentPrWindow.SetActive(false);
            yield return new WaitForSeconds(1);

            // Spawn Drones
            if (PhotonNetwork.isMasterClient)
                ControlSLA.LevelManager.LoadDrones(GameControl.GameState.CurrentLevel);

            //Spawn players and start countdown
            if (PhotonNetwork.isMasterClient)
                _photonView.RPC("SpawnPlayers", PhotonTargets.All);
        }


        [PunRPC]
        private void SpawnPlayers()
        {
            if (ControlSLA.NetworkManager.PlayerState[PhotonNetwork.player.ID - 1].Player == null)
            {
                GameControl.PlayerState.CharacterDto = new CharacterDto(0, "Arena", 0, 0, 0, 0, 1, 0, 0);
                ControlSLA.NetworkManager.PlayerState[PhotonNetwork.player.ID - 1].Player =
                    PlayerFactory.Create(GameControl.PlayerState.CharacterDto);
            }

            ControlSLA.NetworkManager.PlayerState[PhotonNetwork.player.ID - 1].Player.transform.position = StartingPosition();
            ControlSLA.NetworkManager.PlayerState[PhotonNetwork.player.ID - 1].Player.transform.rotation =
                PhotonNetwork.room.PlayerCount != 1 ? Quaternion.LookRotation(Vector3.zero - StartingPosition()) : Quaternion.identity;

            foreach (var state in ControlSLA.NetworkManager.PlayerState)
            {
                state.IsDead = false;
                state.Player.transform.Find("Shield").gameObject.SetActive(true);
            }

            GameControl.PlayerState.IsInvulnerable = true;
            GameControl.PlayerState.IsSafe = false;
            GameControl.PlayerState.IsImmobile = false;

            // TODO: Might need this as RPC to others
            if (GameControl.PlayerState.GodModeActive && !GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.activeSelf)
            {
                ControlSLA.NetworkManager.PlayerState[PhotonNetwork.player.ID - 1].Player.transform.Find("GodMode").gameObject.SetActive(true);
            }

            ControlSLA.StopUpdate = false;

            StartCoroutine(StartCountdown());
        }

        private static Vector3 StartingPosition()
        {
            if (PhotonNetwork.room.PlayerCount == 1)
            {
                return Vector3.zero;
            }

            return Vector3.zero + Quaternion.Euler
                       (0, 360f * (PhotonNetwork.player.ID - 1) / PhotonNetwork.room.PlayerCount, 0) * Vector3.right * 2;
        }

        private IEnumerator StartCountdown()
        {
            // Countdown
            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, GameObject.Find("Canvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(1);
                Destroy(countdown);
            }

            if (PhotonNetwork.isMasterClient)
                _photonView.RPC("StartScore", PhotonTargets.All);
        }

        [PunRPC]
        private void StartScore()
        {
            foreach (var state in ControlSLA.NetworkManager.PlayerState)
            {
                state.Player.transform.Find("Shield").gameObject.SetActive(false);
            }

            GameControl.PlayerState.IsInvulnerable = false;
            ControlSLA.Score.StartScore();
        }
    }
}
