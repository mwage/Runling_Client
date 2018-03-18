using Game.Scripts.Drones.Movement;
using Game.Scripts.Drones.Pattern;
using Game.Scripts.Network;
using UnityEngine;

namespace Game.Scripts.Drones.DroneTypes
{
    public abstract class ADrone : IDrone
    {
        public float Speed { get; protected set; }
        public float Size { get; private set; }
        public DroneColor Color { get; protected set; }
        public DroneType DroneType { get; protected set; }
        public IDroneMovement MovementType { get; set; }
        protected IPattern Pattern { get; set; }
        protected IDrone SpawnedDrones { get; set; }

        public static int SpeedHash => Animator.StringToHash("DroneSpeed");

        protected ADrone()
        {
        }

        protected ADrone(float speed, float size, DroneColor color, DroneType droneType, IDroneMovement movementType, 
            IPattern pattern = null, IDrone spawnedDrones = null)
        {
            Speed = speed;
            Size = size;
            Color = color;
            DroneType = droneType;
            MovementType = movementType ?? new StraightMovement();
            Pattern = pattern;
            SpawnedDrones = spawnedDrones;
        }

        public abstract GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);

        public void ConfigureDrone(GameObject drone, DroneFactory factory, DroneStateManager data = null)
        {
            drone.transform.localScale = Size * Vector3.one;

            if (!factory.IsServer)
            {
                AdjustVisuals(drone, factory);
            }
            else
            {
                
            }

            // Set up Drone Movement
            MovementType?.Initialize(drone, Speed);

            // Add pattern if drone has one
            if (Pattern != null)
            {
                factory.AddPattern(Pattern, drone, SpawnedDrones);
            }
        }

        private void AdjustVisuals(GameObject drone, DroneFactory factory)
        {
            var model = drone.transform.Find("Model");

            foreach (Transform child in model)
            {
                if (child.name == "Top")
                    continue;

                if (child.name == "Sphere")
                {
                    foreach (Transform ch in child)
                    {
                        ch.GetComponent<Renderer>().material = factory.SetDroneMaterial[Color];
                    }
                }
                child.GetComponent<Renderer>().material = factory.SetDroneMaterial[Color];
            }

            if (DroneType == DroneType.BouncingDrone || DroneType == DroneType.FlyingBouncingDrone ||
                DroneType == DroneType.FlyingOneWayDrone)
            {
                if (Size > 1)
                {
                    model.transform.localPosition += new Vector3(0, (Size - 1) / 7, 0);
                }
            }
        }

        protected void CopyFrom(IDrone sourceDrone)
        {
            var rhs = sourceDrone as ADrone;
            if (rhs != null)
            {
                Speed = rhs.Speed;
                Size = rhs.Size;
                Color = rhs.Color;
                DroneType = rhs.DroneType;
                MovementType = rhs.MovementType;
                Pattern = rhs.Pattern;
                SpawnedDrones = rhs.SpawnedDrones;
            }
        }
    }

    public enum DroneType : byte
    {
        BouncingDrone,
        FlyingBouncingDrone,
        FlyingOneWayDrone,
        FlyingBouncingMine,
        BouncingMine,
        FlyingOneWayMine
    }

    public enum DroneColor : byte
    {
        Grey,
        Blue,
        Red,
        Golden,
        Magenta,
        DarkGreen,
        Cyan, 
        BrightGreen
    }
}