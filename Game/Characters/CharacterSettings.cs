using UnityEngine;

namespace Game.Scripts.Characters
{
    [CreateAssetMenu(fileName = "New Charactersettings", menuName = "Character/Character Settings")]
    public class CharacterSettings : ScriptableObject
    {
        // Speed
        public float BaseSpeed = 4;
        public float SpeedIncreasePerLevel = 0.05f;

        // Energy
        public int BaseEnergy = 20;
        public int EnergyPerPoint = 5;
        public float BaseRegen = 1;
        public float RegenPerPoint = 0.1f;
    }
}