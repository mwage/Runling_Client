using System;
using System.Collections.Generic;
using Characters.Repositories;
using System.Linq;
using System.Text;

namespace Characters.Types
{
    public interface ICharacterRepository
    {
        void Add(int id, string character);
        void Remove(int id);
        CharacterDto Get(int id);
        List<CharacterDto> GetAll();
        void UpdatePlayerPrefs(int id, string character, int speedPoints, int regenPoints, int energyPoints,
            int exp, int level, int abilityFirstLevel, int abilitySecondLevel);
    }
}
