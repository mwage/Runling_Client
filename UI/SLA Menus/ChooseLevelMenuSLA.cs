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
            GameControl.Instance.State.CurrentLevel = 1;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level2()
        {
            GameControl.Instance.State.CurrentLevel = 2;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level3()
        {
            GameControl.Instance.State.CurrentLevel = 3;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level4()
        {
            GameControl.Instance.State.CurrentLevel = 4;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level5()
        {
            GameControl.Instance.State.CurrentLevel = 5;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level6()
        {
            GameControl.Instance.State.CurrentLevel = 6;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level7()
        {
            GameControl.Instance.State.CurrentLevel = 7;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level8()
        {
            GameControl.Instance.State.CurrentLevel = 8;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level9()
        {
            GameControl.Instance.State.CurrentLevel = 9;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level10()
        {
            GameControl.Instance.State.CurrentLevel = 10;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level11()
        {
            GameControl.Instance.State.CurrentLevel = 11;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level12()
        {
            GameControl.Instance.State.CurrentLevel = 12;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
            SceneManager.LoadScene("SLA");
        }

        public void Level13()
        {
            GameControl.Instance.State.CurrentLevel = 13;
            Time.timeScale = 1;
            GameControl.Instance.State.TotalScore = 0;
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
