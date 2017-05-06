using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class CurvedMovement : MonoBehaviour
    {
        private Rigidbody _rb;
        public float Force;
        public float Curving;
        public float DroneSpeed;

        private void Start ()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rb.AddForce(_rb.transform.right * Force, ForceMode.Acceleration);
            _rb.transform.Rotate(new Vector3(0F, Curving, 0F)*Time.deltaTime);
            _rb.velocity = _rb.velocity.normalized * DroneSpeed; // don't accelerate too much. Can add if (currentMaginitude > 1.5 startDroneSpeed) then {normalize} - to make first lanes harder
        }
    }
}
