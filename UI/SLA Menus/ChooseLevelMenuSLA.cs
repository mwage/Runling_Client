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
            GameControl.State.CurrentLevel = 1;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level2()
        {
            GameControl.State.CurrentLevel = 2;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level3()
        {
            GameControl.State.CurrentLevel = 3;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level4()
        {
            GameControl.State.CurrentLevel = 4;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level5()
        {
            GameControl.State.CurrentLevel = 5;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level6()
        {
            GameControl.State.CurrentLevel = 6;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level7()
        {
            GameControl.State.CurrentLevel = 7;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level8()
        {
            GameControl.State.CurrentLevel = 8;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level9()
        {
            GameControl.State.CurrentLevel = 9;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level10()
        {
            GameControl.State.CurrentLevel = 10;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level11()
        {
            GameControl.State.CurrentLevel = 11;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level12()
        {
            GameControl.State.CurrentLevel = 12;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level13()
        {
            GameControl.State.CurrentLevel = 13;
            Time.timeScale = 1;
            GameControl.State.TotalScore = 0;
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
