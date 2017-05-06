using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class PatGridDrones : APattern
    {
        protected int DroneCount;
        protected float Delay;
        protected bool AddDrones;
        protected DroneMovement.MovementDelegate MoveDelegate;

        public PatGridDrones(int droneCount, float delay, bool? addDrones = null, DroneMovement.MovementDelegate moveDelegate = null )
        {
            DroneCount = droneCount;
            Delay = delay;
            AddDrones = addDrones ?? false;
            MoveDelegate = moveDelegate;
        }

        public override void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null, DroneMovement.MovementDelegate moveDelegate = null)
        {
            factory.StartCoroutine(GenerateHorizontalGridDrones(drone, DroneCount, Delay, area, factory, AddDrones, MoveDelegate));
            factory.StartCoroutine(GenerateVerticalGridDrones(drone, DroneCount, Delay, area, factory, AddDrones, MoveDelegate));
        }
        
        private static IEnumerator GenerateHorizontalGridDrones(IDrone drone, int droneCount, float delay, Area area, DroneFactory factory, bool addDrones, DroneMovement.MovementDelegate moveDelegate)
        {
            var size = drone.GetSize();
            var speed = drone.GetSpeed();
            var color = drone.GetColor();
            var height = area.TopBoundary - (0.5f + size / 2);
            var length = area.RightBoundary - (0.5f + size / 2);
            const float direction = 90f;

            while (true)
            {
                for (var j = 0; j < (int)(length / height); j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        var startPos = new Vector3(-length, 0.6f, height - i * 2 * height / droneCount);
                        factory.SpawnDrones(new DefaultDrone(speed, size, color, startPos, direction), moveDelegate: moveDelegate);

                        yield return new WaitForSeconds(delay * 2 * height / droneCount);
                    }
                    for (var i = 0; i < droneCount; i++)
                    {
                        var startPos = new Vector3(-length, 0.6f, -height + i * 2 * height / droneCount);
                        factory.SpawnDrones(new DefaultDrone(speed, size, color, startPos, direction), moveDelegate: moveDelegate);
                        yield return new WaitForSeconds(delay * 2 * height / droneCount);
                    }
                }

                if (addDrones)
                    droneCount++;
            }
        }

        private static IEnumerator GenerateVerticalGridDrones(IDrone drone, int droneCount, float delay, Area area, DroneFactory factory, bool addDrones, DroneMovement.MovementDelegate moveDelegate)
        {
            var size = drone.GetSize();
            var speed = drone.GetSpeed();
            var color = drone.GetColor();

            var height = area.TopBoundary - (0.5f + size / 2);
            var lenght = area.RightBoundary - (0.5f + size / 2);
            const float direction = 180f;
            droneCount *= (int)(lenght / height);

            while (true)
            {
                for (var i = 0; i < droneCount; i++)
                {
                    var startPos = new Vector3(-lenght + i * 2 * lenght / droneCount, 0.6f, height);
                    factory.SpawnDrones(new DefaultDrone(speed, size, color, startPos, direction), moveDelegate: moveDelegate);
                    yield return new WaitForSeconds(delay * 2 * lenght / droneCount);
                }
                for (var i = 0; i < droneCount; i++)
                {
                    var startPos = new Vector3(lenght - i * 2 * lenght / droneCount, 0.6f, height);
                    factory.SpawnDrones(new DefaultDrone(speed, size, color, startPos, direction), moveDelegate: moveDelegate);
                    yield return new WaitForSeconds(delay * 2 * lenght / droneCount);
                }

                if (addDrones)
                    droneCount += (int)(lenght / height);
            }
        }

    }
}
