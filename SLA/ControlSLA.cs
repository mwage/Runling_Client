using System.Collections;
using Characters;
using Characters.Repositories;
using Characters.Types;
using Launcher;
using SLA.Levels;
using TMPro;
using UnityEngine;

namespace SLA
{
    public class ControlSLA : MonoBehaviour
    {
        public LevelManagerSLA LevelManager;
        public ScoreSLA Score;
        public InitializeGameSLA InitializeGame;
        public DeathSLA Death;
        public GameObject PracticeMode;
        public NetworkManagerSLA NetworkManager;

        private PhotonView _photonView;

        public bool StopUpdate;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
            GameControl.Settings.CameraRange = 15;
            GameControl.GameState.GameActive = true;
            foreach (var state in NetworkManager.SyncVars)
            {
                state.TotalScore = 0;
            }
            if (GameControl.GameState.SetGameMode == Gamemode.Practice)
            {
                PracticeMode.SetActive(true);
            }
            InitializeGame.InitializeGame();
        }


        private void Update()
        {
            if (NetworkManager.SyncVars[PhotonNetwork.player.ID - 1].IsDead && !StopUpdate)
            {
                Death.Death();

                //in case of highscore, save and 
                if (GameControl.GameState.SetGameMode != Gamemode.Practice)
                {
                    Score.SetHighScore();
                }
                
                //change level
                LevelManager.EndLevel(2f);

                //dont repeat above once player dead
                StopUpdate = true;
            }

            
            // Press 1 to turn on Godmode
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateGodmode) && !GameControl.PlayerState.GodModeActive)
            {
                GameControl.PlayerState.GodModeActive = true;
                if (GameControl.PlayerState.Player != null)
                {
                    GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(true);
                }
            }

            // Press 2 to turn off Godmode
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactiveGodmode) && GameControl.PlayerState.GodModeActive)
            {
                GameControl.PlayerState.GodModeActive = false;
                if (GameControl.PlayerState.Player != null)
                {
                   GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(false);
                }
            }
        }

    }
}

