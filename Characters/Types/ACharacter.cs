using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Characters.Types
{
    public abstract class ACharacter : MonoBehaviour
    {
        public int PlayerId { get; protected set; } 
        public int SpeedPoints { get; protected set; }
        public int RegenPoints { get; protected set; }
        public int EnergyPoints { get; protected set; }
        public int Exp { get; protected set; }
        public int Level { get; protected set; }
        public int AbilityFirstLevel { get; protected set; }
        public int AbilitySecondLevel { get; protected set; }
        public float EnergyCurrent { get; protected set; }
        public int EnergyMax { get; protected set; }
        public int BaseSpeed { get; protected set; }
        public int UnspentPoints { get; protected set; }
        public float Speed
        {
            get { return _baseSpeed + _speedPointRatio * SpeedPoints; }
        }

        protected float _baseSpeed, _speedPointRatio;
        protected float _regenPerSecondRatio;
        protected static GameObject _player;

        protected ACharacter(CharacterDto chacterDto)
        {
            SpeedPoints = chacterDto.SpeedPoints;
            RegenPoints = chacterDto.RegenPoints;
            EnergyPoints = chacterDto.EnergyPoints;
            Exp = chacterDto.Exp;
            Level = chacterDto.Level;
            AbilityFirstLevel = chacterDto.AbilityFirstLevel;
            AbilitySecondLevel = chacterDto.AbilitySecondLevel;
            EnergyCurrent = 0;
        }

        protected void InitiazlizeBase(CharacterDto chacterDto)
        {
            SpeedPoints = chacterDto.SpeedPoints;
            RegenPoints = chacterDto.RegenPoints;
            EnergyPoints = chacterDto.EnergyPoints;
            Exp = chacterDto.Exp;
            Level = chacterDto.Level;
            AbilityFirstLevel = chacterDto.AbilityFirstLevel;
            AbilitySecondLevel = chacterDto.AbilitySecondLevel;
            EnergyCurrent = 0;
        }

        public abstract void Initizalize(CharacterDto character);

        public virtual void AddExp(int exp)
        {
            Exp += exp;
            Debug.Log(string.Format("Added {0} exp", exp));

            IncrementLevelIfPossible();
        }

        public virtual void IncrementLevelIfPossible()
        {
            while (Exp > LevelingSystem.LevelExperienceCurve[Level])
            {
                Level++;
                Debug.Log(string.Format("you lvled to {0} lvl", Level));
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

        public void Update()
        {
            //RegenerateEnergy();
        }

        private void RegenerateEnergy()
        {
            if (EnergyCurrent >= EnergyMax)
            {
                EnergyCurrent = EnergyMax;
            }
            else
            {
                EnergyCurrent += _regenPerSecondRatio * Time.deltaTime;
            }
        }
    }

   
}


