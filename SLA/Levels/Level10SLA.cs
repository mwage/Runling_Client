using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level10SLA : ALevel
    {
        public Level10SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            //Spawn drones (dronecount/delay, speed, size, color)
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1f, Color.blue), 10, 4f);

            // Grid Drones
            GridDrones(10, 0.05f, 7f, 1f, Color.magenta, BoundariesSLA.FlyingSla);
        }

        public void GridDrones(int droneCount, float delay, float speed, float size, Color color, Area area)
        {
            DroneFactory.StartCoroutine(IGridDronesHorizontal(droneCount, delay, speed, size, color, area));
            DroneFactory.StartCoroutine(IGridDronesVertical(droneCount, delay, speed, size, color, area));
        }

        IEnumerator IGridDronesHorizontal(int droneCount, float delay, float speed, float size, Color color, Area area)
        {
            var height = area.TopBoundary - (0.5f + size / 2);
            var lenght = area.RightBoundary - (0.5f + size / 2);
            var direction = 90f;

            while (true)
            {
                for (var j = 0; j < (int)(lenght / height); j++)
                {
                    for (var i = 0; i < droneCount; i++)
                    {
                        var startPos = new Vector3(-lenght, 0.6f, height - i * 2 * height / droneCount);
                        DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, direction));

                        yield return new WaitForSeconds(delay * 2 * height / droneCount);
                    }
                    for (var i = 0; i < droneCount; i++)
                    {
                        var startPos = new Vector3(-lenght, 0.6f, -height + i * 2 * height / droneCount);
                        DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, direction));
                        yield return new WaitForSeconds(delay * 2 * height / droneCount);
                    }
                }
                droneCount++;
            }
        }
        IEnumerator IGridDronesVertical(int droneCount, float delay, float speed, float size, Color color, Area area)
        {
            var height = area.TopBoundary - (0.5f + size / 2);
            var lenght = area.RightBoundary - (0.5f + size / 2);
            var direction = 180f;
            droneCount *= (int)(lenght / height);

            while (true)
            {
                for (var i = 0; i < droneCount; i++)
                {
                    var startPos = new Vector3(-lenght + i * 2 * lenght / droneCount, 0.6f, height);
                    DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, direction));
                    yield return new WaitForSeconds(delay * 2 * lenght / droneCount);
                }
                for (var i = 0; i < droneCount; i++)
                {
                    var startPos = new Vector3(lenght - i * 2 * lenght / droneCount, 0.6f, height);
                    DroneFactory.SpawnDrones(new StraightFlyingOnewayDrone(speed, size, color, startPos, direction));
                    yield return new WaitForSeconds(delay * 2 * lenght / droneCount);
                }
                droneCount += (int)(lenght / height);
            }
        }
    }
}
