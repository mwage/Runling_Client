using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Launcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.RLR
{
    public class InitializeGameRLR : MonoBehaviour
    {

        // Attach scripts
        public LevelManagerRLR LevelManagerSla;
        public ScoreRLR ScoreSla;
        public ControlRLR ControlSla;
        public InGameMenuManagerRLR InGameMenuManagerSla;
        public GenerateMap GenerateMap;

        public GameObject PlayerPrefab;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public GameObject CurrentPrWindow;
        public GameObject Player;
        public Text CurrentPr;
        



        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            GenerateMap.generateMapRLR();
            StartCoroutine(PrepareLevel());
        }

        IEnumerator PrepareLevel()
        {
            // Set current Level and movespeed
            GameControl.MoveSpeed = LevelManagerSla.GetMovementSpeed(GameControl.CurrentLevel);

            // Show level highscore and current level
            CurrentPr.text = HighScoreRLR.highScoreRLR[GameControl.CurrentLevel].ToString();
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.CurrentLevel;
            LevelTextObject.SetActive(true);
            CurrentPrWindow.SetActive(true);
            yield return new WaitForSeconds(2f);
            LevelTextObject.SetActive(false);
            CurrentPrWindow.SetActive(false);
            yield return new WaitForSeconds(1f);

            // Load drones and player

            Player = Instantiate(PlayerPrefab);
            GameControl.Dead = false;
            GameControl.IsInvulnerable = true;
            ControlSla.StopUpdate = false;
            LevelManagerSla.LoadDrones(GameControl.CurrentLevel);

            // Countdown
            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, GameObject.Find("ScoreCanvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(1f);
                Destroy(countdown);
            }

            GameControl.IsInvulnerable = false;
            ScoreSla.StartScore();

        }

        
        

    }
}
