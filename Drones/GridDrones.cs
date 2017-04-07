using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class GridDrones : ADrone
    {
        protected int DroneCount;
        protected float Delay;

        public GridDrones(float speed, float size, Color color, int droneCount, float delay ) : base(speed, size, color)
        {
            DroneCount = droneCount;
            Delay = delay;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area)
        {
            factory.StartCoroutine(GenerateHorizontalGridDrones(DroneCount, Delay, Speed, Size, Color, area, factory));
            factory.StartCoroutine(GenerateVerticalGridDrones(DroneCount, Delay, Speed, Size, Color, area, factory));
            return null;
        }
        
        private static IEnumerator GenerateHorizontalGridDrones(int droneCount, float delay, float speed, float size, Color color, Area area, DroneFactory factory)
        {
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
                        factory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, direction));

                        yield return new WaitForSeconds(delay * 2 * height / droneCount);
                    }
                    for (var i = 0; i < droneCount; i++)
                    {
                        var startPos = new Vector3(-length, 0.6f, -height + i * 2 * height / droneCount);
                        factory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, direction));
                        yield return new WaitForSeconds(delay * 2 * height / droneCount);
                    }
                }

                droneCount++;
            }
        }

        private static IEnumerator GenerateVerticalGridDrones(int droneCount, float delay, float speed, float size, Color color, Area area, DroneFactory factory)
        {
            var height = area.TopBoundary - (0.5f + size / 2);
            var lenght = area.RightBoundary - (0.5f + size / 2);
            const float direction = 180f;
            droneCount *= (int)(lenght / height);

            while (true)
            {
                for (var i = 0; i < droneCount; i++)
                {
                    var startPos = new Vector3(-lenght + i * 2 * lenght / droneCount, 0.6f, height);
                    factory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, direction));
                    yield return new WaitForSeconds(delay * 2 * lenght / droneCount);
                }
                for (var i = 0; i < droneCount; i++)
                {
                    var startPos = new Vector3(lenght - i * 2 * lenght / droneCount, 0.6f, height);
                    factory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, direction));
                    yield return new WaitForSeconds(delay * 2 * lenght / droneCount);
                }

                droneCount += (int)(lenght / height);
            }
        }

    }
}
