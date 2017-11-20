using Launcher;
using SLA;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.SLA_Menus
{
    public class ChooseLevelMenuSLA : MonoBehaviour
    {
        [SerializeField] private InGameMenuManagerSLA _inGameMenuManagerSLA;
        [SerializeField] private ControlSLA _controlSLA;
        [SerializeField] private LevelManagerSLA _levelManager;

        #region Buttons

        public void Level1()
        {
            _controlSLA.CurrentLevel = 1;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level2()
        {
            _controlSLA.CurrentLevel = 2;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level3()
        {
            _controlSLA.CurrentLevel = 3;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level4()
        {
            _controlSLA.CurrentLevel = 4;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level5()
        {
            _controlSLA.CurrentLevel = 5;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level6()
        {
            _controlSLA.CurrentLevel = 6;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level7()
        {
            _controlSLA.CurrentLevel = 7;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level8()
        {
            _controlSLA.CurrentLevel = 8;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level9()
        {
            _controlSLA.CurrentLevel = 9;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level10()
        {
            _controlSLA.CurrentLevel = 10;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level11()
        {
            _controlSLA.CurrentLevel = 11;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level12()
        {
            _controlSLA.CurrentLevel = 12;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Level13()
        {
            _controlSLA.CurrentLevel = 13;
            _levelManager.EndLevel(0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Back()
        {
            gameObject.SetActive(false);
            _inGameMenuManagerSLA.InGameMenu.gameObject.SetActive(true);
        }
        #endregion

    }
}
