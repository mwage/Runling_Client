using Launcher;
using UI.RLR_Menus;
using UI.RLR_Menus.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI.Main_Menu
{
    public class RLRMenu : MonoBehaviour
    {
        public GameObject PrevMenu;
        public GameObject LaunchRLR;
        public GameObject PickCharacterMenu;

        private MainMenuManager _mainMenuManager;
        private HighScoreMenuRLR _highScoreMenu;
        private SceneLoader _sceneLoader;

        private Difficulty? _voteDifficulty;
        private GameMode? _voteGameMode;

        private void Awake()
        {
            _mainMenuManager = transform.parent.GetComponent<MainMenuManager>();
            _highScoreMenu = _mainMenuManager.HighScoreMenuRLR;
            _sceneLoader = _mainMenuManager.SceneLoader;
        }

        private void OnEnable()
        {
            _voteDifficulty = null;
            _voteGameMode = null;
        }

        public void SetModes()
        {
            if (_voteDifficulty != null) GameControl.GameState.SetDifficulty = (Difficulty)_voteDifficulty;
            if (_voteGameMode != null) GameControl.GameState.SetGameMode = (GameMode)_voteGameMode;
        }
        
        public void Update()
        {
            if (_voteDifficulty != null && _voteGameMode != null &&
                PickCharacterMenu.GetComponent<PickCharacterMenu>().PickedSlot != 0)
            {
                LaunchRLR.GetComponentInChildren<Text>().text = "R U N";
                LaunchRLR.GetComponent<Button>().interactable = true;
            }
            else
            {
                LaunchRLR.GetComponentInChildren<Text>().text = "Select Modes";
                LaunchRLR.GetComponent<Button>().interactable = false;
            }
        }

        #region Buttons
        public void StartGame()
        {
            GameControl.GameState.CurrentLevel = 1;
            GameControl.GameState.CharacterDto = PickCharacterMenu.GetComponent<PickCharacterMenu>().GetCharacterDto();
            SetModes();

            _sceneLoader.LoadScene("RLR", 1);
            _mainMenuManager.gameObject.SetActive(false);
        }

        public void StartGameSOLOHARDRLRTEST()
        {
            GameControl.GameState.CurrentLevel = 1;
            GameControl.GameState.CharacterDto = PickCharacterMenu.GetComponent<PickCharacterMenu>().GetCharacterDto();
            GameControl.GameState.SetDifficulty = Difficulty.Hard;
            GameControl.GameState.SetGameMode = GameMode.Practice;

            SceneManager.LoadScene("RLR");
        }

        public void VoteDifficultyNormal()
        {
            _voteDifficulty = Difficulty.Normal;
        }

        public void VoteDifficultyHard()
        {
            _voteDifficulty = Difficulty.Hard;
        }

        public void VoteModeClassic()
        {
            _voteGameMode = GameMode.Classic;
        }

        public void VoteModeTime()
        {
            _voteGameMode = GameMode.TimeMode;
        }

        public void VoteModePractice()
        {
            _voteGameMode = GameMode.Practice;
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            _highScoreMenu.gameObject.SetActive(true);
        }

        public void BackToMenu()
        {
            gameObject.SetActive(false);
            PickCharacterMenu.SetActive(false);
            PrevMenu.gameObject.SetActive(true);
            transform.Find("Mode/Classic").GetComponent<Toggle>().isOn = false;
            transform.Find("Mode/Practice").GetComponent<Toggle>().isOn = false;
            transform.Find("Mode/Time").GetComponent<Toggle>().isOn = false;
            transform.Find("Difficulty/Normal").GetComponent<Toggle>().isOn = false;
            transform.Find("Difficulty/Hard").GetComponent<Toggle>().isOn = false;
            _mainMenuManager.MoveCamera(_mainMenuManager.CameraPosMainMenu, _mainMenuManager.CameraRotMainMenu);
        }
        #endregion
    }
}