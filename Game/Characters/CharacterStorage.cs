using UnityEngine;

namespace Game.Scripts.Characters
{
    [CreateAssetMenu(fileName = "New Characterstorage", menuName = "Character/Storage")]
    public class CharacterStorage : ScriptableObject
    {
        public Character[] AvailableCharacters;
    }
}