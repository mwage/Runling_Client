using Characters.Abilities;
using Characters.Repositories;
using Characters.Types.Features;
using Launcher;
using Players;
using UnityEngine;

namespace Characters.Types
{
    public abstract class ACharacter : MonoBehaviour
    {
        public int Exp { get; private set; }
        public int Level { get; private set; }
        public int Ability1Level { get; private set; }
        public int Ability2Level { get; private set; }
        public int UnspentPoints { get; private set; }
        public Energy Energy { get; private set; }
        public Speed Speed { get; private set; }

        public AAbility Ability1 { get; protected set; }
        public AAbility Ability2 { get; protected set; }
        protected CharacterDto Character { get; set; }

        private void Update()
        {
            if (Character == null)
                return;

            Energy.RefreshEnergy();
            RefreshCooldowns();
            if (Energy.IsExhausted)
            {
                DisableAllSkills();
            }
        }

        /// <summary>
        /// Initialize a RLR character from characterDto
        /// </summary>
        protected void InitializeFromDto()
        {
            Energy = new Energy(Character.EnergyPoints, Character.RegenPoints, 20, 5, 0.5f, 1);
            Speed = new Speed(Character.SpeedPoints);
            Exp = Character.Exp;
            Level = Character.Level;
            Ability1Level = Character.FirstAbilityLevel;
            Ability2Level = Character.SecondAbilityLevel;

            TESTMODE();
        }

        /// <summary>
        /// Initializes speed for a non-Dto character (Arena)
        /// </summary>
        protected void InitializeSpeed()
        {
            Speed = new Speed(10, 0);
        }

        private void TESTMODE()
        {
            Ability1Level = 1;
            Ability2Level = 1;
        }

        public abstract void Initialize(PlayerManager playerManager, CharacterDto character = null);

        public void AddExp(int exp)
        {
            Exp += exp;
            IncrementLevelIfPossible();
        }

        private void IncrementLevelIfPossible()
        {
            while (Exp >= LevelingSystem.LevelExperienceCurve[Level])
            {
                Level++;
                UnspentPoints += LevelingSystem.NewPointsPerLevel;
            }
        }

        public void IncrementSpeedPoints()
        {
            if (UnspentPoints > GetSpeedPropertyCost())
            {
                Speed.IncrementPoints();
                UnspentPoints -= GetSpeedPropertyCost();
            }
        }

        public void IncrementRegenPoints()
        {
            if (UnspentPoints > GetRegenPropertyCost())
            {
                Energy.IncreasePointsRegen();
                UnspentPoints -= GetRegenPropertyCost();
            }
        }

        public void IncrementEnergyPoints()
        {
            if (UnspentPoints > GetEnergyPropertyCost())
            {
                Energy.IncreasePointsEnergy();
                UnspentPoints -= GetEnergyPropertyCost();
            }
        }

        public void IncrementFirstAbilityLevel()
        {
            if (UnspentPoints > LevelingSystem.AbilityPointsCostPerLevel && Ability1Level < LevelingSystem.AbilityMaxLevel)
            {
                Ability1Level++;
                Ability1.IncrementLevel();
                UnspentPoints-= LevelingSystem.AbilityPointsCostPerLevel;
            }
        }

        public void IncrementSecondAbilityLevel()
        {
            if (UnspentPoints > LevelingSystem.AbilityPointsCostPerLevel && Ability2Level < LevelingSystem.AbilityMaxLevel)
            {
                Ability2Level++;
                Ability2.IncrementLevel();
                UnspentPoints -= LevelingSystem.AbilityPointsCostPerLevel;
            }
        }

        public void DisableAllSkills()
        {
            Ability1?.Disable();
            Ability2?.Disable();
        }

        private void ActivateOrDeactivateAbility(AAbility ability)
        {
            if (!ability.IsActive)
            {
                StartCoroutine(ability.Enable());
            }
            else
            {
                ability.Disable();
            }
        }

        public void InputAbilities()
        {
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.Ability1))
            {
                ActivateOrDeactivateAbility(Ability1);
            }
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.Ability2))
            {
                ActivateOrDeactivateAbility(Ability2);
            }
        }

        private void RefreshCooldowns()
        {
            Ability1?.RefreshCooldown();
            Ability2?.RefreshCooldown();
        }

        public int GetSpeedPropertyCost()
        {
            return GetPropertyCost(Speed.Points);
        }

        public int GetEnergyPropertyCost()
        {
            return GetPropertyCost(Energy.PointsEnergy);
        }

        public int GetRegenPropertyCost()
        {
            return GetPropertyCost(Energy.PointsRegen);
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
    }   
}