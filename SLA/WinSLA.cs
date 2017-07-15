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
            GameControl.State.GameActive = false;
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.State.IsDead = true;
            GameControl.State.AutoClickerActive = false;
            GameControl.State.TotalScore = 0;
            GameControl.State.CurrentLevel = 1;

            SceneManager.LoadSceneAsync("SLA");
        }
    }
}