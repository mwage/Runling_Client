using Client.Scripts.IngameCamera;
using Client.Scripts.Launcher;
using Client.Scripts.PlayerInput;
using Game.Scripts.Characters;
using Game.Scripts.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.SLA
{
    public class InitializeGameSLA : MonoBehaviour
    {
        public CameraMovement CameraMovement;
        public CharacterBuilder CharacterBuilder;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public GameObject CurrentPrWindow;
        public Text CurrentPr;

        private ControlSLA _controlSLA;
        private LevelManagerSLA _levelManager;
        private ScoreSLA _score;
        private GameObject _currentCountdown;


        private void Awake()
        {
            _controlSLA = GetComponent<ControlSLA>();
            _levelManager = GetComponent<LevelManagerSLA>();
            _score = GetComponent<ScoreSLA>();
        }

        public PlayerManager InitializePlayer(Game.Scripts.Network.Data.Player player)
        {
            var playerManager = CharacterBuilder.Create("Cat");
            playerManager.Model.SetActive(false);
            playerManager.Player = player;
            _score.InitializeScore(playerManager);

            return playerManager;
        }

        public void InitializeControls(PlayerManager playerManager)
        {
            GetComponent<InputManager>().Initialize(playerManager);
            CameraMovement.InitializeFollowTarget(playerManager.gameObject);
        }

        public void PrepareLevel()
        {
            CameraMovement.SetCameraPosition(Vector3.zero);

            // Show level highscore and current level
            CurrentPr.text = GameControl.HighScores.HighScoreSLA[_controlSLA.CurrentLevel].ToString();
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + _controlSLA.CurrentLevel;
            LevelTextObject.SetActive(true);
            CurrentPrWindow.SetActive(true);
        }

        public void HidePanels()
        {
            LevelTextObject.SetActive(false);
            CurrentPrWindow.SetActive(false);
        }

        public void SpawnPlayer(PlayerManager playerManager, Vector3 position, float rotation = 0)
        {
            playerManager.IsDead = false;
            playerManager.IsImmobile = false;
            playerManager.IsInvulnerable = true;
            playerManager.Model.SetActive(true);
            playerManager.Shield.SetActive(true);
            playerManager.transform.position = position;
            playerManager.transform.eulerAngles = new Vector3(0, rotation, 0);
            playerManager.CharacterManager.Speed.SetBaseSpeed(_levelManager.GetMovementSpeed(_controlSLA.CurrentLevel));
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
            playerManager.Shield.SetActive(false);
            playerManager.IsInvulnerable = false;

            if (GameControl.GameState.Solo)
            {
                _score.StartScore();
            }
        }
    }
}
