using Drones;
using Drones.DroneTypes;
using Drones.Movement;
using Launcher;
using RLR.GenerateMap;

namespace RLR.Levels
{
    public abstract class ALevelRLR : ILevelRLR
    {
        protected readonly LevelManagerRLR Manager;
        protected readonly DroneFactory DroneFactory;
        protected readonly MapGeneratorRLR MapGeneratorRlr;
        protected readonly RunlingChaser RunlingChaser;

        protected Area[] LaneArea;

        protected ALevelRLR(LevelManagerRLR manager)
        {
            Manager = manager;
            DroneFactory = Manager.DroneFactory;
            MapGeneratorRlr = Manager.MapGeneratorRlr;
            RunlingChaser = Manager.RunlingChaser;
        }

        public abstract void CreateDrones();

        public virtual void GenerateMap()
        {
            MapGeneratorRlr.GenerateMap(15,new float[] {8, 6, 8, 6, 8}, 1.2f, 0.3f, SetAirCollider());
            GameControl.MapState.SafeZones = MapGeneratorRlr.GetSafeZones();
            GameControl.MapState.VisitedSafeZones = new bool[GameControl.MapState.SafeZones.Count];
            LaneArea = MapGeneratorRlr.GetDroneSpawnArea();
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
