using System.Collections;
using Characters;
using Characters.Bars;
using Launcher;
using Players;
using Players.AbilitiesButtons;
using Players.Camera;
using RLR.Levels;
using TMPro;
using UI.RLR_Menus;
using UnityEngine;

namespace RLR
{
    public class InitializeGameRLR : MonoBehaviour
    {

        // Attach scripts

        public InGameMenuManagerRLR InGameMenuManager;
        public CameraHandleMovement CameraHandleMovement;
        public PlayerFactory PlayerFactory;
        public PlayerBarsManager PlayerBarsManager;
        public RunlingChaser RunlingChaser;
        public AbilityButtonManager AbilityButtonManager;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;

        private LevelManagerRLR _levelManager;
        private ControlRLR _controlRLR;
        private ScoreRLR _scoreRLR;

        private void Awake()
        {
            _levelManager = GetComponent<LevelManagerRLR>();
            _controlRLR = GetComponent<ControlRLR>();
            _scoreRLR = GetComponent<ScoreRLR>();
        }

        public void InitializePlayer()
        {
            var playerManager = PlayerFactory.Create(GameControl.GameState.CharacterDto);
            _controlRLR.PlayerManager = playerManager;
            GetComponent<InputServer>().Init(InGameMenuManager.gameObject, playerManager);
            PlayerBarsManager.Initialize(playerManager);
            playerManager.Trigger.AddComponent<SafeZoneManager>().InitializeTrigger(PlayerBarsManager, RunlingChaser);
            AbilityButtonManager.InitializeAbilityButtons(playerManager);
        }

        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            _controlRLR.CheckIfFinished = true;
            StartCoroutine(PrepareLevel());
        }

        private IEnumerator PrepareLevel()
        {
            // load map
            _levelManager.GenerateMap(GameControl.GameState.CurrentLevel);

            // Load player
            SpawnPlayer(_controlRLR.PlayerManager);

            // set camera
            GameControl.Settings.CameraRange = _levelManager.MapGenerator.GetAirColliderRange() / 2.5f;
            CameraHandleMovement.SetCameraHandlePosition(
                new Vector3(_controlRLR.PlayerManager.transform.localPosition.x, 0,
                    _controlRLR.PlayerManager.transform.localPosition.z));

            // generate drones
            _levelManager.GenerateChasers(GameControl.GameState.CurrentLevel);
            _levelManager.LoadDrones(GameControl.GameState.CurrentLevel);

            // Show current level
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.GameState.CurrentLevel;
            LevelTextObject.SetActive(true);
            yield return new WaitForSeconds(2);
            LevelTextObject.SetActive(false);

            StartCoroutine(StartCountdown());
        }

        private void SpawnPlayer(PlayerManager playerManager)
        {
            var startPlatform = GameControl.MapState.SafeZones[0];

            playerManager.IsDead = false;
            playerManager.IsImmobile = true;
            playerManager.IsInvulnerable = true;
            playerManager.CheckIfDead = true;
            playerManager.Model.SetActive(true);

            playerManager.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            playerManager.transform.position = new Vector3(
                startPlatform.transform.position.x + startPlatform.transform.Find("VisibleObjects/Ground")
                    .transform.localScale.x / 2 - 1,
                0, startPlatform.transform.position.z);
        }

        private IEnumerator StartCountdown()
        {
            yield return new WaitForSeconds(1);

            // Countdown
            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, GameObject.Find("Canvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(1);
                Destroy(countdown);
            }

            StartLevel();
        }

        private void StartLevel()
        {
            _controlRLR.PlayerManager.IsInvulnerable = false;
            _controlRLR.PlayerManager.IsImmobile = false;
            _scoreRLR.StartTimer();
        }
    }
}
