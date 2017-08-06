using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Characters.Types;
using UnityEngine;

namespace Characters.Abilities
{
    public abstract class AAbility
    {
        public string Name { get; protected set; }
        public int Level { get; protected set; }
        public abstract int Cooldown { get; }
        public float TimeToRenew { get; protected set; }
        public abstract int EnergyCost { get; }
        public float EnergyDrainPerSecond { get; protected set; }
        public bool IsLoaded;

        public bool IsActive;

        public abstract IEnumerator Enable(ACharacter character);
        public abstract void Disable(ACharacter character);


        public virtual void UpdateLevel(int currentAbilityLevel)
        {
            Level = currentAbilityLevel;
        }

        public virtual void RefreshCooldown()
        {
            if (IsLoaded) return;
            if (IsActive) return; // no loading while active
            if (TimeToRenew <= 0)
            {
                IsLoaded = true;
                TimeToRenew = 0F;
            }
            else
            {
                TimeToRenew -= Time.deltaTime;
            }

        }

        public void SetLoaded()
        {
            TimeToRenew = 0F;
            IsLoaded = true;
        }

        public virtual void IncrementLevel()
        {
            Level++;
        }

        public virtual float GetLoadingProgress()
        {
            if (Level == 0) return 0F;
            if (IsLoaded) return 1F;
            return 1 - TimeToRenew / Cooldown;
        }

    }
}
