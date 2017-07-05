using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Characters.Types
{
    public abstract class ACharacter
    {
        public int Id { get; protected set; }
        public int SpeedPoints { get; protected set; }
        public int RegenPoints { get; protected set; }
        public int EnergyPoints { get; protected set; }
        public int Exp { get; protected set; }
        public int Level { get; protected set; }
        public int AbilityFirstLevel { get; protected set; }
        public int AbilitySecondLevel { get; protected set; }
        public int EnergyCurrent { get; protected set; }
        public int EnergyMax { get; protected set; }
        public int BaseSpeed { get; protected set; }
        public int UnspentPoints { get; protected set; }
        public float Speed
        {
            get { return _baseSpeed + _speedPointRatio * SpeedPoints; }
        }

        private float _baseSpeed, _speedPointRatio;
        private float _regenPerSecondRatio;

        public ACharacter(CharacterDto chacterDto)
        {
            Id = chacterDto.Id;
            SpeedPoints = chacterDto.SpeedPoints;
            RegenPoints = chacterDto.RegenPoints;
            EnergyPoints = chacterDto.EnergyPoints;
            Exp = chacterDto.Exp;
            Level = chacterDto.Level;
            AbilityFirstLevel = chacterDto.AbilityFirstLevel;
            AbilitySecondLevel = chacterDto.AbilitySecondLevel;
            EnergyCurrent = 0;
        }

        public virtual void IncrementLevelIfPossible()
        {
            while (Exp > LevelingSystem.LevelExperienceCurve[Level])
            {
                Level++;
                UnspentPoints += LevelingSystem.NewPointsPerLevel;
            }
        }

        public virtual void IncrementSpeedPoints()
        {
            if (UnspentPoints > 0)
            {
                SpeedPoints++;
                UnspentPoints--;
            }
        }

        public virtual void IncrementRegenPoints()
        {
            if (UnspentPoints > 0)
            {
                RegenPoints++;
                UnspentPoints--;
            }
        }

        public virtual void IncrementEnergyPoints()
        {
            if (UnspentPoints > 0)
            {
                EnergyPoints++;
                UnspentPoints--;
            }
        }

        public virtual void IncrementAbilityFirstLevel()
        {
            if (UnspentPoints > LevelingSystem.AbilityPointsCostPerLevel && AbilityFirstLevel < LevelingSystem.AbilityMaxLevel)
            {
                AbilityFirstLevel++;
                UnspentPoints-= LevelingSystem.AbilityPointsCostPerLevel;
            }
        }

        public virtual void IncrementAbilitySecondLevel()
        {
            if (UnspentPoints > LevelingSystem.AbilityPointsCostPerLevel && AbilitySecondLevel < LevelingSystem.AbilityMaxLevel)
            {
                AbilitySecondLevel++;
                UnspentPoints -= LevelingSystem.AbilityPointsCostPerLevel;
            }
        }

    }

   
}


