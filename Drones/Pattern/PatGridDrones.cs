using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class PatGridDrones : APattern
    {
        protected int DroneCount;
        protected float Delay;
        protected bool AddDrones;

        private float _speed;
        private float _size;
        private Color _color;
        private DroneType _droneType;
        private DroneMovement.MovementDelegate _moveDelegate;
        private GameObject _player;
        private float? _curving;
        private float? _sinForce;
        private float? _sinFrequency;

        public PatGridDrones(int droneCount, float delay, bool? addDrones = null)
        {
            DroneCount = droneCount;
            Delay = delay;
            AddDrones = addDrones ?? false;
        }

        public override void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null)
        {
            SetParameters(drone);
            factory.StartCoroutine(GenerateHorizontalGridDrones(drone, DroneCount, Delay, area, factory, AddDrones));
            factory.StartCoroutine(GenerateVerticalGridDrones(drone, DroneCount, Delay, area, factory, AddDrones));
        }
        
        private IEnumerator GenerateHorizontalGridDrones(IDrone drone, int droneCount, float delay, Area area, DroneFactory factory, bool addDrones)
        {
            var height = area.TopBoundary - (0.5f + _size / 2);
            var length = area.RightBoundary - (0.5f + _size / 2);
            const float direction = 90f;

            while (true)
            {
                for (var j = 0; j < (int)(length / height); j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        var startPos = new Vector3(-length, 0.6f, height - i * 2 * height / droneCount);
                        factory.SpawnDrones(new DefaultDrone(_speed, _size, _color, startPos, direction, _droneType, _moveDelegate, _player, _curving, _sinForce, _sinFrequency));

                        yield return new WaitForSeconds(delay * 2 * height / droneCount);
                    }
                    for (var i = 0; i < droneCount; i++)
                    {
                        var startPos = new Vector3(-length, 0.6f, -height + i * 2 * height / droneCount);
                        factory.SpawnDrones(new DefaultDrone(_speed, _size, _color, startPos, direction, _droneType, _moveDelegate, _player, _curving, _sinForce, _sinFrequency));
                        yield return new WaitForSeconds(delay * 2 * height / droneCount);
                    }
                }

                if (addDrones)
                    droneCount++;
            }
        }

        private IEnumerator GenerateVerticalGridDrones(IDrone drone, int droneCount, float delay, Area area, DroneFactory factory, bool addDrones)
        {
            var height = area.TopBoundary - (0.5f + _size / 2);
            var lenght = area.RightBoundary - (0.5f + _size / 2);
            const float direction = 180f;
            droneCount *= (int)(lenght / height);

            while (true)
            {
                for (var i = 0; i < droneCount; i++)
                {
                    var startPos = new Vector3(-lenght + i * 2 * lenght / droneCount, 0.6f, height);
                    factory.SpawnDrones(new DefaultDrone(_speed, _size, _color, startPos, direction, _droneType, _moveDelegate, _player, _curving, _sinForce, _sinFrequency));
                    yield return new WaitForSeconds(delay * 2 * lenght / droneCount);
                }
                for (var i = 0; i < droneCount; i++)
                {
                    var startPos = new Vector3(lenght - i * 2 * lenght / droneCount, 0.6f, height);
                    factory.SpawnDrones(new DefaultDrone(_speed, _size, _color, startPos, direction, _droneType, _moveDelegate, _player, _curving, _sinForce, _sinFrequency));
                    yield return new WaitForSeconds(delay * 2 * lenght / droneCount);
                }

                if (addDrones)
                    droneCount += (int)(lenght / height);
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
