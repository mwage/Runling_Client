using System.Collections;
using Assets.Scripts.Launcher;
using Assets.Scripts.Players.Camera;
using Assets.Scripts.UI.SLA_Menus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SLA
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

        IEnumerator PrepareLevel()
        {
            // Set current movespeed and cameraposition
            GameControl.Instance.State.MoveSpeed = LevelManagerSla.GetMovementSpeed(GameControl.Instance.State.CurrentLevel);
            CameraHandleMovement.SetCameraHandlePosition(Vector3.zero);

            // Show level highscore and current level
            CurrentPr.text = GameControl.Instance.HighScores.HighScoreSLA[GameControl.Instance.State.CurrentLevel].ToString();
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.Instance.State.CurrentLevel;
            LevelTextObject.SetActive(true);
            CurrentPrWindow.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            LevelTextObject.SetActive(false);
            CurrentPrWindow.SetActive(false);
            yield return new WaitForSeconds(0.1f);

            // Load drones and player

            GameControl.Instance.State.Player = Instantiate(PlayerPrefab);
            GameControl.Instance.State.IsDead = false;
            GameControl.Instance.State.IsInvulnerable = true;
            GameControl.Instance.State.Player.transform.Find("Shield").gameObject.SetActive(true);
            if (GameControl.Instance.State.GodModeActive && !GameControl.Instance.State.Player.transform.Find("GodMode").gameObject.activeSelf)
            {
                transform.Find("GodMode").gameObject.SetActive(true);
            }
            GameControl.Instance.State.IsImmobile = false;
            ControlSla.StopUpdate = false;
            LevelManagerSla.LoadDrones(GameControl.Instance.State.CurrentLevel);
            
            // Countdown
            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, GameObject.Find("ScoreCanvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(1f);
                Destroy(countdown);
            }

            GameControl.Instance.State.Player.transform.Find("Shield").gameObject.SetActive(false);
            GameControl.Instance.State.IsInvulnerable = false;
            ScoreSla.StartScore();
            
        }
    }
}
