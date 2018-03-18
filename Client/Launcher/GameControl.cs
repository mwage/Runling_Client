using Client.Scripts.Network;
using Game.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.Launcher
{
    public class GameControl : Singleton<GameControl>
    {
        protected GameControl()
        {
        }
        
        private GameState _gameState;
        private HotkeyMapping _hotkeyMapping;
        private Settings _settings;
        private HighScores _highScores;
        private Rsa _rsa;

        public static GameState GameState => Instance._gameState;
        public static HotkeyMapping HotkeyMapping => Instance._hotkeyMapping;
        public static Settings Settings => Instance._settings;
        public static HighScores HighScores => Instance._highScores;
        public static Rsa Rsa => Instance._rsa;

        public const int Version = 2;

        private void Awake()
        {
            _gameState = new GameState();
            _hotkeyMapping = new HotkeyMapping();
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