using Client.Scripts.UI.Menus.SLA_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.SLA
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
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public void RestartGame()
        {
            SceneManager.LoadSceneAsync("SLA");
        }
    }
}