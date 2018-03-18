using System.Collections.Generic;
using Client.Scripts;
using UnityEngine;

namespace Game.Scripts.Characters.CharacterRepositories
{
    public class CharacterRepositoryPlayerPrefs : ICharacterRepository
    {
        public void Add(int id, Character character)
        {
            if(!PlayerPrefs.HasKey(PrefsStringBuilder(id, "Occupied")))
            {
                PlayerPrefs.SetInt(PrefsStringBuilder(id, "Occupied"), 1);
                UpdateRepository(id, character.Name, 0, 0, 0, 0, 1, new int[character.Abilities.Length]);
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
                    ArrayPrefs.GetIntArray(PrefsStringBuilder(id, "AbilityLevels")));
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
                character.EnergyPoints, character.Exp, character.Level, character.AbilityLevels);
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
                                                   int exp, int level, int[] abilityLevels)
        {
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "SpeedPoints"), speedPoints);
            PlayerPrefs.SetString(PrefsStringBuilder(id, "Character"), characterName);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "RegenPoints"), regenPoints);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "EnergyPoints"), energyPoints);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "Exp"), exp);
            PlayerPrefs.SetInt(PrefsStringBuilder(id, "Level"), level);
            ArrayPrefs.SetIntArray(PrefsStringBuilder(id, "AbilityLevels"), abilityLevels);
        }

        private static string PrefsStringBuilder(int id, string attribute)
        {
            return string.Concat("char", id.ToString(), attribute);
        }
    }
}
