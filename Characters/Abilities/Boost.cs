using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Types;
using Characters.Types.Features;

namespace Characters.Abilities
{
    public class Boost : AAbility
    {
        public float BoostSpeed
        {
            get { return Level*3F + 5F; }
        }

        public override int Cooldown
        {
            get { return 1; }
        }

        public override int EnergyCost
        {
            get { return 6 - Level; }
        }

        public Boost(ACharacter character)
        {
            Name = "Boost";
            Level = character.AbilityFirstLevel;
            //EnergyCost = CalculateEnergyCost();
            EnergyDrainPerSecond = 1F;
            IsActive = false;
        }



        public override IEnumerator Enable(ACharacter character)
        {
            if (IsActive) yield return null;
            if (character.UseEnergy(EnergyCost)) // characterd had enough energy and used it
            {
                character.Speed.ActivateBoost(BoostSpeed);
                IsActive = true;
                character.Energy.EnergyDrainPerSec = EnergyDrainPerSecond;
                character.Energy.RegenStatus = RegenStatus.Drain;
            }
        }

        public override void Disable(ACharacter character)
        {
            if (!IsActive) return;
            character.Speed.DeactivateBoost(BoostSpeed);
            character.Energy.RegenStatus = RegenStatus.Regen;
            IsActive = false;
        }

        public override void IncrementLevel()
        {
            Level++;

        }

        











    }
}