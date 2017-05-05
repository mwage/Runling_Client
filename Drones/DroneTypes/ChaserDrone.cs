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
        
        public ChaserDrone(float speed, float size, Color color, GameObject player) : base(speed, size, color)
        {
            Player = player;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var chaser = Object.Instantiate(factory.FlyingOnewayDrone, new Vector3(0, 0.6f, 0), Quaternion.identity);

            // adjust drone color and size
            var rend = chaser.GetComponent<Renderer>();
            rend.material.color = Color;
            var scale = chaser.transform.localScale;
            scale.x *= Size;
            scale.z *= Size;
            chaser.transform.localScale = scale;

            DroneMovement.ChaserMovement(chaser, Speed, Player);
            
            return chaser;
        }
    }
}
