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

        public Energy(int pointsEnergy, int pointsRegen, int baseEnergy, int energyPointRatio, float regenPerSecondRatio, float baseRegen)
        {
            PointsEnergy = pointsEnergy;
            PointsRegen = pointsRegen;
            _baseEnergy = baseEnergy;
            _energyPointRatio = energyPointRatio;
            _regenPerSecondRatio = regenPerSecondRatio;
            _baseRegen = baseRegen;
            Current = 0; // change to max later
        }

        public void IncreasePointsEnergy()
        {
            PointsEnergy++;
        }

        public void IncreasePointsRegen()
        {
            PointsRegen++;
        }

        public void RegenerateEnergy()
        {
            if (Current >= Max)
            {
                Current = Max;
                return;
            }
            Current += _regen * Time.deltaTime;
        }

        public void RefillEnergy()
        {
            Current = Max;
        }




    }
}