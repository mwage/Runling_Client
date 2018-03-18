using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.Players;
using UnityEngine;

namespace Game.Scripts.Drones.Movement
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
