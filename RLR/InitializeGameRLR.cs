using System.Collections;
using Assets.Scripts.Launcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI.RLR_Menus;


namespace Assets.Scripts.RLR
{
    public class InitializeGameRLR : MonoBehaviour
    {

        // Attach scripts
        public LevelManagerRLR LevelManagerRLR;
        public ControlRLR ControlRLR;
        public InGameMenuManagerRLR InGameMenuManagerRLR;
        public GenerateMap GenerateMap;

        public GameObject PlayerPrefab;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public GameObject Player;
        public GameObject MainCamera;


        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            GenerateMap.generateMapRLR();
            StartCoroutine(PrepareLevel());
        }

        IEnumerator PrepareLevel()
        {
            // Set current Level and movespeed
            GameControl.MoveSpeed = LevelManagerRLR.GetMovementSpeed(GameControl.CurrentLevel);

            // Load drones and player
            Player = Instantiate(PlayerPrefab, new Vector3(-49, 0, 42), Quaternion.Euler(0, 90, 0));
            MainCamera.transform.position = new Vector3(-49, 40, 42);
            GameControl.Dead = false;
            GameControl.IsInvulnerable = true;
            GameControl.IsImmobile = true;
            ControlRLR.StopUpdate = false;
            LevelManagerRLR.LoadDrones(GameControl.CurrentLevel);

            // Show current level
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.CurrentLevel;
            LevelTextObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            LevelTextObject.SetActive(false);
  
            yield return new WaitForSeconds(1f);
                   
            // Countdown
            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, GameObject.Find("ScoreCanvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(1f);
                Destroy(countdown);
            }

            GameControl.IsInvulnerable = false;
            GameControl.IsImmobile = false;
        }

        
        

    }
}
