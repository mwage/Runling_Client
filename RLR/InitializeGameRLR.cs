using System.Collections;
using Launcher;
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
            LevelManagerRLR.GenerateMap(GameControl.State.CurrentLevel);

            // Load player
           if (GameControl.State.Player == null)
            {
                GameControl.State.Player = PlayerFactory.Create(GameControl.State.CharacterDto, 1);
            }
            var startPlatform = LevelManagerRLR.GenerateMapRLR.GetStartPlatform();
            GameControl.State.Player.transform.position =
                new Vector3(
                    startPlatform.transform.position.x + startPlatform.transform.Find("VisibleObjects/Ground")
                        .transform.localScale.x / 2 - 1, 0, startPlatform.transform.position.z);
            if (GameControl.State.GodModeActive && !GameControl.State.Player.transform.Find("GodMode").gameObject.activeSelf)
            {
                GameControl.State.Player.transform.Find("GodMode").gameObject.SetActive(true);
            }
            GameControl.State.IsDead = false;
            GameControl.State.IsInvulnerable = true;
            GameControl.State.IsImmobile = true;

            // set camera
            GameControl.Settings.CameraRange = LevelManagerRLR.GenerateMapRLR.GetAirColliderRange() / 2.5f;
            CameraHandleMovement.SetCameraHandlePosition(new Vector3(GameControl.State.Player.transform.localPosition.x, 0, GameControl.State.Player.transform.localPosition.z));

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

            GameControl.State.IsInvulnerable = false;
            GameControl.State.IsImmobile = false;
            ScoreRLR.StartTimer();
        }
    }
}
