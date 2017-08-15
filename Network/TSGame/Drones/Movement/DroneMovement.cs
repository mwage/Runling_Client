using MP.TSGame.Players;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Movement
{
    public class DroneMovement
    {
        public delegate void MovementDelegate(GameObject drone, FP droneSpeed, FP? curving = null,
            FP? sinForce = null, FP? sinFrequency = null, GameObject player = null);

        private static readonly int SpeedHash = Animator.StringToHash("DroneSpeed");
        private static Animator _anim;
        private static float _rotationSpeed;

        public static void Move(GameObject drone, FP droneSpeed, MovementDelegate moveDelegate = null,
            FP? curving = null, FP? sinForce = null, FP? sinFrequency = null, GameObject player = null)
        {
            var model = drone.transform.Find("Model");
            if (model != null)
            {
                _anim = model.GetComponent<Animator>();
            }
            _rotationSpeed = 3 + (float)droneSpeed / 2;
            if (moveDelegate == null)
            {
                MoveStraight(drone, droneSpeed);
            }
            else
            {
                moveDelegate(drone, droneSpeed, curving, sinForce, sinFrequency, player);
                if (drone.GetComponent<PointToPointMovement>() != null)
                {
                    Object.Destroy(_anim);
                }
            }
        }

        //move drones in a straight line
        public static void MoveStraight(GameObject drone, FP droneSpeed)
        {
            if (_anim != null)
            {
                _anim.SetFloat(SpeedHash, _rotationSpeed);
            }
            var rb = drone.GetComponent<TSRigidBody>();
;
            rb.AddForce(rb.tsTransform.forward * droneSpeed, ForceMode.Impulse);
        }

//          TODO: rework chaser movement
        public static void ChaserMovement(GameObject drone, FP droneSpeed, FP? curving = null,
            FP? sinForce = null, FP? sinFrequency = null, GameObject player = null)
        {
            if (_anim != null)
            {
                _anim.SetFloat(SpeedHash, _rotationSpeed);
            }

            var instance = drone.AddComponent<ChaserMovement>();
            instance.Speed = droneSpeed;
            if (player != null)
                instance.Target = player.GetComponent<PlayerManager>();
        }

        public static void CurvedMovement(GameObject drone, FP droneSpeed, FP? curving = null,
            FP? sinForce = null, FP? sinFrequency = null, GameObject player = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<CurvedMovement>();
            instance.Curving = curving ?? 3;
            instance.DroneSpeed = droneSpeed;
            instance.CurvingDuration = 4;
        }

        public static void CircleMovement(GameObject drone, FP droneSpeed, FP? curving = null,
            FP? sinForce = null, FP? sinFrequency = null, GameObject player = null)
        {
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<CurvedMovement>();
            instance.Curving = curving ?? 1;
            instance.DroneSpeed = droneSpeed;
            instance.CurvingDuration = null;
        }

        public static void SinusoidalMovement(GameObject drone, FP droneSpeed, FP? curving = null,
            FP? sinForce = null, FP? sinFrequency = null, GameObject player = null)
        {
            Object.Destroy(_anim);
            MoveStraight(drone, droneSpeed);
            var instance = drone.AddComponent<SinusoidalMovement>();
            instance.SinFrequency = sinFrequency ?? 5;
            instance.SinForce = sinForce ?? 40;
            instance.DroneSpeed = droneSpeed;
        }

        public static void FixedSinusoidalMovement(GameObject drone, FP droneSpeed, FP? curving = null,
            FP? sinForce = null, FP? sinFrequency = null, GameObject player = null)
        {
            Object.Destroy(_anim);
            SinusoidalMovement(drone, droneSpeed, sinForce: sinForce, sinFrequency: sinFrequency);
            drone.GetComponent<SinusoidalMovement>().Fixed = true;
        }
    }
}
