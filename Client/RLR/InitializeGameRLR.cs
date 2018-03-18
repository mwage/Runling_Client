using Client.Scripts.IngameCamera;
using Client.Scripts.Launcher;
using Client.Scripts.PlayerInput;
using Client.Scripts.PlayerInput.Buttons;
using Client.Scripts.UI.StatusBars;
using Game.Scripts.Characters;
using Game.Scripts.Characters.CharacterRepositories;
using Game.Scripts.Network.Data;
using Game.Scripts.Players;
using Game.Scripts.RLR;
using TMPro;
using UnityEngine;

namespace Client.Scripts.RLR
{
    public class InitializeGameRLR : MonoBehaviour
    {
        public CameraMovement CameraMovement;
        public CharacterBuilder CharacterBuilder;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public GameObject LivesText;

        private LevelManagerRLR _levelManager;
        private ControlRLR _controlRLR;
        private ScoreRLR _scoreRLR;
        private RunlingChaser _runlingChaser;
        private PlayerBarsManager _playerBarsManager;
        private ButtonManager _buttonManager;
        private SafeZoneManager _safeZoneManager;
        private GameObject _currentCountdown;
        
        private void Awake()
        {
            _levelManager = GetComponent<LevelManagerRLR>();
            _controlRLR = GetComponent<ControlRLR>();
            _scoreRLR = GetComponent<ScoreRLR>();
            _runlingChaser = GetComponent<RunlingChaser>();
            _playerBarsManager = CharacterBuilder.GetComponent<PlayerBarsManager>();
            _buttonManager = CharacterBuilder.GetComponent<ButtonManager>();
        }

        public PlayerManager InitializePlayer(Player player)
        {
            var playerManager = CharacterBuilder.Create(GameControl.GameState.CharacterDto, new CharacterRepositoryPlayerPrefs());

            // TODO: for MP: check if it's the player first
            _buttonManager.InitializeAbilityButtons(playerManager.CharacterManager);
            playerManager.Player = player;

            return playerManager;
        }

        public void InitializeControls(PlayerManager playerManager)
        {
            GetComponent<InputManager>().Initialize(playerManager);
            CameraMovement.InitializeFollowTarget(playerManager.gameObject);
            _playerBarsManager.Initialize(playerManager);
        }

        public void InitializeSafeZones(PlayerManager playerManager)
        {
            _safeZoneManager = playerManager.Trigger.AddComponent<SafeZoneManager>();
            _safeZoneManager.InitializeTrigger(_playerBarsManager, _runlingChaser);
        }

        public void ChangeLives(PlayerManager playerManager, int lives)
        {
            playerManager.Lives = lives;
            LivesText.GetComponent<TextMeshProUGUI>().text = "Lives remaining: " + lives;
        }

        public void PrepareLevel()
        {
            // load map
            _levelManager.GenerateMap(_controlRLR.CurrentLevel);
            _levelManager.GenerateChasers(_controlRLR.CurrentLevel);
            _runlingChaser.InitializeChaserPlatforms(_safeZoneManager);

            // set camera
            GameControl.Settings.CameraRange = _levelManager.MapGenerator.GetAirColliderRange() / 2.5f;

            // Show current level
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + _controlRLR.CurrentLevel;
            LevelTextObject.SetActive(true);
        }

        public void SetCameraPosition(PlayerManager playerManager)
        {
            CameraMovement.SetCameraPosition(
                new Vector3(playerManager.transform.localPosition.x, 0,
                    playerManager.transform.localPosition.z));
        }

        public void SpawnPlayer(PlayerManager playerManager)
        {
            var startPlatform = GameControl.GameState.SafeZones[0];

            playerManager.IsDead = false;
            playerManager.IsImmobile = true;
            playerManager.IsInvulnerable = true;
            playerManager.Model.SetActive(true);

            playerManager.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            playerManager.transform.position = new Vector3(
                startPlatform.transform.position.x + startPlatform.transform.Find("VisibleObjects/Ground")
                    .transform.localScale.x / 2 - 1,
                0, startPlatform.transform.position.z);

            _safeZoneManager.VisitedSafeZones = new bool[GameControl.GameState.SafeZones.Count];
        }

        public void HideText()
        {
            LevelTextObject.SetActive(false);
        }

        public void Countdown(int counter)
        {
            if (_currentCountdown != null)
            {
                Destroy(_currentCountdown);
            }

            if (counter == 0)
                return;

            _currentCountdown = Instantiate(CountdownPrefab, GameObject.Find("Canvas").transform);
            _currentCountdown.GetComponent<TextMeshProUGUI>().text = counter.ToString();
        }

        public void StartLevel(PlayerManager playerManager)
        {
            playerManager.IsInvulnerable = false;
            playerManager.IsImmobile = false;
            _scoreRLR.StartTimer();
        }
    }
}