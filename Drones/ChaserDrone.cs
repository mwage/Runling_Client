using System;
using System.Collections;
using Assets.Scripts.Launcher;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Drones
{
    public class ChaserDrone : ADrone
    {
        protected readonly GameObject Player;

        private Vector3 _targetPos;
        private Vector3 _direction;

        private GameObject _chaser;
        private float _rotationSpeed = 15f;
        
        public ChaserDrone(float speed, float size, Color color, GameObject player) : base(speed, size, color)
        {
            Player = player;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            _chaser = Object.Instantiate(factory.FlyingOnewayDrone, new Vector3(0, 0.6f, 0), Quaternion.identity);

            // adjust drone color and size
            var rend = _chaser.GetComponent<Renderer>();
            rend.material.color = Color;
            var scale = _chaser.transform.localScale;
            scale.x *= Size;
            scale.z *= Size;
            _chaser.transform.localScale = scale;

            factory.StartCoroutine(MoveChaser(_chaser));
            return _chaser;
        }

        private IEnumerator MoveChaser(GameObject chaserDrone)
        {
            var rb = chaserDrone.GetComponent<Rigidbody>();
            _rotationSpeed = 15f;
            const float acceleration = 50f;
            var maxSpeed = Speed;

            do
            {
                _targetPos = Player.transform.position;
                _targetPos.y += 0.6f;
                var currentSpeed = rb.velocity.magnitude;
                _direction = (_targetPos - chaserDrone.transform.position).normalized;

                if ((_targetPos - chaserDrone.transform.position).magnitude > 0.1f)
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

                    if (Math.Abs(currentSpeed) > 0.00001) { Rotate(); }
                }
                else
                {
                    rb.velocity = Vector3.zero;
                }

                yield return new WaitForSeconds(0.02f);
            } while (!GameControl.Dead);

            rb.velocity = Vector3.zero;
            chaserDrone.SetActive(false);
        }

        // Rotate Player
        private void Rotate()
        {
            var lookrotation = _targetPos - _chaser.transform.position;
            _chaser.transform.rotation = Quaternion.Slerp(_chaser.transform.rotation, Quaternion.LookRotation(lookrotation), _rotationSpeed * Time.deltaTime);
        }
    }
}
