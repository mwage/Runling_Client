using System.Collections;
using Assets.Scripts.Launcher;
using Assets.Scripts.Players;
using Assets.Scripts.Players.Camera;
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


        public GameObject PlayerPrefab;
        public GameObject LevelTextObject;
        public GameObject CountdownPrefab;
        public CameraHandleMovement CameraHandleMovement;


        //set Spawnimmunity once game starts
        public void InitializeGame()
        {
            StartCoroutine(PrepareLevel());
        }

        IEnumerator PrepareLevel()
        {
            LevelManagerRLR.GenerateMap(GameControl.State.CurrentLevel);

            // Load drones and player
            var startPlatform = LevelManagerRLR.GenerateMapRLR.GetStartPlatform();
            var airColliderRange = LevelManagerRLR.GenerateMapRLR.GetAirColliderRange();
            GameControl.State.Player = Instantiate(PlayerPrefab, new Vector3(startPlatform.x, 0, startPlatform.z), Quaternion.Euler(0, 90, 0));
            GameControl.State.Player.GetComponent<PlayerMovement>().Acceleration = 80;
            if (GameControl.State.GodModeActive && !GameControl.State.Player.transform.Find("GodMode").gameObject.activeSelf)
            {
                GameControl.State.Player.transform.Find("GodMode").gameObject.SetActive(true);
            }
            GameControl.Settings.CameraRange = airColliderRange / 2.5f;
            CameraHandleMovement.SetCameraHandlePosition(new Vector3(GameControl.State.Player.transform.localPosition.x, 0, GameControl.State.Player.transform.localPosition.z));
            LevelManagerRLR.GenerateChasers(GameControl.State.CurrentLevel);
            GameControl.State.IsDead = false;
            GameControl.State.IsInvulnerable = true;
            GameControl.State.IsImmobile = true;
            ControlRLR.StopUpdate = false;
            LevelManagerRLR.LoadDrones(GameControl.State.CurrentLevel);

            // Show current level
            var levelText = LevelTextObject.GetComponent<TextMeshProUGUI>();
            levelText.text = "Level " + GameControl.State.CurrentLevel;
            LevelTextObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            LevelTextObject.SetActive(false);
  
            yield return new WaitForSeconds(0.1f);
                   
            // Countdown
            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(CountdownPrefab, GameObject.Find("Canvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(0.1f);
                Destroy(countdown);
            }

            GameControl.State.IsInvulnerable = false;
            GameControl.State.IsImmobile = false;
        }
    }
}
