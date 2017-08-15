using MP.TSGame.Drones.Movement;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Types
{
    public abstract class ADrone : IDrone
    {
        public FP Size { get; private set; }


        protected FP Speed;
        protected DroneColor Color;
        protected DroneType DroneType;
        protected DroneMovement.MovementDelegate MoveDelegate;
        protected FP? Curving;
        protected FP? SinForce;
        protected FP? SinFrequency;
        protected GameObject Player;

        protected ADrone()
        {
        }

        protected ADrone(FP speed, FP size, DroneColor color, DroneType? droneType = null, DroneMovement.MovementDelegate moveDelegate = null, 
            FP? curving = null, FP? sinForce = null, FP? sinFrequency = null, GameObject player = null)
        {
            Speed = speed;
            Size = size;
            Color = color;
            DroneType = droneType ?? DroneType.BouncingDrone;
            MoveDelegate = moveDelegate;
            Curving = curving;
            SinForce = sinForce;
            SinFrequency = sinFrequency;
            Player = player;
        }

        public abstract GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);

        public void ConfigureDrone(GameObject drone, DroneFactory factory)
        {
            var model = drone.transform.Find("Model");
            foreach (Transform child in model)
            {
                if (child.name == "Top") continue;
                if (child.name == "Sphere")
                {
                    foreach (Transform ch in child)
                    {
                        ch.GetComponent<Renderer>().material = factory.SetDroneMaterial[Color];
                    }
                }
                child.GetComponent<Renderer>().material = factory.SetDroneMaterial[Color];
            }

            // TODO: set drone size via script on drone
            //drone.(comp).tsTransform.localScale = Size * TSVector.one;
//
//            if (DroneType == DroneType.BouncingDrone || DroneType == DroneType.FlyingBouncingDrone ||
//                DroneType == DroneType.FlyingOneWayDrone)
//            {
//                if (Size > 1)
//                {
//                    drone.transform.Find("Model").transform.localPosition += new Vector3(0, (Size - 1) / 7, 0);
//                }
//            }

            // Move drone
            DroneMovement.Move(drone, Speed, MoveDelegate, Curving, SinForce, SinFrequency, Player);
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
                MoveDelegate = rhs.MoveDelegate;
                Curving = rhs.Curving;
                SinForce = rhs.SinForce;
                SinFrequency = rhs.SinFrequency;
            }
        }

    }

    public enum DroneType
    {
        BouncingDrone,
        FlyingBouncingDrone,
        FlyingOneWayDrone,
        FlyingBouncingMine,
        BouncingMine,
        FlyingOneWayMine
    }

    public enum DroneColor
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