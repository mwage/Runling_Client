using Assets.Scripts.SLA.Levels;
using System.Collections;
using UnityEngine;



namespace Assets.Scripts.Drones
{
    public class GridDrones
    {
        public void Grid(int droneCount, float delay, float speed, float size, Color color, Area area, DroneFactory DroneFactory, bool? addDrones = null)
        {
            bool AddDrones = addDrones ?? false;

            DroneFactory.StartCoroutine(IGridDronesHorizontal(droneCount, delay, speed, size, color, area, DroneFactory, AddDrones));
            DroneFactory.StartCoroutine(IGridDronesVertical(droneCount, delay, speed, size, color, area, DroneFactory, AddDrones));
        }

        IEnumerator IGridDronesHorizontal(int droneCount, float delay, float speed, float size, Color color, Area area, DroneFactory DroneFactory, bool AddDrones)
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

                if (AddDrones)
                    droneCount++;
            }
        }

        IEnumerator IGridDronesVertical(int droneCount, float delay, float speed, float size, Color color, Area area, DroneFactory DroneFactory, bool AddDrones)
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

                if(AddDrones)
                    droneCount += (int)(lenght / height);
            }
        }

    }
}
