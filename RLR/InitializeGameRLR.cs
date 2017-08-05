using System.Collections;
using Characters;
using Characters.Types;
using Launcher;
using Players;
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
        private LevelManagerRLR LevelManager;
        private ControlRLR ControlRLR;
        public InGameMenuManagerRLR InGameMenuManager;
        public CameraHandleMovement CameraHandleMovement;
        public ScoreRLR ScoreRLR;
        public PlayerFactory PlayerFactory;

        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;

        public void InitializePlayer()
        {
            var playerManager = PlayerFactory.Create(new CharacterDto(0, "Arena", 0, 0, 0, 0, 1, 0, 0)).GetComponent<PlayerManager>();
            playerManager.Model.SetActive(false);
            playerManager.Trigger.SetActive(false);
            ControlRLR.PlayerManager = playerManager;
            GetComponent<InputServer>().Init(InGameMenuManager.gameObject, playerManager);
        }

        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            ControlRLR.CheckIfFinished = true;
            StartCoroutine(PrepareLevel());
        }

        private IEnumerator PrepareLevel()
        {
            // load map
            LevelManager.GenerateMap(GameControl.GameState.CurrentLevel);

            // Load player
           if (GameControl.PlayerState.Player == null)
            {
                GameControl.PlayerState.Player = PlayerFactory.Create(GameControl.PlayerState.CharacterDto);
                GameControl.PlayerState.PlayerTrigger = GameControl.PlayerState.Player.transform.Find("Trigger").gameObject.GetComponent<PlayerTrigger>();
            }

            var startPlatform = GameControl.MapState.SafeZones[0];
            GameControl.PlayerState.Player.transform.rotation = Quaternion.Euler(new Vector3 (0, 90, 0));
            GameControl.PlayerState.Player.transform.position = new Vector3(
                    startPlatform.transform.position.x + startPlatform.transform.Find("VisibleObjects/Ground").transform.localScale.x / 2 - 1,
                    0,
                    startPlatform.transform.position.z);

            if (GameControl.PlayerState.GodModeActive && !GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.activeSelf)
            {
                GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(true);
            }
            GameControl.PlayerState.IsDead = false;
            GameControl.PlayerState.IsInvulnerable = true;
            GameControl.PlayerState.IsImmobile = true;

            // set camera
            GameControl.Settings.CameraRange = LevelManager.MapGeneratorRlr.GetAirColliderRange() / 2.5f;
            CameraHandleMovement.SetCameraHandlePosition(new Vector3(GameControl.PlayerState.Player.transform.localPosition.x, 0, GameControl.PlayerState.Player.transform.localPosition.z));

            ControlRLR.StopUpdate = false;

            // generate drones
            LevelManager.GenerateChasers(GameControl.GameState.CurrentLevel);
            LevelManager.LoadDrones(GameControl.GameState.CurrentLevel);

            // Show current level
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.GameState.CurrentLevel;
            LevelTextObject.SetActive(true);
            yield return new WaitForSeconds(2);
            LevelTextObject.SetActive(false);

            yield return new WaitForSeconds(1);

            // Countdown
            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, GameObject.Find("Canvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(1);
                Destroy(countdown);
            }

            GameControl.PlayerState.IsInvulnerable = false;
            GameControl.PlayerState.IsImmobile = false;
            ScoreRLR.StartTimer();
        }
    }
}
