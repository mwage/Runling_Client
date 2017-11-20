using System;
using Drones.DroneTypes;
using Players;
using UnityEngine;

namespace Drones.Movement
{
    public class ChaserMovement : IDroneMovement
    {
        private readonly PlayerManager _target;

        public ChaserMovement(PlayerManager chaserTarget)
        {
            _target = chaserTarget;
        }

        public void Initialize(GameObject drone, float speed)
        {
            var anim = drone.transform.Find("Model")?.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetFloat(ADrone.SpeedHash, 3 + speed / 2);
            }

            drone.AddComponent<ChaserMovementImplementation>().Initialize(speed, _target);
        }
    }
}
