using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Drones {
    public class PatContinuousSpawn : APattern
    {
        protected readonly float Delay;
        protected readonly int DroneCount;

        private float _speed;
        private float _size;
        private Color _color;
        private DroneType _droneType;
        private DroneMovement.MovementDelegate _moveDelegate;
        private GameObject _player;
        private float? _curving;
        private float? _sinForce;
        private float? _sinFrequency;

        public PatContinuousSpawn(float delay, int droneCount)
        {
            Delay = delay;
            DroneCount = droneCount;
        }

        public override void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null)
        {
            if (posDelegate == null)
                posDelegate = delegate { return new Vector3(0, 0.6f, 0); };

            factory.StartCoroutine(GenerateDrones(factory, drone, posDelegate));
        }

        public override void AddPattern(DroneFactory factory, GameObject drone, IDrone addedDrone, Area area)
        {
            SetParameters(addedDrone);
            factory.StartCoroutine(GenerateDrones(factory, addedDrone, delegate { return Vector3.zero; }, drone));
        }

        IEnumerator GenerateDrones(DroneFactory factory, IDrone drone, StartPositionDelegate posDelegate, GameObject parentDrone = null)
        {
            var addPattern = parentDrone != null;
            while (true)
            {
                if (parentDrone == null && addPattern) { yield break; }
                if (parentDrone != null)
                {
                    factory.SpawnDrones( new RandomDrone(_speed, _size, _color, _droneType, moveDelegate: _moveDelegate, player: _player, curving: _curving, sinForce: _sinForce, sinFrequency: _sinFrequency), DroneCount, posDelegate: delegate
                    {
                        return parentDrone.transform.position;
                    });
                }
                else
                {
                    factory.SpawnDrones(drone, DroneCount, posDelegate: posDelegate);
                }
                yield return new WaitForSeconds(Delay);
            }
        }

        private void SetParameters(IDrone drone)
        {
            _speed = (float)drone.GetParameters()[0];
            _size = (float)drone.GetParameters()[1];
            _color = (Color)drone.GetParameters()[2];
            _droneType = (DroneType)drone.GetParameters()[3];
            _moveDelegate = (DroneMovement.MovementDelegate)drone.GetParameters()[4];
            _player = (GameObject)drone.GetParameters()[5];
            _curving = (float?)drone.GetParameters()[6];
            _sinForce = (float?)drone.GetParameters()[7];
            _sinFrequency = (float?)drone.GetParameters()[8];
        }
    }
}
