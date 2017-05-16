using Assets.Scripts.Launcher;
using Assets.Scripts.SLA;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.SLA_Menus
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
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.State.IsDead = true;
            GameControl.State.AutoClickerActive = false;
            GameControl.State.TotalScore = 0;

            SceneManager.LoadScene("SLA");
        }
    }
}