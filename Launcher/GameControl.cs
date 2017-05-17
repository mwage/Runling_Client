using UI;

namespace Launcher
{
    public class GameControl : Singleton<GameControl>{

        protected GameControl()
        {
        }

        public SceneLoader SceneLoader;

        private GameState _state;
        private InputManager _inputManager;
        private Settings _settings;
        private HighScores _highScores;

        public static GameState State { get { return Instance._state; } }
        public static InputManager InputManager { get { return Instance._inputManager; } }
        public static Settings Settings { get { return Instance._settings; } }
        public static HighScores HighScores { get { return Instance._highScores; } }


        //Keep Game Manager active and destroy any additional copys
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _state = new GameState();
            _inputManager = new InputManager();
            _settings = new Settings();
            _highScores = new HighScores();
        }

        //Start Game
         private void Start()
        {
            if (SceneLoader != null)
                SceneLoader.LoadScene("Mainmenu", 0.5f);
        }
    }
}