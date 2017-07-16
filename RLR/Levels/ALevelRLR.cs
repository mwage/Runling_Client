using Drones;
using Drones.DroneTypes;
using Drones.Movement;
using RLR.GenerateMap;

namespace RLR.Levels
{
    public abstract class ALevelRLR : ILevelRLR
    {
        protected readonly LevelManagerRLR Manager;
        protected readonly DroneFactory DroneFactory;
        protected readonly GenerateMapRLR GenerateMapRLR;
        protected readonly RunlingChaser RunlingChaser;

        protected Area[] LaneArea;

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
            GenerateMapRLR.GenerateMap(15,new float[] {8, 6, 8, 6, 8}, 1.2f, 0.3f, SetAirCollider());
            LaneArea = GenerateMapRLR.GetDroneSpawnArea();
        }

        protected virtual float SetAirCollider()
        {
            return 20;
        }

        public virtual void SetChasers()
        {
            RunlingChaser.SetChaserPlatforms(new DefaultDrone(5f, 1f, DroneColor.DarkGreen, moveDelegate: DroneMovement.ChaserMovement));
        }
    }
}
