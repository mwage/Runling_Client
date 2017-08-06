using Characters.Abilities;
using Characters.Types;
using Launcher;
using UnityEngine.UI;

namespace MP.TSGame.Players.AbilitiesButtons
{
    public class AbilityButton
    {
        private bool _isLoaded;
        private Slider _abilitySlider;
        private bool _shouldBeAnimated;
        private ACharacter _characterController;
        private AAbility _ability;
        private string _abilityName;

        public AbilityButton(Slider abilitySlider, string abilityName)
        {
            _abilitySlider = abilitySlider;
            _abilityName = abilityName;
        }

        public void Refresh()
        {
            
        }

        public void SetProgress()
        {
            if (Initialize() == false) return;
            _abilitySlider.value = _ability.GetLoadingProgress();
        }

        private bool Initialize()
        {
            if (_characterController == null)
            {
//                if (GameControl.PlayerState.CharacterController == null) return false;
//                _characterController = GameControl.PlayerState.CharacterController;
                InitializeAbility();
                return true;
            }
            return true;
        }

        private void InitializeAbility()
        {
            switch (_abilityName)
            {
                case ("1"):
                {
                    _ability = _characterController.Ability1;
                    break;
                }
                case "2":
                {
                    _ability = _characterController.Ability2;
                    break;
                }
                case "Ultimatum":
                {
                    _ability = _characterController.Ability1;
                    break;
                }
            }
        }

        private void SetShouldBeAnimated()
        {
            if (!_shouldBeAnimated)
            {
                if (_ability.IsLoaded)
                {
                    _shouldBeAnimated = true;
                }
            }
        }
    }
}