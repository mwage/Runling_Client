



using Characters.Types;
using Launcher;

namespace Characters.Bars
{
    public class PlayerBarsManager
    {
        public EnergyBar EnergyBar;
        public LevelBar LevelBar;

        private ACharacter _characterController;

        public void Awake()
        {
            _characterController = GameControl.PlayerState.CharacterController;
        }

        public void UpdateAll()
        {
            UpdateEnergyBar();
            UpdateLevelBar();
        }

        public void UpdateEnergyBar()
        {
            EnergyBar.SetText(string.Format("{0}/{1}", (int)_characterController.EnergyCurrent, _characterController.EnergyMax));
            EnergyBar.SetProgress(_characterController.EnergyCurrent / _characterController.EnergyMax);
        }

        public void UpdateLevelBar()
        {
            LevelBar.SetText(_characterController.Level);
            LevelBar.SetProgress(_characterController.Level, _characterController.Exp);
        }
    }
}
