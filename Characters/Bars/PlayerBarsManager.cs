



using Characters.Types;
using Launcher;
using UnityEngine;

namespace Characters.Bars
{
    public class PlayerBarsManager : MonoBehaviour
    {
        public EnergyBar EnergyBar;
        public LevelBar LevelBar;

        private ACharacter _characterController;

        public void Start()
        {
            _characterController = GameControl.PlayerState.CharacterController;
        }

        public void Update()
        {
            UpdateEnergyBar();
        }

        public void UpdateAll()
        {
            UpdateEnergyBar();
            UpdateLevelBar();
        }

        public void UpdateEnergyBar()
        {
            //if (EnergyBar == null) return;
            //if (_characterController == null) return;
            if (_characterController == null)
            {
                Start();
                return;
            }
            EnergyBar.SetText(string.Format("{0}/{1}", (int)_characterController.Energy.Current, _characterController.Energy.Max));
            EnergyBar.SetProgress(_characterController.Energy.Current / _characterController.Energy.Max);
        }

        public void UpdateLevelBar()
        {
            if (_characterController == null) _characterController = GameControl.PlayerState.CharacterController;
            LevelBar.SetText(_characterController.Level);
            LevelBar.SetProgress(_characterController.Level, _characterController.Exp);
        }
    }
}
