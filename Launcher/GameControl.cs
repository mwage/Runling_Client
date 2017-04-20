using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Launcher
{
    public class GameControl : MonoBehaviour {

        public static GameControl control;

        public static bool Dead = true;                 //to set Player to dead/alive 
        public static bool GameActive = false;          //if a game is ongoing
        public static int CurrentLevel = 0;             //current active level
        public static float MoveSpeed = 0;              //movespeed of your character

        public static bool AutoClickerActive = false;
        public static bool IsInvulnerable = false;
        public static bool GodModeActive = false;

        //Keep Game Manager active and destroy any additional copys
        private void Awake()
        {
            if (control == null)
            {
                DontDestroyOnLoad(gameObject);
                control = this;
            }
            else if (control != this)
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
