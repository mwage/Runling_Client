using Client.Scripts.UI.Menus.RLR_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.RLR
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
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("RLR");
        }
    }
}