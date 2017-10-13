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

        public static Client Client;

        private GameState _gameState;
        private InputManager _inputManager;
        private Settings _settings;
        private HighScores _highScores;

        public static GameState GameState => Instance._gameState;
        public static InputManager InputManager => Instance._inputManager;
        public static Settings Settings => Instance._settings;
        public static HighScores HighScores => Instance._highScores;

        public const int Version = 2;

        //Keep Game Manager active and destroy any additional copys
        private void Awake()
        {
            Client = GetComponent<Client>();
            _gameState = new GameState();
            _inputManager = new InputManager();
            _settings = new Settings();
            _highScores = new HighScores();
            DontDestroyOnLoad(gameObject);
        }

        //Start Game
        private void Start()
        {
            if (Client.Connect(Client.Address, Client.Port, Client.IpVersion) &&
                SceneManager.GetActiveScene().name == "Launcher")
            {
                SceneManager.LoadScene("Login");
            }
            else
            {
                Debug.Log("Insert reconnect / offline window");
            }
        }
    }
}