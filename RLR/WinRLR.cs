using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.Scripts.RLR
{
    public class WinRLR : MonoBehaviour
    {

        public void BackToMenu()
        {
            GameControl.GameActive = false;
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.Dead = true;
            GameControl.AutoClickerActive = false;

            SceneManager.LoadScene("RLR");
        }
    }
}