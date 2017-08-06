using System.IO;
using Characters.Types;
using Launcher;
using Players;
using UnityEngine;

namespace Characters
{
    public class PlayerFactory : MonoBehaviour
    {
        public GameObject ManticorePrefab;
        public GameObject UnicornPrefab;

        private PlayerFactory()
        {
        }

        public PlayerManager Create(CharacterDto character)
        {
            switch (character.Character)
            {
                case "Manticore":
                {
                    var playerManager = Instantiate(ManticorePrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<PlayerManager>();

                    playerManager.CharacterController = playerManager.gameObject.AddComponent<Manticore>();
                    playerManager.CharacterController.Initialize(character, playerManager);

                    return playerManager;
                }
                case "Unicorn":
                {
                    var playerManager = Instantiate(UnicornPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<PlayerManager>();

                    playerManager.CharacterController = playerManager.gameObject.AddComponent<Unicorn>();
                    playerManager.CharacterController.Initialize(character, playerManager);

                    return playerManager;
                    }
                case "Arena":
                {
                    var playerManager = Instantiate(ManticorePrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<PlayerManager>();

                    playerManager.CharacterController = playerManager.gameObject.AddComponent<ArenaCharacter>();
                    playerManager.CharacterController.Initialize(character, playerManager);

                    return playerManager;
                }
                default:
                {
                    Debug.Log("you want create non-existed character");
                    return null;
                }
            }
        }
    }
}
