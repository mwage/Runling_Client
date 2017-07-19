using Launcher;
using UI.RLR_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RLR
{
    public class WinRLR : MonoBehaviour
    {
        public GameObject Background;
        public HighScoreMenuRLR HighScoreMenuRLR;


        public void Awake()
        {
            Background.SetActive(true);
            HighScoreMenuRLR.CreateTextObjects(Background);
            HighScoreMenuRLR.SetNumbers();
        }

        public void BackToMenu()
        {
            GameControl.GameState.GameActive = false;
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.PlayerState.IsDead = true;
            GameControl.GameState.AutoClickerActive = false;
            GameControl.GameState.CurrentLevel = 1;

            SceneManager.LoadScene("RLR");
        }
    }
}