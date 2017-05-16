using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.Scripts.RLR
{
    public class WinRLR : MonoBehaviour
    {

        public void BackToMenu()
        {
            GameControl.State.GameActive = false;
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