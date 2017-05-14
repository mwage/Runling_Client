using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.SLA_Menus
{
    public class ChooseLevelMenuSLA : MonoBehaviour
    {
        public bool ChooseLevelMenuActive;
        public GameObject Menu;

        public void Level1()
        {
            GameControl.CurrentLevel = 1;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level2()
        {
            GameControl.CurrentLevel = 2;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level3()
        {
            GameControl.CurrentLevel = 3;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level4()
        {
            GameControl.CurrentLevel = 4;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level5()
        {
            GameControl.CurrentLevel = 5;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level6()
        {
            GameControl.CurrentLevel = 6;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level7()
        {
            GameControl.CurrentLevel = 7;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level8()
        {
            GameControl.CurrentLevel = 8;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level9()
        {
            GameControl.CurrentLevel = 9;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level10()
        {
            GameControl.CurrentLevel = 10;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level11()
        {
            GameControl.CurrentLevel = 11;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level12()
        {
            GameControl.CurrentLevel = 12;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level13()
        {
            GameControl.CurrentLevel = 13;
            Time.timeScale = 1;
            GameControl.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }


        public void Back()
        {
            ChooseLevelMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }
    }
}
