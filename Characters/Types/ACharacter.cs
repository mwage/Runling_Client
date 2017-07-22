using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Characters.Abilities;
using Characters.Types.Features;
using Launcher;
using UnityEngine;

namespace Characters.Types
{
    public abstract class ACharacter : MonoBehaviour
    {
        public int PlayerId { get; protected set; } 
        public int Exp { get; protected set; }
        public int Level { get; protected set; }
        public int Ability1Level { get; protected set; }
        public int Ability2Level { get; protected set; }
        public int UnspentPoints { get; protected set; }
        public Energy Energy;
        public Speed Speed;

        public AAbility Ability1 { get; protected set; }
        public AAbility Ability2 { get; protected set; }

        protected ACharacter(CharacterDto chacterDto) 
        {
        }

        public void Update()
        {
            Energy.RefreshEnergy();
            RefreshCooldowns();
            if (Energy.IsExhausted)
            {
                DisableAllSkills();
            }
        }

        protected void InitializeBase(CharacterDto chacterDto)
        {
            Energy = new Energy(chacterDto.EnergyPoints, chacterDto.RegenPoints, 20, 5, 0.5F, 1F);
            Speed = new Speed(10F, 0.05F);
            Exp = chacterDto.Exp;
            Level = chacterDto.Level;
            Ability1Level = chacterDto.AbilityFirstLevel;
            Ability2Level = chacterDto.AbilitySecondLevel;

            TESTMODE();
        }

        protected void TESTMODE()
        {
            Ability1Level = 1;
            Ability2Level = 1;
        }

        public abstract void Initialize(CharacterDto character);

        public virtual void AddExp(int exp)
        {
            Exp += exp;
            //Debug.Log(string.Format("Added {0} exp", exp));
            IncrementLevelIfPossible();
        }

        public virtual void IncrementLevelIfPossible()
        {
            while (Exp >= LevelingSystem.LevelExperienceCurve[Level])
            {
                Level++;
                UnspentPoints += LevelingSystem.NewPointsPerLevel;
            }
        }

        public virtual void IncrementSpeedPoints()
        {
            if (UnspentPoints < GetSpeedPropertyCost()) return;
            Speed.IncrementPoints();
            UnspentPoints -= GetSpeedPropertyCost();
        }

        public virtual void IncrementRegenPoints()
        {
            if (UnspentPoints < GetRegenPropertyCost()) return;
            Energy.IncreasePointsRegen();
            UnspentPoints -= GetRegenPropertyCost();
        }

        public virtual void IncrementEnergyPoints()
        {
            if (UnspentPoints < GetEnergyPropertyCost()) return;
            Energy.IncreasePointsEnergy();
            UnspentPoints -= GetEnergyPropertyCost();
        }

        public virtual void IncrementAbilityFirstLevel()
        {
            if (UnspentPoints > LevelingSystem.AbilityPointsCostPerLevel && Ability1Level < LevelingSystem.AbilityMaxLevel)
            {
                Ability1Level++;
                UnspentPoints-= LevelingSystem.AbilityPointsCostPerLevel;
            }
        }

        public virtual void IncrementAbilitySecondLevel()
        {
            if (UnspentPoints > LevelingSystem.AbilityPointsCostPerLevel && Ability2Level < LevelingSystem.AbilityMaxLevel)
            {
                Ability2Level++;
                UnspentPoints -= LevelingSystem.AbilityPointsCostPerLevel;
            }
        }

        public virtual bool UseEnergy(int value)
        {
            return Energy.UseEnergy(value);
        }

        public void DisableAllSkills()
        {
            Ability1.Disable(this);
            Ability2.Disable(this);
        }

        protected virtual void ActivateOrDeactivateAbility1()
        {
            if (!Ability1.IsActive)
            {
                StartCoroutine(Ability1.Enable(this));
            }
            else
            {
                Ability1.Disable(this);
            }
            
        }

        protected virtual void ActivateOrDeactivateAbility2()
        {
            if (!Ability2.IsActive)
            {
                StartCoroutine(Ability2.Enable(this));
            }
            else
            {
                Ability2.Disable(this);
            }
            
        }

        public virtual void InputAbilities()
        {
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.Ability1))
            {
                ActivateOrDeactivateAbility1();
            }
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.Ability2))
            {
                ActivateOrDeactivateAbility2();
            }
        }

        public virtual void RefreshCooldowns()
        {
            if (Ability1 != null)
                Ability1.RefreshCooldown();
            if (Ability2 != null)
                Ability2.RefreshCooldown();
        }

        public virtual int GetSpeedPropertyCost()
        {
            return GetPropertyCost(Speed.Points);
        }

        public virtual int GetEnergyPropertyCost()
        {
            return GetPropertyCost(Energy.PointsEnergy);
        }

        public virtual int GetRegenPropertyCost()
        {
            return GetPropertyCost(Energy.PointsRegen);
        }

        protected virtual int GetPropertyCost(int propertyPoints)
        {
            for (int i = 0; i < LevelingSystem.PropertyCostModifierLevels.Length; i++)
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


