using Client.Scripts.Launcher;
using Client.Scripts.UI.Menus.RLR_Menus;
using Client.Scripts.UI.Menus.RLR_Menus.Characters;
using Game.Scripts.GameSettings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Client.Scripts.UI.Menus.Main_Menu
{
    public class RLRMenu : MonoBehaviour
    {
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
            PickCharacterMenu.gameObject.SetActive(true);
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
                PickCharacterMenu.GetComponent<PickCharacterMenu>().GetCharacterDto().Name != null)
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
            GameControl.GameState.CharacterDto = PickCharacterMenu.GetComponent<PickCharacterMenu>().GetCharacterDto();
            if (GameControl.GameState.CharacterDto.Name == null)
                return;

            SetModes();
            GameControl.GameState.Solo = true;

            _sceneLoader.LoadScene("RLR", 1);
            _mainMenuManager.gameObject.SetActive(false);
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
            transform.Find("Selection/Mode/Classic").GetComponent<Toggle>().isOn = false;
            transform.Find("Selection/Mode/Practice").GetComponent<Toggle>().isOn = false;
            transform.Find("Selection/Mode/Time").GetComponent<Toggle>().isOn = false;
            transform.Find("Selection/Difficulty/Normal").GetComponent<Toggle>().isOn = false;
            transform.Find("Selection/Difficulty/Hard").GetComponent<Toggle>().isOn = false;
            _mainMenuManager.MoveCamera(_mainMenuManager.CameraPosMainMenu, _mainMenuManager.CameraRotMainMenu);
        }
        #endregion
    }
}