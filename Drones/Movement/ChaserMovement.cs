using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class ChaserMovement : MonoBehaviour
    {
        public float Speed;
        public GameObject Player;

        private float _rotationSpeed;
        private Vector3 _targetPos;
        private Vector3 _direction;
        

        private void Start()
        {
        }


        private void FixedUpdate()
        {
        var rb = GetComponent<Rigidbody>();
        _rotationSpeed = 15f;
        const float acceleration = 50f;
        var maxSpeed = Speed;

            _targetPos = Player.transform.position;
            _targetPos.y += 0.6f;
            var currentSpeed = rb.velocity.magnitude;
            _direction = (_targetPos - transform.position).normalized;

            if ((_targetPos - transform.position).magnitude > 0.1f)
            {
                rb.velocity = _direction * currentSpeed;

                if (currentSpeed < maxSpeed)
                {
                    rb.AddForce(_direction * acceleration);
                }

                // Don't accelerate over maxSpeed
                else
                {
                    currentSpeed = maxSpeed;
                    rb.velocity = _direction * currentSpeed;
                }

                if (Mathf.Abs(currentSpeed) > 0.00001)
                {
                    Rotate(); 
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }

        // Rotate Player
        private void Rotate()
        {
            var lookrotation = _targetPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), _rotationSpeed * Time.deltaTime);
        }

    }
}
