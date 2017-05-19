using UnityEngine;

namespace Drones.Movement
{

    public class RotateBouncingDrone : MonoBehaviour
    {
        public float DroneSpeed;
        private float _rotation;

        private void Start()
        {
            _rotation = Random.Range(-1, 1) > 0 ? DroneSpeed / 20 : - DroneSpeed / 20;
        }

        private void FixedUpdate()
        {
            transform.Rotate(0, _rotation, 0);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 19)
            {
                _rotation = -_rotation;
            }
        }
    }
}