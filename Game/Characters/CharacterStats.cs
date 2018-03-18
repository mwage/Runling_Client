using Game.Scripts.Characters.CharacterRepositories;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Characters
{
    public class CharacterStats
    {
        public int Exp { get; private set; }
        public int Level { get; private set; } = 1;
        public int UnspentPoints { get; private set; }
        public int SpeedPoints => _speedPoints;
        public int EnergyPoints => _energyPoints;
        public int RegenPoints => _regenPoints;
        public int[] AbilityLevels { get; }

        private int _speedPoints;
        private int _energyPoints;
        private int _regenPoints;

        private readonly CharacterDto _characterDto;
        private readonly ICharacterRepository _repository;

        /// <summary>
        /// Create Character with default stats and no skills/statpoints
        /// </summary>
        public CharacterStats()
        {
        }

        /// <summary>
        /// Create character from Dto with skills, stats and a repository to save progress to
        /// </summary>
        public CharacterStats(CharacterDto characterDto, ICharacterRepository repository)
        {
            _characterDto = characterDto;
            _repository = repository;

            // XP
            Exp = characterDto.Exp;
            Level = characterDto.Level;
            AbilityLevels = characterDto.AbilityLevels;

            // Stat points
            _speedPoints = characterDto.SpeedPoints;
            _energyPoints = characterDto.EnergyPoints;
            _regenPoints = characterDto.RegenPoints;
            UnspentPoints = CalcUnspentPoints();
        }

        private int CalcUnspentPoints()
        {
            return Level * LevelingSystem.NewPointsPerLevel - _speedPoints - _energyPoints - _regenPoints - LevelingSystem.AbilityPointsCostPerLevel * AbilityLevels.Sum();
        }

        #region Increment Stats

        public void AddExp(int exp)
        {
            Exp += exp;
            IncrementLevel();
        }

        private void IncrementLevel()
        {
            while (Exp >= LevelingSystem.LevelExperienceCurve[Level])
            {
                Level++;
                UnspentPoints = CalcUnspentPoints();
            }
            UpdateRepository();
        }

        public void IncrementSpeedPoints()
        {
            IncrementStat(ref _speedPoints);
        }

        public void IncrementEnergyPoints()
        {
          IncrementStat(ref _energyPoints);
        }

        public void IncrementRegenPoints()
        {
            IncrementStat(ref _regenPoints);
        }

        private void IncrementStat(ref int statPoints)
        {
            var upgradeCost = GetPropertyCost(statPoints);
            if (UnspentPoints >= upgradeCost)
            {
                statPoints++;
                UnspentPoints = CalcUnspentPoints();
                UpdateRepository();
            }
            else
            {
                Debug.Log("Not enough points available!");
            }
        }

        public void IncrementAbilityLevel(int id)
        {
            if (UnspentPoints >= LevelingSystem.AbilityPointsCostPerLevel && AbilityLevels[id] < LevelingSystem.AbilityMaxLevel)
            {
                if (Level < 5 * AbilityLevels[id])
                {
                    Debug.Log("Level not high enough!");
                    return;
                }

                AbilityLevels[id]++;
                UnspentPoints = CalcUnspentPoints();
                UpdateRepository();
            }
            else
            {
                Debug.Log("Not enough points available!");
            }
        }

        private static int GetPropertyCost(int propertyPoints)
        {
            for (var i = 0; i < LevelingSystem.PropertyCostModifierLevels.Length; i++)
            {
                if (propertyPoints < LevelingSystem.PropertyCostModifierLevels[i])
                {
                    return i + 1;
                }
            }
            return LevelingSystem.PropertyCostModifierLevels.Length + 1;
        }

        #endregion

        private void UpdateRepository()
        {
            _repository.UpdateRepository(
                new CharacterDto(_characterDto.Id, _characterDto.Name, _speedPoints, _regenPoints, _energyPoints, 
                Exp, Level, AbilityLevels, _characterDto.Occupied));
        }
    }
}