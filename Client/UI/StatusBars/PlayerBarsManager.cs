using Game.Scripts.Characters;
using Game.Scripts.Players;
using UnityEngine;

namespace Client.Scripts.UI.StatusBars
{
    public class PlayerBarsManager : MonoBehaviour
    {
        public EnergyBar EnergyBar;
        public LevelBar LevelBar;

        private CharacterManager _character;

        public void Initialize(PlayerManager playerManager)
        {
            _character = playerManager.CharacterManager;
        }

        private void Update()
        {
            if (_character != null)
            {
                UpdateEnergyBar();
            }
        }

        public void UpdateEnergyBar()
        {
            EnergyBar.SetText($"{(int) _character.Energy.Current}/{_character.Energy.Max}");
            EnergyBar.SetProgress(_character.Energy.Current / _character.Energy.Max);
        }

        public void UpdateLevelBar()
        {
            LevelBar.SetText(_character.Stats.Level);
            LevelBar.SetProgress(_character.Stats.Level, _character.Stats.Exp);
        }
    }
}
