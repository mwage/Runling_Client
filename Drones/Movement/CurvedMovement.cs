using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class CurvedMovement : MonoBehaviour
    {
        private Rigidbody _rb;
        public float Curving;
        public float DroneSpeed;

        private void Start ()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rb.AddForce(_rb.transform.right * -Curving, ForceMode.Acceleration);
            if (Vector3.Cross(_rb.velocity, Vector3.up) != Vector3.zero)
            {
                _rb.transform.rotation = Quaternion.LookRotation(_rb.velocity, Vector3.up);
            }
            _rb.velocity = _rb.velocity.normalized * DroneSpeed;
        }
    }
}
