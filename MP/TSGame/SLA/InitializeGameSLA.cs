using System.Collections;
using Characters.Types;
using Launcher;
using MP.TSGame.Players;
using MP.TSGame.Players.Camera;
using TMPro;
using TrueSync;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.UI;

namespace MP.TSGame.SLA
{
    public class InitializeGameSLA : TrueSyncBehaviour
    {
        public InGameMenuManagerSLA InGameMenuManager;
        public CameraHandleMovement CameraHandleMovement;
        public PlayerFactory PlayerFactory;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public GameObject CurrentPrWindow;
        public Text CurrentPr;

        private ControlSLA _controlSLA;
        private LevelManagerSLA _levelManager;


        private void Awake()
        {
            _controlSLA = GetComponent<ControlSLA>();
            _levelManager = GetComponent<LevelManagerSLA>();
        }

        public void InitializeGame()
        {
            TrueSyncManager.SyncedStartCoroutine(PrepareLevel());
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
            yield return 3;
            LevelTextObject.SetActive(false);
            CurrentPrWindow.SetActive(false);
            yield return 1;

            StartLevel();
        }

        private void StartLevel()
        {
            SpawnPlayer();
            _levelManager.LoadDrones(GameControl.GameState.CurrentLevel);

            _controlSLA.CheckDeaths = true;
            _controlSLA.AllDead = false;
            _controlSLA.LoadingNextLevel = false;

            TrueSyncManager.SyncedStartCoroutine(StartCountdown());
        }

        private static TSVector StartingPosition(int id)
        {
            if (TrueSyncManager.Players.Count == 1)
            {
                return TSVector.zero;
            }

            return TSVector.zero + TSQuaternion.Euler
                       (0, 360f * (id - 1) / TrueSyncManager.Players.Count, 0) * TSVector.right * 2;
        }

        private void SpawnPlayer()
        {
            foreach (var player in TrueSyncManager.Players)
            {
                if (_controlSLA.PlayerManager[player.Id - 1] == null)
                {
                    _controlSLA.PlayerManager[player.Id - 1] = PlayerFactory.Create(new CharacterDto(0, "Arena", 0, 0, 0, 0, 1, 0, 0), player).GetComponent<PlayerManager>();
                }
                var playerManager = _controlSLA.PlayerManager[player.Id - 1];
                InitializePlayer(playerManager);

                playerManager.CharacterController.Speed.SetBaseSpeed((float)_levelManager.GetMovementSpeed(GameControl.GameState.CurrentLevel));
                playerManager.tsTransform.position = StartingPosition(player.Id);
                playerManager.tsTransform.rotation = TrueSyncManager.Players.Count != 1 ?
                    TSQuaternion.LookRotation(TSVector.zero - StartingPosition(player.Id)) : TSQuaternion.identity;
            }
        }

        private static void InitializePlayer(PlayerManager playerManager)
        {
            playerManager.IsDead = false;
            playerManager.IsSafe = false;
            playerManager.IsImmobile = false;
            playerManager.IsInvulnerable = true;
            playerManager.CheckIfDead = true;
            playerManager.Model.SetActive(true);
            playerManager.Shield.SetActive(true);
            playerManager.Trigger.SetActive(true);

            if(playerManager.GodModeActive)
                playerManager.GodMode.SetActive(true);
        }

        private IEnumerator StartCountdown()
        {
            yield return 2;

            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, _controlSLA.Score.transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return 1;
                Destroy(countdown);
            }

            StartScore();
        }
        
        private void StartScore()
        {
            foreach (var playerManager in _controlSLA.PlayerManager)
            {
                playerManager.Shield.SetActive(false);
                playerManager.IsInvulnerable = false;
            }

            _controlSLA.Score.StartScore();
        }
    }
}
