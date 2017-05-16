﻿using Assets.Scripts.Launcher;
using Assets.Scripts.UI.RLR_Menus;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Main_Menu
{
    public class RLRMenu : MonoBehaviour
    {

        public HighScoreMenuRLR HighScoreMenuSLA;
        public SceneLoader SceneLoader;

        public GameObject MainMenu;
        public GameObject HighScoreMenu;
        public GameObject LaunchRLR;
        public GameObject Menus;

        private Difficulty? _voteDifficulty;
        private Gamemode? _voteGameMode;

        public bool RLRMenuActive;
        
        public void StartGame()
        {
            GameControl.Instance.State.IsDead = true;
            GameControl.Instance.State.TotalScore = 0;
            GameControl.Instance.State.CurrentLevel = 1;
            SetModes();

            SceneLoader.LoadScene("RLR", 1);
            Menus.SetActive(false);
        }

        private void OnDisable()
        {
            _voteDifficulty = null;
            _voteGameMode = null;
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
            _voteGameMode = Gamemode.Classic;
        }

        public void VoteModeTime()
        {
            _voteGameMode = Gamemode.TimeMode;
        }

        public void VoteModePractice()
        {
            _voteGameMode = Gamemode.Practice;
        }

        public void SetModes()
        {
            GameControl.Instance.State.SetDifficulty = (Difficulty)_voteDifficulty;
            GameControl.Instance.State.SetGameMode = (Gamemode)_voteGameMode;
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
            if (_voteDifficulty != null && _voteGameMode != null)
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
    }
}
