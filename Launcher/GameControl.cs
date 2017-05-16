
namespace Assets.Scripts.Launcher
{
    public class GameControl : Singleton<GameControl>{

        protected GameControl()
        {
        }

        public SceneLoader SceneLoader;

        public GameState State;
        public InputManager InputManager;
        public Settings Settings;
        public HighScores HighScores;

        //Keep Game Manager active and destroy any additional copys
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            State = new GameState();
            InputManager = new InputManager();
            Settings = new Settings();
            HighScores = new HighScores();
        }

        //Start Game
         private void Start()
        {
            if (SceneLoader != null)
                SceneLoader.LoadScene("Mainmenu", 0.5f);
        }
    }
}