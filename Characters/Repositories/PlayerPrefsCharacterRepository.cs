using System.Collections.Generic;
using UnityEngine;

namespace Characters.Repositories
{
    public class CharacterRepositoryPlayerPrefs : ICharacterRepository
    {
        public void Add(int id, string character)
        {
            if(!PlayerPrefs.HasKey(PrefsStringBuilder(id, "Occupied")))
            {
                PlayerPrefs.SetInt(PrefsStringBuilder(id, "Occupied"), 1);
                UpdateRepository(id, character, 0, 0, 0, 0, 1, 0, 0);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("You tried to overwrite a character.");
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
                                  PlayerPrefs.GetInt(PrefsStringBuilder(id, "Ability1Level")),
                                  PlayerPrefs.GetInt(PrefsStringBuilder(id, "Ability2Level")));
            }

            // No saved character for this slot
            return new CharacterDto(id, false); 
        }

        public List<CharacterDto> GetAll()
        {
            var characters = new List<CharacterDto>();
            for (var i = 1; i <= LevelingSystem.MaxCharactersAmount; i++)
            {
                characters.Add(Get(i));
            }
            return characters;
        }

        public void UpdateRepository(CharacterDto character)
        {
            UpdateRepository(character.Id, character.Name, character.SpeedPoints, character.RegenPoints,
                character.EnergyPoints, character.Exp, character.Level, character.FirstAbilityLevel,
                character.SecondAbilityLevel);
        }

        public void Remove(int id)
        {
            if (PlayerPrefs.HasKey(PrefsStringBuilder(id, "Occupied")))
            {
                PlayerPrefs.DeleteKey(PrefsStringBuilder(id, "Occupied"));
                PlayerPrefs.Save();
            } 
        }

        public void UpdateRepository(int id, string characterName, int speedPoints, int regenPoints, int energyPoints,
                                                   int exp, int level, int firstAbilityLevel, int secondAbilityLevel)
        {
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "SpeedPoints"), speedPoints);
            PlayerPrefs.SetString(PrefsStringBuilder(id, "Character"), characterName);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "RegenPoints"), regenPoints);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "EnergyPoints"), energyPoints);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "Exp"), exp);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "Level"), level);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "Ability1Level"), firstAbilityLevel);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "Ability2Level"), secondAbilityLevel);
        }

        private static string PrefsStringBuilder(int id, string attribute)
        {
            return string.Concat("char", id.ToString(), attribute);
        }
    }
}
