using Characters.Repositories;
using Characters.Types;
using Players;
using UnityEngine;

namespace Characters
{
    public class PlayerFactory : MonoBehaviour
    {
        public GameObject ManticorePrefab;
        public GameObject CatPrefab;
        
        /// <summary>
        /// Create a character by CharacterDto with it's Abilities (f.e. RLR character).
        /// </summary>
        public PlayerManager Create(CharacterDto character)
        {
            var playerManager = InstantiateCharacter(character.Name);
            playerManager?.CharacterController.Initialize(playerManager, character);
            return playerManager;
        }

        /// <summary>
        /// Create a character by name without any skills/energy (f.e. Arena character)
        /// </summary>
        public PlayerManager Create(string characterName)
        {
            var playerManager = InstantiateCharacter(characterName);
            playerManager?.CharacterController.Initialize(playerManager);
            return playerManager;
        }

        private PlayerManager InstantiateCharacter(string characterName)
        {
            switch (characterName)
            {
                case "Manticore":
                    return InstantiateManticore();
                case "Cat":
                    return InstantiateCat();
                default:
                    Debug.LogError("You tried to create non-existed character");
                    return null;
            }
        }

        private PlayerManager InstantiateCat()
        {
            var playerManager = Instantiate(CatPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<PlayerManager>();
            playerManager.CharacterController = playerManager.gameObject.AddComponent<Cat>();
            return playerManager;
        }

        private PlayerManager InstantiateManticore()
        {
            var playerManager = Instantiate(ManticorePrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<PlayerManager>();
            playerManager.CharacterController = playerManager.gameObject.AddComponent<Manticore>();
            return playerManager;
        }
    }
}
