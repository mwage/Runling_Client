using UnityEngine;
using Characters.Types;

namespace Characters.Abilities
{
    public class Boost : AAbility
    {
        public float BoostSpeed
        {
            get { return 5; }
        }

        public Boost(ACharacter character)
        {
            Name = "Boost";
            Level = character.AbilityFirstLevel;
            Cooldown = CalculateCooldown();
            EnergyCost = CalculateEnergyCost();
            EnergyDrainPerSecond = 1;
            IsActive = false;
        }



        public override void Enable(ACharacter character)
        {
            if (IsActive) return;
            if (character.UseEnergy(EnergyCost)) // characterd had enough energy and used it
            {
                character.Speed.ActivateBoost(BoostSpeed);
                IsActive = true;
                character.Energy.EnergyDrainPerSec = EnergyDrainPerSecond;
            }
        }

        public override void Disable(ACharacter character)
        {
            if (!IsActive) return;
            character.Speed.DeactivateBoost(BoostSpeed);
            IsActive = false;
        }





        protected override int CalculateCooldown()
        {
            return 1;
        }

        protected override int CalculateEnergyCost()
        {
            return 5;
        }






    }
}