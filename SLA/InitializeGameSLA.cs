using System.Collections;
using System.IO;
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
        public LevelManagerSLA LevelManagerSLA;
        public ScoreSLA ScoreSLA;
        public ControlSLA ControlSLA;
        public InGameMenuManagerSLA InGameMenuManagerSLA;

        public GameObject PlayerPrefab;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public GameObject CurrentPrWindow;
        public Text CurrentPr;
        public CameraHandleMovement CameraHandleMovement;

        private const string PlayerCharacter = "Cat";
        

        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            Debug.Log(StartingPosition());
            StartCoroutine(PrepareLevel());
        }

        private IEnumerator PrepareLevel()
        {
            // Set current movespeed and cameraposition
            GameControl.State.MoveSpeed = LevelManagerSLA.GetMovementSpeed(GameControl.State.CurrentLevel);
            CameraHandleMovement.SetCameraHandlePosition(Vector3.zero);

            // Show level highscore and current level
            CurrentPr.text = GameControl.HighScores.HighScoreSLA[GameControl.State.CurrentLevel].ToString();
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.State.CurrentLevel;
            LevelTextObject.SetActive(true);
            CurrentPrWindow.SetActive(true);
            yield return new WaitForSeconds(2);
            LevelTextObject.SetActive(false);
            CurrentPrWindow.SetActive(false);
            yield return new WaitForSeconds(1);

            // Load drones and player

            GameControl.State.Player = PhotonNetwork.Instantiate(Path.Combine("Characters", PlayerCharacter), StartingPosition(), Quaternion.LookRotation(Vector3.zero - StartingPosition()), 0);
            GameControl.State.IsDead = false;
            GameControl.State.IsInvulnerable = true;
            GameControl.State.IsSafe = false;
            GameControl.State.Player.transform.Find("Shield").gameObject.SetActive(true);
            if (GameControl.State.GodModeActive && !GameControl.State.Player.transform.Find("GodMode").gameObject.activeSelf)
            {
                GameControl.State.Player.transform.Find("GodMode").gameObject.SetActive(true);
            }
            GameControl.State.IsImmobile = false;
            ControlSLA.StopUpdate = false;
            LevelManagerSLA.LoadDrones(GameControl.State.CurrentLevel);
            
            // Countdown
            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, GameObject.Find("Canvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(1);
                Destroy(countdown);
            }

            GameControl.State.Player.transform.Find("Shield").gameObject.SetActive(false);
            GameControl.State.IsInvulnerable = false;
            ScoreSLA.StartScore();
        }

        private static Vector3 StartingPosition()
        {
            if (PhotonNetwork.room.PlayerCount == 1)
            {
                return Vector3.zero;
            }

            return Vector3.zero + Quaternion.Euler
                       (0, 360f * (PhotonNetwork.player.ID - 1) / PhotonNetwork.room.PlayerCount, 0) * Vector3.right * 2;
        }
    }
}
