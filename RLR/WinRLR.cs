using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.Scripts.RLR
{
    public class WinRLR : MonoBehaviour
    {

        public void BackToMenu()
        {
            GameControl.Instance.State.GameActive = false;
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.Instance.State.IsDead = true;
            GameControl.Instance.State.AutoClickerActive = false;

            SceneManager.LoadScene("RLR");
        }
    }
}