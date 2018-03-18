using System.Collections.Generic;
using Game.Scripts.Drones.DroneTypes;
using UnityEngine;

namespace Game.Scripts.Drones
{
    [CreateAssetMenu(fileName = "New DronePrefabs", menuName = "Drones/Prefabs")]
    public class DronePrefabs : ScriptableObject
    {
        //attach gameobjects
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
        

        public void GetPrefabs(Dictionary<DroneType, GameObject> setDroneType, Dictionary<DroneColor, Material> setDroneMaterial)
        {
            setDroneType[DroneType.BouncingDrone] = BouncingDrone;
            setDroneType[DroneType.FlyingBouncingDrone] = FlyingBouncingDrone;
            setDroneType[DroneType.FlyingOneWayDrone] = FlyingOneWayDrone;
            setDroneType[DroneType.FlyingBouncingMine] = FlyingBouncingMine;
            setDroneType[DroneType.BouncingMine] = BouncingMine;
            setDroneType[DroneType.FlyingOneWayMine] = FlyingOneWayMine;

            setDroneMaterial[DroneColor.Grey] = GreyMaterial;
            setDroneMaterial[DroneColor.Blue] = BlueMaterial;
            setDroneMaterial[DroneColor.Red] = RedMaterial;
            setDroneMaterial[DroneColor.Golden] = GoldenMaterial;
            setDroneMaterial[DroneColor.Magenta] = MagentaMaterial;
            setDroneMaterial[DroneColor.DarkGreen] = DarkGreenMaterial;
            setDroneMaterial[DroneColor.Cyan] = CyanMaterial;
            setDroneMaterial[DroneColor.BrightGreen] = BrightGreenMaterial;
        }
    }
}