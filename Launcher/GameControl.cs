using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Launcher
{
    public class GameControl : MonoBehaviour {

        public static GameControl Control;
        public SceneLoader SceneLoader;

        // Level/Game management
        public static bool GameActive = false;
        public static int CurrentLevel = 1;
        public static int TotalScore = 0;
        public static bool FinishedLevel = false;
        public static Difficulty SetDifficulty = Difficulty.Hard;
        public static Gamemode SetGameMode = Gamemode.Practice;

        // Camera
        public static float CameraRange = 0;
        public static Limits CameraZoom = new Limits(10, 100, def: 50);
        public static Limits CameraAngle = new Limits(10, 90, def: 90);
        public static Limits CameraSpeed = new Limits(5, 50, def: 10);
        public static int CameraFollow = 0;

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
            SceneLoader.LoadScene("Mainmenu", 0.5f);
        }
    }
    public class Limits
    {
        public float Min;
        public float Max;
        public float Val;
        public float Def;

        public Limits(float min, float max, float val = 10, float def = 10)
        {
            Min = min;
            Max = max;
            //Val = val;
            Def = def;
        }

        public void Decrease(float v)
        {
            Val = Val - v > Min ? Val - v : Min;
        }
        public void Increase(float v)
        {
            Val = Val + v < Max ? Val + v : Max;
        }
    }
}