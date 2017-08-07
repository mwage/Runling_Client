using System.Collections;
using Characters.Types;
using Characters.Types.Features;
using Players;

namespace Characters.Abilities
{
    public class Boost : AAbility
    {
        private readonly PlayerManager _playerManager;   // Use later for boost animation

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

        public Boost(ACharacter character, PlayerManager playerManager)
        {
            Name = "Boost";
            Level = character.Ability1Level;
            //EnergyCost = CalculateEnergyCost();
            EnergyDrainPerSecond = 1F;
            IsActive = false;
            _playerManager = playerManager;
        }

        public override IEnumerator Enable(ACharacter character)
        {
            if (IsActive) yield return null;
            if (character.UseEnergy(EnergyCost)) // character had enough energy and used it
            {
                character.Speed.ActivateBoost(BoostSpeed);
                IsActive = true;
                TimeToRenew = Cooldown;
                IsLoaded = false;
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