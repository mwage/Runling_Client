using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.SLA.test
{
    public class MovementTest2 : MonoBehaviour
    {
        Vector3 clickPos;
        Vector3 targetPos;
        Vector3 direction;
        float rotationSpeed;
        float maxSpeed;
        float currentSpeed;
        float highestSpeedReached;
        float distance;
        float acceleration;
        float deceleration;
        float stopSensitivity;
        bool accelerate;
        bool stop;
        Rigidbody rb;

        void Start()
        {
            rb = this.GetComponent<Rigidbody>();
            targetPos = transform.position;
            rotationSpeed = 15f;
            acceleration = 150f;
            deceleration = 100f;
            currentSpeed = 0;
            GameControl.MoveSpeed = 20;
            accelerate = false;
            stop = true;
            highestSpeedReached = 0;
            stopSensitivity = 20;
        }

        private void Update()
        {
            // On right mouseclick, set new target location
            if (!Input.GetMouseButtonDown(1)) return;

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit)) return;

            maxSpeed = GameControl.MoveSpeed;
            clickPos = hit.point;
            clickPos.y = transform.localScale.x/2;
            Debug.Log(clickPos.y);
            if (!((clickPos - transform.position).magnitude > 0.05f)) return;

            targetPos = clickPos;
            direction = (targetPos - transform.position).normalized;
            accelerate = true;
            stop = false;

            if ((targetPos - transform.position).magnitude < 0.5)
            {
                rb.velocity = direction * currentSpeed / 2;
            }
            else
            {
                rb.velocity = direction * currentSpeed;
            }

            highestSpeedReached = rb.velocity.magnitude;
        }


        private void FixedUpdate()
        {
            distance = (targetPos - transform.position).magnitude;
            currentSpeed = rb.velocity.magnitude;

            // Stop
            if (distance < highestSpeedReached / (10 * stopSensitivity) | distance < 0.02f)
            {
                rb.velocity = Vector3.zero;
                stop = true;
                accelerate = false;
                currentSpeed = 0f;
            }

            // Accelerate
            if (accelerate)
            {
                if (currentSpeed < maxSpeed)
                {
                    rb.AddForce(direction * acceleration, ForceMode.Acceleration);
                    currentSpeed = rb.velocity.magnitude;
                    highestSpeedReached = currentSpeed;
                }

                // Don't accelerate over maxSpeed
                else
                {
                    currentSpeed = maxSpeed;
                    rb.velocity = direction * currentSpeed;
                    accelerate = false;
                    highestSpeedReached = currentSpeed;
                }
            }

            // Decelerate
            if (distance < highestSpeedReached * highestSpeedReached / (2 * deceleration) && !stop)
            {
                rb.AddForce(direction * (-deceleration), ForceMode.Acceleration);
                accelerate = false;
            }

            if (currentSpeed != 0) { Rotate(); }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider.tag == "Wall")
            {
                transform.position = transform.position + collision.contacts[0].normal * 0.05f;
                if (collision.contacts[0].normal.x == 0)
                {
                    rb.velocity = new Vector3(1, 0, 0) * rb.velocity.x;
                    targetPos = transform.position + new Vector3(0.1f, 0, 0) * rb.velocity.x;
                    direction = (targetPos - transform.position).normalized;
                    highestSpeedReached = rb.velocity.magnitude;
                }
                else if (collision.contacts[0].normal.z == 0)
                {
                    rb.velocity = new Vector3(0, 0, 1) * rb.velocity.z;
                    targetPos = transform.position + new Vector3(0.1f, 0, 0) * rb.velocity.z;
                    direction = (targetPos - transform.position).normalized;
                    highestSpeedReached = rb.velocity.magnitude;
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    stop = true;
                    accelerate = false;
                }
            }
        }

        // Rotate Player
        void Rotate()
        {
            var lookrotation = targetPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), rotationSpeed * Time.deltaTime);
        }
    }
}