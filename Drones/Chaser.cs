using UnityEngine;
using System.Collections;

public class Chaser : MonoBehaviour
{
    Vector3 targetPos;
    Vector3 direction;
    float rotationSpeed;
    float maxSpeed;
    float currentSpeed;
    float acceleration;
    Rigidbody rb;

    public void ChaserDrone(GameObject chaserDrone, GameObject player, float speed, float size, Color color)
    {
        rb = chaserDrone.GetComponent<Rigidbody>();

        rotationSpeed = 15f;
        acceleration = 500f;
        currentSpeed = 0;
        maxSpeed = speed;

        // adjust drone color and size
        Renderer rend;
        Vector3 scale;
        rend = chaserDrone.GetComponent<Renderer>();
        rend.material.color = color;
        scale = chaserDrone.transform.localScale;
        scale.x *= size;
        scale.z *= size;
        chaserDrone.transform.localScale = scale;

        StartCoroutine(IChaser(chaserDrone, player, speed));
    }

    IEnumerator IChaser(GameObject chaserDrone, GameObject player, float speed)
    {
        do
        {
            targetPos = player.transform.position;
            direction = (targetPos - chaserDrone.transform.position).normalized;
            rb.velocity = direction * currentSpeed;

            if (currentSpeed < maxSpeed)
            {
                rb.AddForce(direction * acceleration, ForceMode.Acceleration);
                currentSpeed = rb.velocity.magnitude;
            }

            // Don't accelerate over maxSpeed
            else
            {
                currentSpeed = maxSpeed;
                rb.velocity = direction * currentSpeed;
            }

            if (currentSpeed != 0) { Rotate(); }

            yield return new WaitForSeconds(0.02f);
        } while (!GameControl.dead);

        rb.velocity = Vector3.zero;
        currentSpeed = 0f;
        chaserDrone.SetActive(false);
    }

    // Rotate Player
    void Rotate()
    {
        Vector3 lookrotation = targetPos - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), rotationSpeed * Time.deltaTime);
    }
}