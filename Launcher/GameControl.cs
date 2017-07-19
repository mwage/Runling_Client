using System.Linq;
using System.Text;
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
        private PlayerState _playerState;
        private MapState _mapState;

        public static GameState GameState { get { return Instance._gameState; } }
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
            _gameState = new GameState();
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

        #region HelperMethods

        public static string GetFriendlyName(string input)
        {
            var sb = new StringBuilder();
            foreach (var c in input)
            {
                if (char.IsUpper(c))
                    sb.Append(" ");
                sb.Append(c);
            }

            if (input.Length > 0 && char.IsUpper(input[0]))
                sb.Remove(0, 1);

            return sb.ToString();
        }


        public static string GenerateRoomName(string roomName)
        {
            if (PhotonNetwork.offlineMode)
                return "SoloSLA";
            var rooms = PhotonNetwork.GetRoomList();
            var roomNames = rooms.Select(room => room.Name).ToList();
            int i = 0;
            var internalName = roomName + i;

            if (roomNames.Contains(internalName))
            {
                i++;
                internalName = roomName + i;
            }
            return internalName;
        }
        #endregion
    }
}