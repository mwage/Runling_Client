using Drones.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Drones.DroneTypes
{
    public abstract class ADrone : IDrone
    {
        public float Size { get; private set; }


        protected float Speed;
        protected DroneColor Color;
        protected DroneType DroneType;
        protected DroneMovement.MovementDelegate MoveDelegate;
        protected float? Curving;
        protected float? SinForce;
        protected float? SinFrequency;


        protected ADrone()
        {
        }

        protected ADrone(float speed, float size, DroneColor color, DroneType? droneType = null, DroneMovement.MovementDelegate moveDelegate = null, 
            float? curving = null, float? sinForce = null, float? sinFrequency = null)
        {
            Speed = speed;
            Size = size;
            Color = color;
            DroneType = droneType ?? DroneType.BouncingDrone;
            MoveDelegate = moveDelegate;
            Curving = curving;
            SinForce = sinForce;
            SinFrequency = sinFrequency;
        }

        public abstract GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);

        public void ConfigureDrone(GameObject drone, DroneFactory factory)
        {
            var applyMaterials = drone.GetComponent<ApplyMaterials>();
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                applyMaterials.PhotonView.RPC("ChangeColorAndSize", PhotonTargets.All, Color, Size);
            }
            else
            {
                var model = drone.transform.GetChild(0);
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

                drone.transform.localScale = Size * Vector3.one;
            }

            if (DroneType == DroneType.BouncingDrone || DroneType == DroneType.FlyingBouncingDrone ||
                DroneType == DroneType.FlyingOneWayDrone)
            {
                if (Size > 1)
                {
                    drone.transform.Find("Model").transform.localPosition += new Vector3(0, (Size - 1) / 7, 0);
                }
            }

            // Move drone
            DroneMovement.Move(drone, Speed, MoveDelegate, Curving, SinForce, SinFrequency);
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