﻿using UnityEngine;

namespace Drones.Movement
{

    public class RotateBouncingDrone : ADroneMovement
    {
        public float DroneSpeed;
        private float _rotation;
        private Rigidbody _rb;
        private float _angle = 15;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rotation = Random.Range(-1, 1) >= 0 ? DroneSpeed / 5 : - DroneSpeed / 5;
        }

        private void FixedUpdate()
        {
            transform.Rotate(0, _rotation * _angle / 90, 0);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 19)
            {
                _angle = Vector3.Angle(_rb.velocity, collision.contacts[0].normal);
                //_angle = Mathf.Atan2(Vector3.Dot(Vector3.up, Vector3.Cross(_rb.velocity, collision.contacts[0].normal)), Vector3.Dot(_rb.velocity, collision.contacts[0].normal)) * Mathf.Rad2Deg;
            }
        }

        public override void Move()
        {
            throw new System.NotImplementedException();
        }

        public override void Freeze()
        {
            throw new System.NotImplementedException();
        }

        public override void UnFreeze()
        {
            throw new System.NotImplementedException();
        }

        public override void SlowDown(float percentage)
        {
            throw new System.NotImplementedException();
        }

        public override void UnSlowDown()
        {
            throw new System.NotImplementedException();
        }
    }
}