using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.RLR_Menus
{
    public class ChooseLevelMenuRLR : MonoBehaviour
    {
        public bool ChooseLevelMenuActive;
        public GameObject Menu;

        public void Level1()
        {
            GameControl.State.CurrentLevel = 1;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level2()
        {
            GameControl.State.CurrentLevel = 2;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level3()
        {
            GameControl.State.CurrentLevel = 3;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level4()
        {
            GameControl.State.CurrentLevel = 4;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level5()
        {
            GameControl.State.CurrentLevel = 5;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level6()
        {
            GameControl.State.CurrentLevel = 6;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level7()
        {
            GameControl.State.CurrentLevel = 7;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level8()
        {
            GameControl.State.CurrentLevel = 8;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Level9()
        {
            GameControl.State.CurrentLevel = 9;
            Time.timeScale = 1;
            SceneManager.LoadScene("RLR");
        }

        public void Back()
        {
            ChooseLevelMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }
    }
}
