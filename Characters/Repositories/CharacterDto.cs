namespace Characters.Repositories
{
    public class CharacterDto
    {
        public bool Occupied { get; }
        public int Id { get; }
        public string Name { get; }
        public int SpeedPoints { get; }
        public int RegenPoints { get; }
        public int EnergyPoints { get; }
        public int Exp { get; }
        public int Level { get; }
        public int FirstAbilityLevel { get; }
        public int SecondAbilityLevel { get; }


        public CharacterDto(int id, string name, int speedPoints, int regenPoints, int energyPoints,
                          int exp, int level, int firstAbilityLevel, int secondAbilityLevel, bool occupied = true)
        {
            Id = id;
            Name = name;
            SpeedPoints = speedPoints;
            RegenPoints = regenPoints;
            EnergyPoints = energyPoints;
            Exp = exp;
            Level = level;
            FirstAbilityLevel = firstAbilityLevel;
            SecondAbilityLevel = secondAbilityLevel;
            Occupied = occupied;
        }

        public CharacterDto(int id, bool occupied)
        {
            Occupied = occupied;
            Id = id;
        }
    }
}
