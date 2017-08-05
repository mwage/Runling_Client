using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.SLA_Menus
{
    public class ChooseLevelMenuSLA : MonoBehaviour
    {
        [SerializeField] private InGameMenuManagerSLA _inGameMenuManagerSLA;

        #region Buttons

        public void Level1()
        {
            GameControl.GameState.CurrentLevel = 1;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level2()
        {
            GameControl.GameState.CurrentLevel = 2;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level3()
        {
            GameControl.GameState.CurrentLevel = 3;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level4()
        {
            GameControl.GameState.CurrentLevel = 4;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level5()
        {
            GameControl.GameState.CurrentLevel = 5;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level6()
        {
            GameControl.GameState.CurrentLevel = 6;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level7()
        {
            GameControl.GameState.CurrentLevel = 7;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level8()
        {
            GameControl.GameState.CurrentLevel = 8;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level9()
        {
            GameControl.GameState.CurrentLevel = 9;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level10()
        {
            GameControl.GameState.CurrentLevel = 10;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level11()
        {
            GameControl.GameState.CurrentLevel = 11;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level12()
        {
            GameControl.GameState.CurrentLevel = 12;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Level13()
        {
            GameControl.GameState.CurrentLevel = 13;
            Time.timeScale = 1;
            SceneManager.LoadScene("SLA");
        }

        public void Back()
        {
            gameObject.SetActive(false);
            _inGameMenuManagerSLA.InGameMenu.gameObject.SetActive(true);
        }
        #endregion
    }
}
