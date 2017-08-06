using Characters.Types;
using Players;
using UnityEngine;

namespace Characters.Bars
{
    public class PlayerBarsManager : MonoBehaviour
    {
        public EnergyBar EnergyBar;
        public LevelBar LevelBar;

        private ACharacter _characterController;

        public void Initialize(PlayerManager playerManager)
        {
            _characterController = playerManager.CharacterController;
        }

        private void Update()
        {
            if (_characterController != null)
                UpdateEnergyBar();
        }
//
//        public void UpdateAll()
//        {
//            UpdateEnergyBar();
//            UpdateLevelBar();
//        }

        public void UpdateEnergyBar()
        {
            EnergyBar.SetText(string.Format("{0}/{1}", (int)_characterController.Energy.Current, _characterController.Energy.Max));
            EnergyBar.SetProgress(_characterController.Energy.Current / _characterController.Energy.Max);
        }

        public void UpdateLevelBar()
        {
            LevelBar.SetText(_characterController.Level);
            LevelBar.SetProgress(_characterController.Level, _characterController.Exp);
        }
    }
}
