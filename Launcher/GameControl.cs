using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Launcher
{
    public class GameControl : MonoBehaviour {

        public static GameControl Control;

        public static bool Dead = true;                 //to set Player to dead/alive 
        public static bool GameActive = false;          //if a game is ongoing
        public static int CurrentLevel = 1;             //current active level
        public static float MoveSpeed = 0;              //movespeed of your character
        public static float CameraRange = 0;            //how far camera can go away from the middle
        public static int TotalScore = 0;               //track total score of multiple levels


        public static bool AutoClickerActive = false;
        public static bool GodModeActive = false;
        public static bool IsInvulnerable = false;
        public static bool IsImmobile = false;

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
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
