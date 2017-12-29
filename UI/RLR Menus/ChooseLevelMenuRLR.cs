using Players;
using RLR;
using UnityEngine;

namespace UI.RLR_Menus
{
    public class ChooseLevelMenuRLR : AMenu
    {
        [SerializeField] private ControlRLR _controlRLR;

        private LevelManagerRLR _levelManager;
        private InitializeGameRLR _initializeGame;
        private MenuManager _menuManager;

        private void Awake()
        {
            _initializeGame = _controlRLR.GetComponent<InitializeGameRLR>();
            _levelManager = _controlRLR.GetComponent<LevelManagerRLR>();
            _menuManager = transform.parent.GetComponent<MenuManager>();
        }

        private void OnEnable()
        {
            _menuManager.ActiveMenu?.gameObject.SetActive(false);
            _menuManager.ActiveMenu = this;
        }

        #region Buttons

        public void Level1()
        {
            PickLevel(1);
        }

        public void Level2()
        {
            PickLevel(2);
        }

        public void Level3()
        {
            PickLevel(3);
        }

        public void Level4()
        {
            PickLevel(4);
        }

        public void Level5()
        {
            PickLevel(5);
        }

        public void Level6()
        {
            PickLevel(6);
        }

        public void Level7()
        {
            PickLevel(7);
        }

        public void Level8()
        {
            PickLevel(8);
        }

        public void Level9()
        {
            PickLevel(9);
        }

        private void PickLevel(int level)
        {
            _controlRLR.CurrentLevel = level - 1;
            _controlRLR.StopAllCoroutines();
            _levelManager.EndLevel(0);

            // if there is an active countdown, destroy it
            _initializeGame.Countdown(0);

            _menuManager.CloseMenu(this);
        }

        public override void Back()
        {
            _menuManager.Menu.SetActive(true);
        }
        #endregion
    }
}
