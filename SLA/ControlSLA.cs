using System.Linq;
using Launcher;
using SLA.Levels;
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

        private PhotonView _photonView;
        public bool LoadingNextLevel;
        public bool CheckIfDead;
        public bool CheckIfAllDead;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            CheckIfDead = false;
            CheckIfAllDead = false;
            GameControl.Settings.CameraRange = 15;
            GameControl.GameState.GameActive = true;
            foreach (var state in GameControl.PlayerState.SyncVars)
            {
                state.TotalScore = 0;
            }
            if (GameControl.GameState.SetGameMode == GameMode.Practice)
            {
                PracticeMode.SetActive(true);
            }
            InitializeGame.InitializeGame();
        }


        private void Update()
        {
            if (CheckIfDead && GameControl.PlayerState.SyncVars[PhotonNetwork.player.ID - 1].IsDead)
            {
                CheckIfDead = false;
                //Death.Death();

                //in case of highscore, save and 
                if (GameControl.GameState.SetGameMode != GameMode.Practice)
                {
                    Score.SetHighScore();
                }
            }

            if (PhotonNetwork.isMasterClient && CheckIfAllDead)
            {
                CheckDead();
            }

            // TODO: substitute with InputServer
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

        private void CheckDead()
        {
            if (GameControl.PlayerState.SyncVars.Where(state => state != null).Any(state => !state.IsDead))
                return;

            CheckIfAllDead = false;
            _photonView.RPC("LevelOver", PhotonTargets.All);
        }

        [PunRPC]
        private void LevelOver()
        {
            CheckIfAllDead = false;
            GameControl.GameState.AllDead = true;
            Debug.Log("loading next level");
            if (!LoadingNextLevel)
            {
                LoadingNextLevel = true;
                LevelManager.EndLevel(2f);
            }
        }
    }
}

