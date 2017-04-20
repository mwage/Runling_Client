using System.Collections;
using Assets.Scripts.Launcher;
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

        public GameObject Player;
        public GameObject LevelTextObject;
        public GameObject Text3;
        public GameObject Text2;
        public GameObject Text1;
        public GameObject CurrentPrWindow;
        public GameObject NewPlayer;
        public Text CurrentPr;
        private TextMeshProUGUI _levelText;


        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            StartCoroutine(PrepareLevel());
        }

        IEnumerator PrepareLevel()
        {
            // Set current Level and movespeed
            GameControl.CurrentLevel++;
            GameControl.MoveSpeed = LevelManagerSla.GetMovementSpeed(GameControl.CurrentLevel);
            
            // Show level highscore and current level
            CurrentPr.text = HighScoreSLA.highScoreSLA[GameControl.CurrentLevel].ToString();
            _levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            _levelText.text = "Level " + GameControl.CurrentLevel;
            LevelTextObject.SetActive(true);
            CurrentPrWindow.SetActive(true);
            yield return new WaitForSeconds(2f);
            LevelTextObject.SetActive(false);
            CurrentPrWindow.SetActive(false);
            yield return new WaitForSeconds(1f);

            // Load drones and player

            NewPlayer = Instantiate(Player);
            GameControl.Dead = false;
            GameControl.IsInvulnerable = true;
            ControlSla.StopUpdate = false;
            LevelManagerSla.LoadDrones(GameControl.CurrentLevel);

            // Countdown
            Text3.SetActive(true);
            yield return new WaitForSeconds(1f);
            Text3.SetActive(false);
            Text2.SetActive(true);
            yield return new WaitForSeconds(1f);
            Text2.SetActive(false);
            Text1.SetActive(true);
            yield return new WaitForSeconds(1f);
            Text1.SetActive(false);

            GameControl.IsInvulnerable = false;
            ScoreSla.StartScore();

        }
    }
}
