﻿using Drones.Movement;
using UnityEngine;

namespace Drones
{
    public class DroneManager : MonoBehaviour
    {
        public bool IsFrozen;
        public float TimeToUnfreeze;

        public bool IsSlowed;

        private ADroneMovement _droneMovement;

        private void Awake()
        {
            //droneRb = gameObject.transform.GetComponent<Rigidbody>();
            TimeToUnfreeze = 0F;
        }

        private void Initialize()
        {
            if (_droneMovement != null) return;
            _droneMovement = gameObject.GetComponent<ADroneMovement>();
        }

        private void Update()
        {
            if(_droneMovement != null)
                _droneMovement.Move();
            else
            {
                _droneMovement = gameObject.GetComponent<ADroneMovement>();
                return;
            }
            if (IsFrozen)
            {
                TimeToUnfreeze -= Time.deltaTime;
                TryUnfreeze();
            }
        }

        public void Freeze(float durationTime)
        {
            
            if (!IsFrozen)
            {
                IsFrozen = true;
                Initialize();
                _droneMovement.Freeze();
            }
            TimeToUnfreeze = TimeToUnfreeze < durationTime ? durationTime : TimeToUnfreeze; // for many freezes in MP
        }

        public void EnableSlowdown(float slowPercentage)
        {
            Debug.Log("slowing");
            IsSlowed = true;
            _droneMovement.SlowDown(slowPercentage);
        }

        public void DisableSlowdown()
        {
            Debug.Log("Un slowing");
            IsSlowed = false;
            _droneMovement.UnSlowDown();
        }

        private void TryUnfreeze()
        {
            if (TimeToUnfreeze <= 0)
            {
                Unfreeze();
            }
        }

        private void Unfreeze()
        {
            IsFrozen = false;
            _droneMovement.UnFreeze();
        }
    }
}