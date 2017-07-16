using UI;
using UnityEngine.SceneManagement;

namespace Launcher
{
    public class GameControl : Singleton<GameControl>
    {
        protected GameControl()
        {
        }

        private GameState _state;
        private InputManager _inputManager;
        private Settings _settings;
        private HighScores _highScores;
        private PlayerState _playerState;
        private MapState _mapState;

        public static GameState State { get { return Instance._state; } }
        public static InputManager InputManager { get { return Instance._inputManager; } }
        public static Settings Settings { get { return Instance._settings; } }
        public static HighScores HighScores { get { return Instance._highScores; } }
        public static PlayerState PlayerState { get { return Instance._playerState; } }
        public static MapState MapState { get { return Instance._mapState; } }

        public const int Version = 2;

        //Keep Game Manager active and destroy any additional copys
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _state = new GameState();
            _inputManager = new InputManager();
            _settings = new Settings();
            _highScores = new HighScores();
            _playerState = new PlayerState();
            _mapState = new MapState();
        }

        //Start Game
        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "Launcher")
                SceneManager.LoadScene("Connect");
        }
    }
}