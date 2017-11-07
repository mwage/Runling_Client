using Characters.Types;
using Characters.Types.Features;
using Players;
using System.Collections;

namespace Characters.Abilities
{
    public class Boost : AAbility
    {
        private float BoostSpeed => Level*3 + 5;
        public override int Cooldown => 1;
        public override int EnergyCost => 6 - Level;

        public Boost(ACharacter character, PlayerManager playerManager)
        {
            Name = "Boost";
            Level = character.Ability1Level;
            EnergyDrainPerSecond = 1;
            IsActive = false;
            PlayerManager = playerManager;
            Character = character;
        }

        public override IEnumerator Enable()
        {
            if (IsActive)
                yield break;

            // Check if character has enough energy and use it
            if (Character.Energy.UseEnergy(EnergyCost))
            {
                Character.Speed.ActivateBoost(BoostSpeed);
                IsActive = true;
                TimeToRenew = Cooldown;
                IsUsable = false;
                Character.Energy.EnergyDrainPerSec = EnergyDrainPerSecond;
                Character.Energy.RegenStatus = RegenStatus.Drain;
            }
        }

        public override void Disable()
        {
            if (!IsActive)
                return;

            Character.Speed.DeactivateBoost(BoostSpeed);
            Character.Energy.RegenStatus = RegenStatus.Regen;
            IsActive = false;
        }
    }
}