using System.Collections;
using Characters.Types;
using Players;
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
        public bool IsUsable { get; protected set; }
        public bool IsActive { get; protected set; }

        protected ACharacter Character { get; set; }
        protected PlayerManager PlayerManager { get; set; }

        public abstract IEnumerator Enable();
        public abstract void Disable();


        public void UpdateLevel(int currentAbilityLevel)
        {
            Level = currentAbilityLevel;
        }

        public void RefreshCooldown()
        {
            if (IsUsable)
                return;

            if (TimeToRenew <= 0)
            {
                IsUsable = true;
                TimeToRenew = 0;
            }
            else
            {
                TimeToRenew -= Time.deltaTime;
            }
        }

        protected void SetLoaded()
        {
            TimeToRenew = 0F;
            IsUsable = true;
        }

        public void IncrementLevel()
        {
            Level++;
        }

        public float GetLoadingProgress()
        {
            if (Level == 0)
                return 0;

            if (IsUsable)
                return 1;

            return 1 - TimeToRenew / Cooldown;
        }

    }
}
