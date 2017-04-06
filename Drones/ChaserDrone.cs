using System.Collections;
using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class ChaserDrone : ADrone
    {
        protected readonly GameObject Player;

        private Vector3 targetPos;
        private Vector3 _direction;

        private GameObject _chaser;
        float rotationSpeed = 15f;


        public ChaserDrone(float speed, float size, Color color, GameObject player) : base(speed, size, color)
        {
            Player = player;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded)
        {
            _chaser = Object.Instantiate(factory.FlyingOnewayDrone, new Vector3(0, 0.6f, 0), Quaternion.identity);






            // adjust drone color and size
            var rend = _chaser.GetComponent<Renderer>();
            rend.material.color = Color;
            var scale = _chaser.transform.localScale;
            scale.x *= Size;
            scale.z *= Size;
            _chaser.transform.localScale = scale;

            factory.StartCoroutine(IChaser(_chaser));
            return _chaser;
        }

        private IEnumerator IChaser(GameObject chaserDrone)
        {
            Rigidbody rb = chaserDrone.GetComponent<Rigidbody>();
            rotationSpeed = 15f;
            float acceleration = 50f;
            float currentSpeed = 0;
            float maxSpeed = Speed;

            do
            {
                targetPos = Player.transform.position;
                targetPos.y += 0.6f;
                currentSpeed = rb.velocity.magnitude;
                _direction = (targetPos - chaserDrone.transform.position).normalized;

                if ((targetPos - chaserDrone.transform.position).magnitude > 0.1f)
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

                    if (currentSpeed != 0) { Rotate(); }
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    currentSpeed = 0f;
                }

                yield return new WaitForSeconds(0.02f);
            } while (!GameControl.dead);

            rb.velocity = Vector3.zero;
            currentSpeed = 0f;
            chaserDrone.SetActive(false);
        }

        // Rotate Player
        private void Rotate()
        {
            Vector3 lookrotation = targetPos - _chaser.transform.position;
            _chaser.transform.rotation = Quaternion.Slerp(_chaser.transform.rotation, Quaternion.LookRotation(lookrotation), rotationSpeed * Time.deltaTime);
        }
    }
}
