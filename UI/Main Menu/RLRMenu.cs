using Assets.Scripts.Launcher;
using Assets.Scripts.UI.SLA_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Main_Menu
{
    public class RLRMenu : MonoBehaviour
    {

        public HighScoreMenuRLR HighScoreMenuSLA;

        public GameObject MainMenu;
        public GameObject HighScoreMenu;
        public GameObject LaunchRLR;

        private GameControl.Difficulty? voteDifficulty = null;
        private GameControl.Gamemode? voteGameMode = null;

        public bool RLRMenuActive;
        
        public void StartGame()
        {
            GameControl.IsDead = true;
            GameControl.TotalScore = 0;
            GameControl.CurrentLevel = 1;
            SetModes();
            SceneManager.LoadScene("RLR");
        }

        public void VoteDifficultyNormal()
        {
            voteDifficulty = GameControl.Difficulty.Normal;
        }

        public void VoteDifficultyHard()
        {
            voteDifficulty = GameControl.Difficulty.Hard;
        }

        public void VoteModeClassic()
        {
            voteGameMode = GameControl.Gamemode.Classic;
        }

        public void VoteModeTime()
        {
            voteGameMode = GameControl.Gamemode.TimeMode;
        }

        public void VoteModePractice()
        {
            voteGameMode = GameControl.Gamemode.Practice;
        }

        public void SetModes()
        {
            GameControl.SetDifficulty = (GameControl.Difficulty)voteDifficulty;
            GameControl.SetGameMode = (GameControl.Gamemode)voteGameMode;
        }

        public void HighScores()
        {
            gameObject.SetActive(false);
            RLRMenuActive = false;
            HighScoreMenu.gameObject.SetActive(true);
            HighScoreMenuSLA.HighScoreMenuActive = true;
        }

        public void BackToMenu()
        {
            RLRMenuActive = false;
            gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }

        public void Update()
        {
            if (!voteDifficulty.Equals(null) && !voteGameMode.Equals(null))
            {
                LaunchRLR.GetComponentInChildren<Text>().text = "__________ R U N __________";
                LaunchRLR.GetComponent<Button>().interactable = true;
            }
            else
            {
                LaunchRLR.GetComponentInChildren<Text>().text = "Select Modes";
                LaunchRLR.GetComponent<Button>().interactable = false;
            }
        }
    }
}
