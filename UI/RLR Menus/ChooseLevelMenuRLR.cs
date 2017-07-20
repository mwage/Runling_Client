using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.RLR_Menus
{
    public class ChooseLevelMenuRLR : MonoBehaviour
    {
        [SerializeField] private InGameMenuManagerRLR _inGameMenuManagerRLR;

        #region Buttons

        public void Level1()
        {
            GameControl.GameState.CurrentLevel = 1;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level2()
        {
            GameControl.GameState.CurrentLevel = 2;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level3()
        {
            GameControl.GameState.CurrentLevel = 3;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level4()
        {
            GameControl.GameState.CurrentLevel = 4;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level5()
        {
            GameControl.GameState.CurrentLevel = 5;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level6()
        {
            GameControl.GameState.CurrentLevel = 6;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level7()
        {
            GameControl.GameState.CurrentLevel = 7;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level8()
        {
            GameControl.GameState.CurrentLevel = 8;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level9()
        {
            GameControl.GameState.CurrentLevel = 9;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Back()
        {
            gameObject.SetActive(false);
            _inGameMenuManagerRLR.InGameMenu.gameObject.SetActive(true);
        }
        #endregion
    }
}
