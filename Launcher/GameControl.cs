using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Launcher
{
    public class GameControl : MonoBehaviour {

        public static GameControl Instance;
        public SceneLoader SceneLoader;

        // Level/Game management
        public static bool GameActive = false;
        public static int CurrentLevel = 1;
        public static int TotalScore = 0;
        public static bool FinishedLevel = false;
        public static Difficulty SetDifficulty = Difficulty.Hard;
        public static Gamemode SetGameMode = Gamemode.Practice;


        // Toggles
        public static bool AutoClickerActive = false;
        public static bool GodModeActive = false;

        // Player
        public static GameObject Player;
        public static float MoveSpeed = 0;
        public static bool IsDead = true;
        public static bool IsInvulnerable = false;
        public static bool IsSafe = false;
        public static bool IsImmobile = false;
        

        //Keep Game Manager active and destroy any additional copys
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        //Start Game
        private void Start()
        {
            Settings.Instance.LoadSettings();
            SceneLoader.LoadScene("Mainmenu", 0.5f);
        }

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
    }
}