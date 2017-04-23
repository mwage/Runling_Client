using Assets.Scripts.Launcher;
using Assets.Scripts.UI.SLA_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.SLA
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
            GameControl.GameActive = false;
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.Dead = true;
            GameControl.AutoClickerActive = false;
            GameControl.TotalScore = 0;

            SceneManager.LoadScene("SLA");
        }
    }
}