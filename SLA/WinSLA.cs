using Launcher;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLA
{
    public class WinSLA : MonoBehaviour
    {
        public GameObject Background;
        public HighScoreMenuSLA HighScoreMenuSLA;
        public ScoreSLA ScoreSLA;

        public void Awake()
        {
            HighScoreMenuSLA.CreateTextObjects(Background);
            HighScoreMenuSLA.SetNumbers();
        }

        public void BackToMenu()
        {
            GameControl.GameState.GameActive = false;
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.PlayerState.IsDead = true;
            GameControl.PlayerState.AutoClickerActive = false;
            GameControl.PlayerState.TotalScore = 0;
            GameControl.GameState.CurrentLevel = 1;

            SceneManager.LoadSceneAsync("SLA");
        }
    }
}