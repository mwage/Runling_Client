using Network;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Launcher
{
    public class GameControl : Singleton<GameControl>
    {
        protected GameControl()
        {
        }
        
        private GameState _gameState;
        private InputManager _inputManager;
        private Settings _settings;
        private HighScores _highScores;
        private Rsa _rsa;

        public static GameState GameState => Instance._gameState;
        public static InputManager InputManager => Instance._inputManager;
        public static Settings Settings => Instance._settings;
        public static HighScores HighScores => Instance._highScores;
        public static Rsa Rsa => Instance._rsa;

        public const int Version = 2;

        private void Awake()
        {
            _gameState = new GameState();
            _inputManager = new InputManager();
            _settings = new Settings();
            _highScores = new HighScores();
            _rsa = new Rsa();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name != "Launcher")
                return;

            if (MainClient.Instance.Connect())
            {
                SceneManager.LoadScene("Login");
            }
            else
            {
                Debug.Log("Todo: reconnect / offline window");
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}