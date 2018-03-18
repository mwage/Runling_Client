using System.Collections;
using UnityEngine;

namespace Game.Scripts.Characters.Abilities
{
    [CreateAssetMenu(fileName = "New Boost Ability", menuName = "Ability/Boost")]
    public class Boost : AAbility
    {
        public float EnergyDrainPerSecond = 1;
        public float BaseSpeed = 2;
        public float IncreasePerLevel = 1;

        public override bool IsToggle => true;
        private float BoostSpeed => BaseSpeed + IncreasePerLevel * (Level - 1);
        
        public override IEnumerator Enable()
        {
            if (IsActive)
                yield break;

            // Check if character has enough energy and use it
            if (CharacterManager.Energy.UseEnergy(ActivationCost))
            {
                CharacterManager.Speed.ActivateBoost(BoostSpeed);
                IsActive = true;
                IsUsable = false;
                Animation.SetActive(true);
                TimeToRenew = Cooldown;
                CharacterManager.Energy.EnableEnergyDrain(EnergyDrainPerSecond);
            }
        }

        public override void Disable()
        {
            if (!IsActive)
                return;

            CharacterManager.Speed.DeactivateBoost(BoostSpeed);
            CharacterManager.Energy.DisableEnergyDrain();
            Animation.SetActive(false);
            IsActive = false;
        }
    }
}