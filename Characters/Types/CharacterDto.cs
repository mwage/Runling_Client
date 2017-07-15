using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Characters.Types
{
    public class CharacterDto
    {
        public bool Occupied { get; protected set; }
        public int Id { get; protected set; }
        public string Character { get; protected set; }
        public int SpeedPoints { get; protected set; }
        public int RegenPoints { get; protected set; }
        public int EnergyPoints { get; protected set; }
        public int Exp { get; protected set; }
        public int Level { get; protected set; }
        public int AbilityFirstLevel { get; protected set; }
        public int AbilitySecondLevel { get; protected set; }
        public int EnergyCurrent { get; protected set; }


        public CharacterDto(int id, string character, int speedPoints, int regenPoints, int energyPoints,
                          int exp, int level, int abilityFirstLevel, int abilitySecondLevel, bool occupied = true)
        {
            Id = id;
            Character = character;
            SpeedPoints = speedPoints;
            RegenPoints = regenPoints;
            EnergyPoints = energyPoints;
            Exp = exp;
            Level = level;
            AbilityFirstLevel = abilityFirstLevel;
            AbilitySecondLevel = abilitySecondLevel;
            EnergyCurrent = 0;
            Occupied = occupied;
        }

        public CharacterDto(bool occupied, int id)
        {
            Occupied = occupied;
            Id = id;
        }
    }
}
