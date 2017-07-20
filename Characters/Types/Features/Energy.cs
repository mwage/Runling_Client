using System.Collections;
using UnityEngine;

namespace Characters.Types.Features
{
    public class Energy
    {
        public float Current { get; private set; }
        public int PointsEnergy { get; private set; }
        public int Max { get { return _baseEnergy + _energyPointRatio * PointsEnergy; } }
        private readonly int _baseEnergy;
        private readonly int _energyPointRatio;

        public int PointsRegen { get; private set; }
        private float _regen { get { return _baseRegen + PointsRegen * _regenPerSecondRatio; } }
        private readonly float _regenPerSecondRatio;
        private readonly float _baseRegen;

        public RegenStatus RegenStatus;
        public float EnergyDrainPerSec;
        public bool IsExhausted;

        public Energy(int pointsEnergy, int pointsRegen, int baseEnergy, int energyPointRatio, float regenPerSecondRatio, float baseRegen)
        {
            PointsEnergy = pointsEnergy;
            PointsRegen = pointsRegen;
            _baseEnergy = baseEnergy;
            _energyPointRatio = energyPointRatio;
            _regenPerSecondRatio = regenPerSecondRatio;
            _baseRegen = baseRegen;
            Current = 0; // change to max later
            RegenStatus = RegenStatus.Regen;
            IsExhausted = false;
        }

        public void IncreasePointsEnergy()
        {
            PointsEnergy++;
        }

        public void IncreasePointsRegen()
        {
            PointsRegen++;
        }

        public void RefreshEnergy()
        {
            // returns 
            switch (RegenStatus)
            {
                case (RegenStatus.Blocked):
                {
                    return;
                }
                case (RegenStatus.Regen):
                {
                    IsExhausted = false;
                    if (Current >= Max)
                    {
                        Current = Max;
                        return;
                    }
                    Current += _regen * Time.deltaTime;
                    return;
                }
                case (RegenStatus.Drain):
                {
                    if (Current <= 0F)
                    {
                        IsExhausted = true;
                        Current = 0F;
                        RegenStatus = RegenStatus.Regen;

                    }
                    Current -= EnergyDrainPerSec * Time.deltaTime;
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
            if (value > Current) return false;
            Current -= value;
            return true;
        }

        




    }

    public enum RegenStatus
    {
        Regen,
        Blocked,
        Drain
    }
}