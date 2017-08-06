using Players;
using UnityEngine;

namespace Drones.Movement
{
    public class DroneMovement
    {
        public delegate void MovementDelegate(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null, PlayerManager chaserTarget = null);

        private static readonly int SpeedHash = Animator.StringToHash("DroneSpeed");
        private static Animator _anim;
        private static float _rotationSpeed;

        public static void Move(GameObject drone, float droneSpeed, MovementDelegate moveDelegate = null,
            float? curving = null, float? sinForce = null, float? sinFrequency = null, PlayerManager chaserTarget = null)
        {
            var model = drone.transform.Find("Model");
            if (model != null)
            {
                _anim = model.GetComponent<Animator>();
            }
            _rotationSpeed = 3 + droneSpeed / 2;
            if (moveDelegate == null)
            {
                MoveStraight(drone, droneSpeed);
                var instance = drone.AddComponent<StraightBouncingMovement>();
                //instance.DroneSpeed = droneSpeed;
            }
            else
            {
                moveDelegate(drone, droneSpeed, curving, sinForce, sinFrequency, chaserTarget);
                if (drone.GetComponent<PointToPointMovement>() != null)
                {
                    Object.Destroy(_anim);
                }
            }
        }

        //move drones in a straight line
        public static void MoveStraight(GameObject drone, float droneSpeed)
        {
            if (_anim != null)
            {
                _anim.SetFloat(SpeedHash, _rotationSpeed);
            }
            var rb = drone.GetComponent<Rigidbody>();
            rb.AddForce(drone.transform.forward * droneSpeed, ForceMode.VelocityChange);
            
        }

        public static void ChaserMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null, PlayerManager chaserTarget = null)
        {
            if (_anim != null)
            {
                _anim.SetFloat(SpeedHash, _rotationSpeed);
            }

            var instance = drone.AddComponent<ChaserMovement>();
            instance.Speed = droneSpeed;
            instance.ChaserTarget = chaserTarget;
        }

        public static void CurvedMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null, PlayerManager chaserTarget = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<CurvedMovement>();
            instance.Curving = curving ?? 3;
            instance.DroneSpeed = droneSpeed;
            instance.CurvingDuration = 4;
        }

        public static void CircleMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null, PlayerManager chaserTarget = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<CurvedMovement>();
            instance.Curving = curving ?? 1;
            instance.DroneSpeed = droneSpeed;
            instance.CurvingDuration = null;
        }

        public static void SinusoidalMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null, PlayerManager chaserTarget = null)
        {
            Object.Destroy(_anim);
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<SinusoidalMovement>();
            instance.SinFrequency = sinFrequency ?? 5;
            instance.SinForce = sinForce ?? 40;
            instance.DroneSpeed = droneSpeed;
            instance.Initialized = true;
        }

        public static void FixedSinusoidalMovement(GameObject drone, float droneSpeed, float? curving = null,
            float? sinForce = null, float? sinFrequency = null, PlayerManager chaserTarget = null)
        {
            Object.Destroy(_anim);
            SinusoidalMovement(drone, droneSpeed, sinForce: sinForce, sinFrequency: sinFrequency);
            drone.GetComponent<SinusoidalMovement>().Fixed = true;
        }
    }
}
