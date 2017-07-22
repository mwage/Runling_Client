using System.Collections;
using System.Collections.Generic;
using Drones.DroneTypes;
using Drones.Pattern;
using UnityEngine;

namespace Characters.Types.Features
{
    public class Speed
    {
        public int Points { get; private set; }
        
        public float Current
        {
            get { return BaseSpeed + SpeedPointRatio * Points + _bonusSpeed; }
        }
        protected float BaseSpeed, SpeedPointRatio;
        private float _bonusSpeed;

        public Speed(float baseSpeed, float speedPointRatio)
        {
            BaseSpeed = baseSpeed;
            SpeedPointRatio = speedPointRatio;
            _bonusSpeed = 0F;
        }

        public void IncrementPoints()
        {
            Points++;
        }

        public IEnumerator AddBonusSpeed(float bonusSpeed, float workingTime)
        {
            _bonusSpeed += bonusSpeed;
            yield return new WaitForSeconds(workingTime);
            _bonusSpeed -= bonusSpeed;
        }

        public void ActivateBoost(float boostSpeed) // probably need also bool variable to verify if is actiave already
        {
            _bonusSpeed = boostSpeed;
        }

        public void DeactivateBoost(float boostSpeed)
        {
            _bonusSpeed = 0F;
        }

        public void SetBaseSpeed (float newSpeed)
        {
            BaseSpeed = newSpeed;
        }
    }
}