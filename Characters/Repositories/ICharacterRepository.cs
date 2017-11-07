using System.Collections.Generic;

namespace Characters.Repositories
{
    public interface ICharacterRepository
    {
        void Add(int id, string character);
        void Remove(int id);
        CharacterDto Get(int id);
        List<CharacterDto> GetAll();
        void UpdateRepository(CharacterDto character);
        void UpdateRepository(int id, string character, int speedPoints, int regenPoints, int energyPoints,
            int exp, int level, int abilityFirstLevel, int abilitySecondLevel);
    }
}
