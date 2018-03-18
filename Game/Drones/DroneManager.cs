using Game.Scripts.Drones.Movement;
using UnityEngine;

namespace Game.Scripts.Drones
{
    public class DroneManager : MonoBehaviour
    {
        public bool IsFrozen { get; private set; }
        public float TimeToUnfreeze { get; private set; }

        public bool IsSlowed { get; private set; }

        private ADroneMovementImplementation _droneMovement;

        private void Awake()
        {
            //droneRb = gameObject.transform.GetComponent<Rigidbody>();
            TimeToUnfreeze = 0;
        }

        private void Initialize()
        {
            if (_droneMovement != null)
                return;

            _droneMovement = gameObject.GetComponent<ADroneMovementImplementation>();
        }

        private void Update()
        {
            if (_droneMovement != null)
            {
                _droneMovement.Move();
            }
            else
            {
                _droneMovement = gameObject.GetComponent<ADroneMovementImplementation>();
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