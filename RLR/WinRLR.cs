using Assets.Scripts.Launcher;
using Assets.Scripts.UI.RLR_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.RLR
{
    public class WinRLR : MonoBehaviour
    {
        public GameObject Background;
        public HighScoreMenuRLR HighScoreMenuRLR;
        public ScoreRLR ScoreRLR;

        public void Awake()
        {
            HighScoreMenuRLR.CreateTextObjects(Background);
            HighScoreMenuRLR.SetNumbers();
        }

        public void BackToMenu()
        {
            GameControl.GameActive = false;
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.Dead = true;
            GameControl.AutoClickerActive = false;
            GameControl.TotalScore = 0;

            SceneManager.LoadScene("RLR");
        }
    }
}