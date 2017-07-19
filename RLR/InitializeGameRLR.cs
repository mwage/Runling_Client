using System.Collections;
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
        public LevelManagerRLR LevelManagerRLR;
        public ControlRLR ControlRLR;
        public InGameMenuManagerRLR InGameMenuManagerRLR;
        public CameraHandleMovement CameraHandleMovement;
        public ScoreRLR ScoreRLR;
        public PlayerFactory PlayerFactory;

        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;



        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            StartCoroutine(PrepareLevel());
        }

        private IEnumerator PrepareLevel()
        {
            // load map
            LevelManagerRLR.GenerateMap(GameControl.State.CurrentLevel);

            // Load player
           if (GameControl.PlayerState.Player == null)
            {
                GameControl.PlayerState.Player = PlayerFactory.Create(GameControl.PlayerState.CharacterDto, 1);
                GameControl.PlayerState.PlayerTrigger = GameControl.PlayerState.Player.transform.Find("Trigger").gameObject.GetComponent<PlayerTrigger>();
            }

            var startPlatform = GameControl.MapState.SafeZones[0];
            GameControl.PlayerState.Player.transform.rotation = Quaternion.Euler(new Vector3 (0, 90, 0));
            GameControl.PlayerState.Player.transform.position = new Vector3(
                    startPlatform.transform.position.x + startPlatform.transform.Find("VisibleObjects/Ground").transform.localScale.x / 2 - 1,
                    0,
                    startPlatform.transform.position.z);

            if (GameControl.State.GodModeActive && !GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.activeSelf)
            {
                GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(true);
            }
            GameControl.PlayerState.IsDead = false;
            GameControl.PlayerState.IsInvulnerable = true;
            GameControl.PlayerState.IsImmobile = true;

            // set camera
            GameControl.Settings.CameraRange = LevelManagerRLR.MapGeneratorRlr.GetAirColliderRange() / 2.5f;
            CameraHandleMovement.SetCameraHandlePosition(new Vector3(GameControl.PlayerState.Player.transform.localPosition.x, 0, GameControl.PlayerState.Player.transform.localPosition.z));

            ControlRLR.StopUpdate = false;

            // generate drones
            LevelManagerRLR.GenerateChasers(GameControl.State.CurrentLevel);
            LevelManagerRLR.LoadDrones(GameControl.State.CurrentLevel);

            // Show current level
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.State.CurrentLevel;
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
