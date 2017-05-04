using Assets.Scripts.Drones;
using Assets.Scripts.RLR.GenerateMap;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public abstract class ALevelRLR : ILevelRLR
    {
        protected readonly LevelManagerRLR Manager;
        protected readonly DroneFactory DroneFactory;
        protected readonly GenerateMapRLR GenerateMapRLR;
        protected readonly RunlingChaser RunlingChaser;

        protected ALevelRLR(LevelManagerRLR manager)
        {
            Manager = manager;
            DroneFactory = Manager.DroneFactory;
            GenerateMapRLR = Manager.GenerateMapRLR;
            RunlingChaser = Manager.RunlingChaser;
        }

        public abstract void CreateDrones();

        public virtual void GenerateMap()
        {
            GenerateMapRLR.GenerateMap(10,new float[] {7,5,7,5,7}, 1, 0.2f);
        }

        public virtual void SetChasers()
        {
            RunlingChaser.SetChaserPlatforms(new ChaserDrone(5f, 1f, Color.green, Manager.InitializeGameRLR.Player));
        }
    }
}
