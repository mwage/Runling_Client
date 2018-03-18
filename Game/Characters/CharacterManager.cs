using Game.Scripts.Characters.Abilities;
using Game.Scripts.Characters.Features;
using UnityEngine;

namespace Game.Scripts.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        public Character Character { get; private set; }
        public CharacterStats Stats { get; private set; }
        public Energy Energy { get; private set; }
        public Speed Speed { get; private set; }

        public void InitializeCharacter(Character character, CharacterStats stats)
        {
            Character = character;
            Stats = stats;
            Speed = new Speed(stats, character.Settings);

            // If character doesn't have Abilities, it doesn't need Energy
            Energy = character.Abilities.Length != 0 ? new Energy(stats, character.Settings) : null;
        }

        // Update Energy and Abilities
        private void Update()
        {
            if (Energy == null)
                return;

            Energy.RefreshEnergy();
            RefreshCooldowns();
            if (Energy.IsExhausted)
            {
                DisableAllSkills();
            }
        }

        public void DisableAllSkills()
        {
            foreach (var ability in Character.Abilities)
            {
                ability.Disable();
            }
        }

        // TODO: Better way to select Ability Inputs, like a preset Grid to set hotkeys for each gridcomponent.
        public void ActivateOrDeactivateAbility(int abilityId)
        {
            if (Character.Abilities.Length < abilityId + 1)
                return;

            var ability = Character.Abilities[abilityId];

            if (ability == null)
            {
                Debug.Log("Invalid ability or ability not set!");
                return;
            }

            ActivateOrDeactivateAbility(ability);
        }

        public void ActivateOrDeactivateAbility(AAbility ability)
        {

            if (ability.Level == 0)
                return;

            // Activate
            if (!ability.IsActive)
            {
                StartCoroutine(ability.Enable());
            }
            // Deactivate
            else
            {
                if (ability.IsToggle && ability.IsUsable)
                {
                    ability.Disable();
                }
            }
        }

        private void RefreshCooldowns()
        {
            foreach (var ability in Character.Abilities)
            {
                ability.RefreshCooldown();
            }
        }
    }
}
