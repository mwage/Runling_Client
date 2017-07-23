using System.Collections;
using Characters;
using Characters.Types;
using Launcher;
using Players.Camera;
using TMPro;
using UI.SLA_Menus;
using UnityEngine;
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
            {
                ControlSLA.LevelManager.LoadDrones(GameControl.GameState.CurrentLevel);
                _photonView.RPC("StartLevel", PhotonTargets.AllViaServer);
            }
        }


        [PunRPC]
        private void StartLevel(PhotonMessageInfo info)
        {
            SpawnPlayer();

            if (GameControl.PlayerState.GodModeActive && !GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.activeSelf)
            {
                GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(true);

            }
            ControlSLA.CheckIfDead = true;
            ControlSLA.CheckIfAllDead = true;
            GameControl.GameState.AllDead = false;
            ControlSLA.LoadingNextLevel = false;

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

        private void SpawnPlayer()
        {
            if (GameControl.PlayerState.Player == null)
            {
                GameControl.PlayerState.CharacterDto = new CharacterDto(0, "Arena", 0, 0, 0, 0, 1, 0, 0);
                GameControl.PlayerState.Player = PlayerFactory.Create(GameControl.PlayerState.CharacterDto);
            }
            GameControl.PlayerState.Player.SetActive(true);

            GameControl.PlayerState.Player.transform.Find("Shield").gameObject.SetActive(true);
            _photonView.RPC("InitializePlayer", PhotonTargets.All, PhotonNetwork.player.ID);

            GameControl.PlayerState.CharacterController.Speed.SetBaseSpeed(ControlSLA.LevelManager.GetMovementSpeed(GameControl.GameState.CurrentLevel));
            GameControl.PlayerState.Player.transform.position = StartingPosition();
            GameControl.PlayerState.Player.transform.rotation = PhotonNetwork.room.PlayerCount != 1 ? 
                Quaternion.LookRotation(Vector3.zero - StartingPosition()) : Quaternion.identity;
        }

        [PunRPC]
        private void InitializePlayer(int playerID)
        {
            GameControl.PlayerState.SyncVars[playerID - 1].IsDead = false;
            GameControl.PlayerState.SyncVars[playerID - 1].IsSafe = false;
            GameControl.PlayerState.SyncVars[playerID - 1].IsImmobile = false;
            GameControl.PlayerState.SyncVars[playerID - 1].IsInvulnerable = true;
        }

        [PunRPC]
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
                _photonView.RPC("StartScore", PhotonTargets.AllViaServer);
        }
        
        [PunRPC]
        private void StartScore()
        {
            GameControl.PlayerState.Player.transform.Find("Shield").gameObject.SetActive(false);
            foreach (var state in GameControl.PlayerState.SyncVars)
            {
                if (state != null)
                    state.IsInvulnerable = false;
            }
            ControlSLA.Score.StartScore();
        }
    }
}
