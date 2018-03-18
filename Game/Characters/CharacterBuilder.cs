using Game.Scripts.Characters.CharacterRepositories;
using Game.Scripts.Players;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Characters
{
    public class CharacterBuilder : MonoBehaviour
    {
        public CharacterStorage Characters;

        /// <summary>
        /// Create a character from CharacterDto
        /// </summary>
        public PlayerManager Create(CharacterDto characterDto, ICharacterRepository repository)
        {
            var character = Characters.AvailableCharacters.FirstOrDefault(c => c.Name == characterDto.Name);
            if (character == null)
            {
                Debug.LogError("Failed to load character!");
                return null;
            }

            var playerManager = character.Initialize(transform);
            
            // Set characters stats and abilities
            var stats = new CharacterStats(characterDto, repository);

            // Initialize character
            playerManager.CharacterManager.InitializeCharacter(character, stats);
            return playerManager;
        }

        /// <summary>
        /// Create a character from name
        /// </summary>
        public PlayerManager Create(string characterName)
        {
            var character = Characters.AvailableCharacters.FirstOrDefault(c => c.Name == characterName);
            if (character == null)
            {
                Debug.LogError("Failed to load character!");
                return null;
            }

            var playerManager = character.Initialize(transform);

            playerManager.CharacterManager.InitializeCharacter(character, new CharacterStats());
            return playerManager;
        }
    }
}
