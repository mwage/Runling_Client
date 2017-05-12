using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Launcher
{
    public class GameControl : MonoBehaviour {

        public static GameControl Control;

        // Level/Game management
        public static bool GameActive = false;
        public static int CurrentLevel = 1;
        public static int TotalScore = 0;
        public static bool FinishedLevel = false;
        public static Difficulty SetDifficulty = Difficulty.Hard;
        public static Gamemode SetGameMode = Gamemode.Practice;

        // Camera
        public static float CameraRange = 0;
        public static float CameraZoom = 30;
        public static float CameraAngle = 90;
        public static float CameraSpeed = 20;
        
        // Toggles
        public static bool AutoClickerActive = false;
        public static bool GodModeActive = false;

        // Player
        public static float MoveSpeed = 0;
        public static bool IsDead = true;
        public static bool IsInvulnerable = false;
        public static bool IsSafe = false;
        public static bool IsImmobile = false;
        

        public enum Difficulty
        {
            Normal, 
            Hard
        }

        public enum Gamemode
        {
            Classic,
            TimeMode,
            Practice
        }

        //Keep Game Manager active and destroy any additional copys
        private void Awake()
        {
            if (Control == null)
            {
                DontDestroyOnLoad(gameObject);
                Control = this;
            }
            else if (Control != this)
            {
                Destroy(gameObject);
            }
        }

        //Start Game
        private void Start()
        {
            var zoom = PlayerPrefs.GetFloat("CameraZoom");
            CameraZoom = zoom != 0 ? zoom : 30;
            var angle = PlayerPrefs.GetFloat("CameraAngle");
            CameraAngle = angle != 0 ? angle : 90;
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
