using Launcher;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MP.TSGame.SLA
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
            GameControl.GameState.CurrentLevel = 1;

            SceneManager.LoadSceneAsync("SLA");
        }
    }
}