using Game.Scripts.Characters;
using Game.Scripts.Characters.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.PlayerInput.Buttons
{
    public class AbilityButton : MonoBehaviour
    {
        public Image AbilityIcon;
        public Image CooldownMask;
        public Text CooldownText;

        private AAbility _ability;
        private CharacterManager _characterManager;
        
        public void Initialize(AAbility ability, CharacterManager characterManager)
        {
            _ability = ability;
            _characterManager = characterManager;
            AbilityIcon.sprite = ability.Icon;
            CooldownMask.fillAmount = 0;
            CooldownText.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void SetProgress()
        {
            if (_ability.Level == 0)
                return;
            
            // Show button if ability level != 0
            gameObject.SetActive(true);
            CooldownMask.fillAmount = 1 - _ability.GetLoadingProgress();

            if (_ability.TimeToRenew >= Mathf.Epsilon)
            {
                CooldownText.gameObject.SetActive(true);
                CooldownText.text = Mathf.Round(_ability.TimeToRenew).ToString();

            }
            else
            {
                CooldownText.gameObject.SetActive(false);

                if (!_ability.IsToggle)
                    return;

                CooldownMask.fillAmount = _ability.IsActive ? 1 : 0;
            }
        }

        public void ActivateAbility()
        {
            _characterManager.ActivateOrDeactivateAbility(_ability);
        }
    }
}