using Characters;
using Launcher;
using Network.Synchronization;
using Network.Synchronization.Data;
using Players;
using Players.Camera;
using TMPro;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.UI;

namespace SLA
{
    public class InitializeGameSLA : MonoBehaviour
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
        private ScoreSLA _score;
        private GameObject _currentCountdown;


        private void Awake()
        {
            _controlSLA = GetComponent<ControlSLA>();
            _levelManager = GetComponent<LevelManagerSLA>();
            _score = GetComponent<ScoreSLA>();
        }

        public PlayerManager InitializePlayer(Player player)
        {
            var playerManager = PlayerFactory.Create("Manticore");
            playerManager.Model.SetActive(false);
            playerManager.Player = player;
            GetComponent<InputServer>().Init(InGameMenuManager.gameObject, playerManager);
            CameraHandleMovement.InitializeFollowTarget(playerManager.gameObject);
            playerManager.PlayerMovement = playerManager.gameObject.AddComponent<PlayerMovement>();

            if (GameControl.GameState.Solo)
            {
                _score.SetName(player.Name);
            }
            else
            {
                playerManager.PlayerStateManager = playerManager.gameObject.AddComponent<PlayerStateManager>();
            }

            return playerManager;
        }

        public PlayerManager InitializeOtherPlayer(Player player)
        {
            var playerManager = PlayerFactory.Create("Manticore");
            playerManager.Model.SetActive(false);
            playerManager.Player = player;
            return playerManager;
        }

        public void PrepareLevel()
        {
            CameraHandleMovement.SetCameraHandlePosition(Vector3.zero);

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
                playerManager.CharacterController.Speed.SetBaseSpeed(_levelManager.GetMovementSpeed(_controlSLA.CurrentLevel));
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
