using System.Collections;
using Characters.Types;
using Launcher;
using Characters;
using Players.Camera;
using Players;
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


        private void Awake()
        {
            _controlSLA = GetComponent<ControlSLA>();
            _levelManager = GetComponent<LevelManagerSLA>();
            _score = GetComponent<ScoreSLA>();
        }

        public void InitializePlayer()
        {
            var playerManager = PlayerFactory.Create(new CharacterDto(0, "Arena", 0, 0, 0, 0, 1, 0, 0)).GetComponent<PlayerManager>();
            playerManager.Model.SetActive(false);
            _controlSLA.PlayerManager = playerManager;
            GetComponent<InputServer>().Init(InGameMenuManager.gameObject, playerManager);
            CameraHandleMovement.InitializeFollowTarget(playerManager.gameObject);
        }

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
            yield return new WaitForSeconds (3);
            LevelTextObject.SetActive(false);
            CurrentPrWindow.SetActive(false);
            yield return new WaitForSeconds (1);

            
            SpawnPlayer(_controlSLA.PlayerManager);
            _levelManager.LoadDrones(GameControl.GameState.CurrentLevel);

            StartCoroutine(StartCountdown());
        }

        private void SpawnPlayer(PlayerManager playerManager)
        {
            playerManager.IsDead = false;
            playerManager.IsSafe = false;
            playerManager.IsInvulnerable = true;
            playerManager.CheckIfDead = true;
            playerManager.Model.SetActive(true);
            playerManager.Shield.SetActive(true);

            playerManager.CharacterController.Speed.SetBaseSpeed(_levelManager.GetMovementSpeed(GameControl.GameState.CurrentLevel));
            playerManager.transform.position = Vector3.zero;
            playerManager.transform.rotation = Quaternion.identity;
        }

        private IEnumerator StartCountdown()
        {
            yield return new WaitForSeconds (1);

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
            _controlSLA.PlayerManager.Shield.SetActive(false);
            _controlSLA.PlayerManager.IsInvulnerable = false;

            _score.StartScore();
        }
    }
}
