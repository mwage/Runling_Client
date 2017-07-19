using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Characters.Types;

namespace Characters.Abilities
{
    public abstract class AAbility
    {
        public string Name { get; protected set; }
        public int Level { get; protected set; }
        public int Cooldown { get; protected set; }
        public int EnergyCost { get; protected set; }
        public int EnergyDrainPerSecond { get; protected set; }

        public bool IsActive;

        public abstract void Enable(ACharacter character);
        public abstract void Disable(ACharacter character);

        protected abstract int CalculateCooldown();

        protected abstract int CalculateEnergyCost();

    }
}
