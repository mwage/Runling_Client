using System.Collections;
using System.Collections.Generic;
using MP.TSGame.Drones.Pattern;
using MP.TSGame.Drones.Types;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones
{
    public class DroneFactory : TrueSyncBehaviour
    { 
        public GameObject BouncingDrone;
        public GameObject FlyingBouncingDrone;
        public GameObject FlyingOneWayDrone;
        public GameObject FlyingBouncingMine;
        public GameObject BouncingMine;
        public GameObject FlyingOneWayMine;

        public Material GreyMaterial;
        public Material BlueMaterial;
        public Material RedMaterial;
        public Material GoldenMaterial;
        public Material MagentaMaterial;
        public Material DarkGreenMaterial;
        public Material CyanMaterial;
        public Material BrightGreenMaterial;

        public Dictionary<DroneType,GameObject> SetDroneType = new Dictionary<DroneType,GameObject>();
        public Dictionary<DroneColor, Material> SetDroneMaterial = new Dictionary<DroneColor, Material>();

        public int LevelCounter;

        private void Awake()
        {
            SetDroneType[DroneType.BouncingDrone] = BouncingDrone;
            SetDroneType[DroneType.FlyingBouncingDrone] = FlyingBouncingDrone;
            SetDroneType[DroneType.FlyingOneWayDrone] = FlyingOneWayDrone;
            SetDroneType[DroneType.FlyingBouncingMine] = FlyingBouncingMine;
            SetDroneType[DroneType.BouncingMine] = BouncingMine;
            SetDroneType[DroneType.FlyingOneWayMine] = FlyingOneWayMine;

            SetDroneMaterial[DroneColor.Grey] = GreyMaterial;
            SetDroneMaterial[DroneColor.Blue] = BlueMaterial;
            SetDroneMaterial[DroneColor.Red] = RedMaterial;
            SetDroneMaterial[DroneColor.Golden] = GoldenMaterial;
            SetDroneMaterial[DroneColor.Magenta] = MagentaMaterial;
            SetDroneMaterial[DroneColor.DarkGreen] = DarkGreenMaterial;
            SetDroneMaterial[DroneColor.Cyan] = CyanMaterial;
            SetDroneMaterial[DroneColor.BrightGreen] = BrightGreenMaterial;
        }


        public List<GameObject> SpawnDrones(IDrone drone, int droneCount = 1, bool isAdded = false, Area area = new Area(), StartPositionDelegate posDelegate = null)
        {
            var drones = new List<GameObject>();

            for (var i = 0; i < droneCount; i++)
            {
                var newDrone = drone.CreateDroneInstance(this, isAdded, area, posDelegate);

                if (newDrone != null)
                {
                    drone.ConfigureDrone(newDrone, this);
                }
                drones.Add(newDrone);
            }

            return drones;
        }

        public void SetPattern(IPattern pattern, IDrone drone, Area area = new Area(), StartPositionDelegate posDelegate = null)
        {
            pattern.SetPattern(this, drone, area, posDelegate);
        }

        public void AddPattern( IPattern pattern, GameObject parentDrone, IDrone addedDrone, Area area = new Area())
        {
            pattern.AddPattern(this, parentDrone, addedDrone, area);
        }

        public void AddDrones(IDrone drone, FP delay, Area area = new Area(), StartPositionDelegate posDelegate = null)
        {
            TrueSyncManager.SyncedStartCoroutine(GenerateDrones(drone, delay, area, posDelegate));
        }

        private IEnumerator GenerateDrones(IDrone drone, FP delay, Area area, StartPositionDelegate posDelegate)
        {
            var counter = LevelCounter;
            while (true)
            {
                yield return delay;
                if (counter != LevelCounter)
                {
                    yield break;
                }
                SpawnDrones(drone, isAdded: true, area: area, posDelegate: posDelegate);
            }
        }

        public void SpawnAndAddDrones(IDrone drone, int droneCount, FP delay, Area area = new Area(), StartPositionDelegate posDelegate = null)
        {
            SpawnDrones(drone, droneCount, area: area, posDelegate: posDelegate);
            AddDrones(drone, delay, area, posDelegate);
        }

        public void BufferedSpawnAndAddDrones(IDrone drone, int droneCount, FP delay, Area area, FP timeWindow, int ticks, StartPositionDelegate posDelegate = null)
        {
            BufferedSpawnDrones(drone, droneCount, area, timeWindow, ticks);
            AddDrones(drone, delay, area, posDelegate);
        }

        public void BufferedSpawnDrones(IDrone drone, int droneCount, Area area, FP timeWindow, int ticks)
        {
            TrueSyncManager.SyncedStartCoroutine(BufferedSpawn(drone, droneCount, area, timeWindow, ticks));
        }

        private IEnumerator BufferedSpawn(IDrone drone, int droneCount, Area area, FP timeWindow, int ticks)
        {
            var tickCount = ticks;
            while (tickCount > 0)
            {
                yield return timeWindow / ticks;
                tickCount--;
                SpawnDrones(drone, droneCount / ticks, area: area);
            }
        }


    }

    // Boundaries in which new drones can be spawned
    public struct Area
    {
        public float LeftBoundary;
        public float RightBoundary;
        public float TopBoundary;
        public float BottomBoundary;
    }
}
