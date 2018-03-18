using UnityEngine;

namespace Game.Scripts.Characters.Features
{
    public class Energy
    {
        private readonly CharacterStats _stats;

        public float Current { get; private set; }
        public int Max => _baseEnergy + _energyPointRatio * _stats.EnergyPoints;

        private readonly int _baseEnergy;
        private readonly int _energyPointRatio;
        private readonly float _regenPointRatio;
        private readonly float _baseRegen;
        private float Regen => _baseRegen + _stats.RegenPoints * _regenPointRatio;

        public RegenStatus RegenStatus { get; private set; } = RegenStatus.Regen;
        private float _energyDrain;
        public bool IsExhausted { get; private set; }

        public Energy(CharacterStats stats, CharacterSettings settings)
        {
            _stats = stats;
            _baseEnergy = settings.BaseEnergy;
            _energyPointRatio = settings.EnergyPerPoint;
            _baseRegen = settings.BaseRegen;
            _regenPointRatio = settings.RegenPerPoint;
            Current = Max;
        }

        public void RefreshEnergy()
        {
            switch (RegenStatus)
            {
                case RegenStatus.Blocked:
                {
                    return;
                }
                case RegenStatus.Regen:
                {
                    IsExhausted = false;
                    if (Current >= Max)
                    {
                        Current = Max;
                        return;
                    }
                    Current += Regen * Time.deltaTime;
                    return;
                }
                case RegenStatus.Drain:
                {
                    if (Current <= 0F)
                    {
                        IsExhausted = true;
                        Current = 0F;
                        RegenStatus = RegenStatus.Regen;

                    }
                    Current -= _energyDrain * Time.deltaTime;
                    return;
                }
            }
        }

        public void RefillEnergy()
        {
            Current = Max;
        }

        public bool UseEnergy(int value)
        {
            if (value > Current)
                return false;

            Current -= value;
            return true;
        }

        public void EnableEnergyDrain(float drainPerSecond)
        {
            _energyDrain = drainPerSecond;
            RegenStatus = RegenStatus.Drain;
        }

        public void DisableEnergyDrain()
        {
            _energyDrain = 0;
            RegenStatus = RegenStatus.Regen;
        }

        public void BlockRegen()
        {
            RegenStatus = RegenStatus.Blocked;
        }

        public void ContinueRegen()
        {
            RegenStatus = RegenStatus.Regen;
        }
    }
}