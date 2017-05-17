using System.Collections;
using Launcher;
using Players.Camera;
using SLA.Levels;
using TMPro;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.UI;

namespace SLA
{
    public class InitializeGameSLA : MonoBehaviour {

        // Attach scripts
        public LevelManagerSLA LevelManagerSla;
        public ScoreSLA ScoreSla;
        public ControlSLA ControlSla;
        public InGameMenuManagerSLA InGameMenuManagerSla;

        public GameObject PlayerPrefab;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public GameObject CurrentPrWindow;
        public Text CurrentPr;
        public CameraHandleMovement CameraHandleMovement;

        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            StartCoroutine(PrepareLevel());
        }

        private IEnumerator PrepareLevel()
        {
            // Set current movespeed and cameraposition
            GameControl.State.MoveSpeed = LevelManagerSla.GetMovementSpeed(GameControl.State.CurrentLevel);
            CameraHandleMovement.SetCameraHandlePosition(Vector3.zero);

            // Show level highscore and current level
            CurrentPr.text = GameControl.HighScores.HighScoreSLA[GameControl.State.CurrentLevel].ToString();
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.State.CurrentLevel;
            LevelTextObject.SetActive(true);
            CurrentPrWindow.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            LevelTextObject.SetActive(false);
            CurrentPrWindow.SetActive(false);
            yield return new WaitForSeconds(0.1f);

            // Load drones and player

            GameControl.State.Player = Instantiate(PlayerPrefab);
            GameControl.State.IsDead = false;
            GameControl.State.IsInvulnerable = true;
            GameControl.State.Player.transform.Find("Shield").gameObject.SetActive(true);
            if (GameControl.State.GodModeActive && !GameControl.State.Player.transform.Find("GodMode").gameObject.activeSelf)
            {
                GameControl.State.Player.transform.Find("GodMode").gameObject.SetActive(true);
            }
            GameControl.State.IsImmobile = false;
            ControlSla.StopUpdate = false;
            LevelManagerSla.LoadDrones(GameControl.State.CurrentLevel);
            
            // Countdown
            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, GameObject.Find("ScoreCanvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(1f);
                Destroy(countdown);
            }

            GameControl.State.Player.transform.Find("Shield").gameObject.SetActive(false);
            GameControl.State.IsInvulnerable = false;
            ScoreSla.StartScore();
        }
    }
}
