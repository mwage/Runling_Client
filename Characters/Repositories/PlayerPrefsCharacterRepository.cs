using System.Collections.Generic;
using Characters.Repositories;
using Characters.Types;
using UnityEngine;

namespace Characters.Repositories
{
    public class PlayerPrefsCharacterRepository : ICharacterRepository
    {
        
        public PlayerPrefsCharacterRepository()
        {
        }

        public void Add(int id, string character)
        {
            if(!PlayerPrefs.HasKey(PrefsStringBuilder(id, "Occupied")))
            {
                PlayerPrefs.SetInt(PrefsStringBuilder(id, "Occupied"), 1);
                SetPlayerPrefsCharacterFields(id, character, 0, 0, 0, 0, 1, 0, 0);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("U overwrite the character");
            }
        }

        public CharacterDto Get(int id) 
        {
            if (PlayerPrefs.HasKey(PrefsStringBuilder(id, "Occupied")))
            {
                return new CharacterDto(id,
                                  PlayerPrefs.GetString(PrefsStringBuilder(id, "Character")),
                                  PlayerPrefs.GetInt(PrefsStringBuilder(id, "SpeedPoints")),
                                  PlayerPrefs.GetInt(PrefsStringBuilder(id, "RegenPoints")),
                                  PlayerPrefs.GetInt(PrefsStringBuilder(id, "EnergyPoints")),
                                  PlayerPrefs.GetInt(PrefsStringBuilder(id, "Exp")),
                                  PlayerPrefs.GetInt(PrefsStringBuilder(id, "Level")),
                                  PlayerPrefs.GetInt(PrefsStringBuilder(id, "AbilityFirstLevel")),
                                  PlayerPrefs.GetInt(PrefsStringBuilder(id, "AbilitySecondLevel")),
                                  true);
            }
            else
            {
                return new CharacterDto(false, id); // no saved ling here
            }
        }

        public List<CharacterDto> GetAll()
        {
            List<CharacterDto> characters = new List<CharacterDto>();
            for (int id = 1; id <= LevelingSystem.MaxCharactersAmount; id++)
            {
                characters.Add(Get(id));
            }
            return characters;
        }

        public void Remove(int id)
        {
            if (PlayerPrefs.HasKey(PrefsStringBuilder(id, "Occupied")))
            {
                PlayerPrefs.DeleteKey(PrefsStringBuilder(id, "Occupied"));
                PlayerPrefs.Save();
            } 
        }

        public void UpdatePlayerPrefs(int id, string character, int speedPoints, int regenPoints, int energyPoints, // to change for Dto probably or single method for properties
                           int exp, int level, int abilityFirstLevel, int abilitySecondLevel)
        {
            SetPlayerPrefsCharacterFields(id, character, speedPoints, regenPoints, energyPoints, exp, level,
                                          abilityFirstLevel, abilitySecondLevel);
        }

        private void SetPlayerPrefsCharacterFields(int id, string character, int speedPoints, int regenPoints, int energyPoints,
                                                   int exp, int level, int abilityFirstLevel, int abilitySecondLevel)
        {
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "SpeedPoints"), speedPoints);
            PlayerPrefs.SetString(PrefsStringBuilder(id, "Character"), character);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "RegenPoints"), regenPoints);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "EnergyPoints"), energyPoints);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "Exp"), exp);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "Level"), level);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "AbilityFirstLevel"), abilityFirstLevel);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "AbilitySecondLevel"), abilitySecondLevel);
        }

        private string PrefsStringBuilder(int id, string attribute)
        {
            return string.Concat("char", id.ToString(), attribute);
        }
    }
}
