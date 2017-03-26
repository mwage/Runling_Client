using UnityEngine;
using System.Collections;

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
        rotationSpeed = 20f;
        rb = this.GetComponent<Rigidbody>();
        GameControl.moveSpeed = 10f;
        targetPos = transform.position;
        acceleration = 100f;
        deceleration = 100f;
        accelerate = true;
        stop = false;
        highestSpeedReached = 0;
        stopSensitivity = 20;
    }


    private void FixedUpdate()
    {
        // On right mouseclick, set new target location
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouseclick!");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                maxSpeed = GameControl.moveSpeed;
                clickPos = hit.point;
                clickPos.y = 0;
                if ((clickPos - transform.position).magnitude > 0.05f)
                {
                    targetPos = clickPos;
                    direction = (targetPos - transform.position).normalized;
                    accelerate = true;
                    stop = false;
                    if ((targetPos - transform.position).magnitude < 0.4)
                    {
                        rb.velocity = direction * currentSpeed / 2;
                    }
                    else
                    {
                        rb.velocity = direction * currentSpeed;
                    }
                }                
            }
        }

        distance = (targetPos - transform.position).magnitude;
        currentSpeed = rb.velocity.magnitude;

        Debug.Log("Distance: " + distance + ", current Speed: " + currentSpeed);

        // Stop
        if (distance < highestSpeedReached / (10 * stopSensitivity) | distance < 0.02f)
        {
            Debug.Log("Stop");
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
                Debug.Log("accelerating");
                if (distance < 0.5f)
                {
                    rb.AddForce(direction * acceleration / 3, ForceMode.Acceleration);
                    highestSpeedReached = rb.velocity.magnitude;
                }
                else
                {
                    rb.AddForce(direction * acceleration, ForceMode.Acceleration);
                    highestSpeedReached = currentSpeed;
                }
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
            Debug.Log("decelerate");
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
                targetPos = transform.position + new Vector3(1 / 2, 0, 0) * rb.velocity.x;
                direction = (targetPos - transform.position).normalized;
                highestSpeedReached = rb.velocity.magnitude;
            }
            else if (collision.contacts[0].normal.z == 0)
            {
                rb.velocity = new Vector3(0, 0, 1) * rb.velocity.z;
                targetPos = transform.position + new Vector3(1 / 2, 0, 0) * rb.velocity.z;
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
        Vector3 lookrotation = targetPos - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), rotationSpeed * Time.deltaTime);
    }
}