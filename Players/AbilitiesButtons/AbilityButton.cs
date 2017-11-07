using Characters.Abilities;
using UnityEngine.UI;

namespace Players.AbilitiesButtons
{
    public class AbilityButton
    {
        private AAbility _ability;
        private readonly string _abilityName;
        private readonly Slider _abilitySlider;
        private bool _isLoaded;
        private readonly PlayerManager _playerManager;
        private bool _shouldBeAnimated;

        public AbilityButton(Slider abilitySlider, string abilityName, PlayerManager playerManager)
        {
            _abilitySlider = abilitySlider;
            _abilityName = abilityName;
            _playerManager = playerManager;
            InitializeAbility();
        }

        public void SetProgress()
        {
            if (_playerManager != null)
                _abilitySlider.value = _ability.GetLoadingProgress();
        }

        private void InitializeAbility()
        {
            switch (_abilityName)
            {
                case "1":
                {
                    _ability = _playerManager.CharacterController.Ability1;
                    break;
                }
                case "2":
                {
                    _ability = _playerManager.CharacterController.Ability2;
                    break;
                }
                case "Ultimatum":
                {
                    _ability = _playerManager.CharacterController.Ability1;
                    break;
                }
            }
        }

        private void SetShouldBeAnimated()
        {
            if (!_shouldBeAnimated)
                if (_ability.IsUsable)
                    _shouldBeAnimated = true;
        }
    }
}