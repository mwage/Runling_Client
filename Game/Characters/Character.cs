using Game.Scripts.Characters.Abilities;
using Game.Scripts.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Characters
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Character/New Character")]
    public class Character : ScriptableObject
    {
        public string Name = "Character Name";
        public string Description = "Description";
        public Sprite PreviewImage;
        public GameObject CharacterPrefab;
        public CharacterSettings Settings;
        public AAbility[] Abilities;

        public PlayerManager Initialize(Transform parent)
        {
            var playerManager = Instantiate(CharacterPrefab, parent).GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("No PlayerManager instance attached to the prefab of " + Name);
                return null;
            }

            playerManager.CharacterManager = playerManager.gameObject.AddComponent<CharacterManager>();

            foreach (var ability in Abilities)
            {
                ability.Initialize(playerManager);
            }
            return playerManager;
        }
    }
}