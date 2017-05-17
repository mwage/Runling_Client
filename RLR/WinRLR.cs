using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RLR
{
    public class WinRLR : MonoBehaviour
    {

        public void BackToMenu()
        {
            GameControl.State.GameActive = false;
            GameControl.State.IsSafe = false;
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.State.IsDead = true;
            GameControl.State.AutoClickerActive = false;

            SceneManager.LoadScene("RLR");
        }
    }
}