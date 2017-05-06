using System;
using System.Collections;
using Assets.Scripts.Launcher;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Drones
{
    public class ChaserDrone : ADrone
    {
        protected readonly GameObject Player;
        
        public ChaserDrone(float speed, float size, Color color, GameObject player, DroneType? droneType = null) : base(speed, size, color, droneType)
        {
            Player = player;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var chaser = factory.SpawnDrones(new OnewayDrone(Speed, Size, Color));

            DroneMovement.ChaserMovement(chaser[0], Speed, Player);
            return chaser[0];
        }
    }
}
