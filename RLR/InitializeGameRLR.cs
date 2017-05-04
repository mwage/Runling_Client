using System.Collections;
using Assets.Scripts.Launcher;
using Assets.Scripts.RLR.Levels;
using TMPro;
using UnityEngine;
using Assets.Scripts.UI.RLR_Menus;


namespace Assets.Scripts.RLR
{
    public class InitializeGameRLR : MonoBehaviour
    {

        // Attach scripts
        public LevelManagerRLR LevelManagerRLR;
        public ControlRLR ControlRLR;
        public InGameMenuManagerRLR InGameMenuManagerRLR;
        public RunlingChaser RunlingChaser;


        public GameObject PlayerPrefab;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public GameObject Player;
        public GameObject MainCamera;


        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            StartCoroutine(PrepareLevel());
        }

        IEnumerator PrepareLevel()
        {
            LevelManagerRLR.GenerateMap(GameControl.CurrentLevel);

            // Load drones and player
            var startPlatform = GameObject.Find("StartPlatform(Clone)");
            var airCollider = GameObject.Find("FlyingDroneCollider(Clone)");
            Player = Instantiate(PlayerPrefab, new Vector3(startPlatform.transform.position.x, 0, startPlatform.transform.position.z), Quaternion.Euler(0, 90, 0));
            MainCamera.transform.position = new Vector3(Player.transform.localPosition.x, 40, Player.transform.localPosition.z);
            RunlingChaser.GetTriggerInstance(Player.transform.GetChild(2).gameObject);
            RunlingChaser.GetSaveZones();
            GameControl.CameraRange = airCollider.transform.localScale.x/2.5f;
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
