using System.Collections;
using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class ChaserDrone : ADrone
    {
        protected readonly GameObject Player;

        private Vector3 _targetPos;
        private Vector3 _direction;
        private float _rotationSpeed;
        private float _maxSpeed;
        private float _currentSpeed;
        private float _acceleration;
        private Rigidbody _rb;
        private GameObject _chaser;

        public ChaserDrone(float speed, float size, Color color, GameObject player) : base(speed, size, color)
        {
            Player = player;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded)
        {
            _chaser = Object.Instantiate(factory.FlyingOnewayDrone, new Vector3(0, 0.6f, 0), Quaternion.identity);

            _rb = _chaser.GetComponent<Rigidbody>();

            _rotationSpeed = 15f;
            _acceleration = 500f;
            _currentSpeed = 0;
            _maxSpeed = Speed;

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
            do
            {
                _targetPos = Player.transform.position;
                _direction = (_targetPos - chaserDrone.transform.position).normalized;
                _rb.velocity = _direction * _currentSpeed;

                if (_currentSpeed < _maxSpeed)
                {
                    _rb.AddForce(_direction * _acceleration, ForceMode.Acceleration);
                    _currentSpeed = _rb.velocity.magnitude;
                }

                // Don't accelerate over maxSpeed
                else
                {
                    _currentSpeed = _maxSpeed;
                    _rb.velocity = _direction * _currentSpeed;
                }

                if (_currentSpeed != 0) { Rotate(); }

                yield return new WaitForSeconds(0.02f);
            } while (!GameControl.dead);

            _rb.velocity = Vector3.zero;
            _currentSpeed = 0f;
            chaserDrone.SetActive(false);
        }

        // Rotate Player
        private void Rotate()
        {
            Vector3 lookrotation = _targetPos - _chaser.transform.position;
            _chaser.transform.rotation = Quaternion.Slerp(_chaser.transform.rotation, Quaternion.LookRotation(lookrotation), _rotationSpeed * Time.deltaTime);
        }
    }
}
