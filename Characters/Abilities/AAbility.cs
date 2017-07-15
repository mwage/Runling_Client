using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Characters.Abilities
{
    public abstract class AAbility
    {
        public string Name { get; protected set; }
        public int Level { get; protected set; }
        public int Cooldown { get; protected set; }
        public int EnergyCost { get; protected set; }
        public int EnergyDrainPerSecond { get; protected set; }
    }
}
