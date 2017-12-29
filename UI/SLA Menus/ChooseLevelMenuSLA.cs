using Players;
using SLA;
using UnityEngine;

namespace UI.SLA_Menus
{
    public class ChooseLevelMenuSLA : AMenu
    {
        [SerializeField] private ControlSLA _controlSLA;

        private LevelManagerSLA _levelManager;
        private InitializeGameSLA _initializeGame;
        private MenuManager _menuManager;

        private void Awake()
        {
            _initializeGame = _controlSLA?.GetComponent<InitializeGameSLA>();
            _levelManager = _controlSLA?.GetComponent<LevelManagerSLA>();
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

        public void Level10()
        {
            PickLevel(10);
        }

        public void Level11()
        {
            PickLevel(11);
        }

        public void Level12()
        {
            PickLevel(12);
        }

        public void Level13()
        {
            PickLevel(13);
        }

        private void PickLevel(int level)
        {
            _controlSLA.CurrentLevel = level;
            _controlSLA.StopAllCoroutines();
            _initializeGame.Countdown(0);
            KillPlayer(_controlSLA.PlayerManagers[0]);
            _levelManager.EndLevel(0);

            _menuManager.CloseMenu(this);
        }

        private static void KillPlayer(PlayerManager playerManager)
        {
            playerManager.IsImmobile = true;
            playerManager.IsInvulnerable = true;
            playerManager.Model.SetActive(false);
            playerManager.Shield.SetActive(false);
        }

        public override void Back()
        {
            _menuManager.Menu.SetActive(true);
        }
        #endregion
    }
}
