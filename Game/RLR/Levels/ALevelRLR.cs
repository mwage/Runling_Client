using Client.Scripts.Launcher;
using Game.Scripts.Drones;
using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.RLR.MapGenerator;

namespace Game.Scripts.RLR.Levels
{
    public abstract class ALevelRLR : ILevelRLR
    {
        protected readonly ILevelManagerRLR Manager;
        protected readonly DroneFactory DroneFactory;
        protected readonly MapGeneratorRLR MapGeneratorRlr;
        protected readonly RunlingChaser RunlingChaser;

        protected Area[] LaneArea;

        protected ALevelRLR(ILevelManagerRLR manager)
        {
            Manager = manager;
            DroneFactory = Manager.DroneFactory;
            MapGeneratorRlr = Manager.MapGenerator;
            RunlingChaser = Manager.RunlingChaser;
        }

        public abstract void CreateDrones();

        public virtual void GenerateMap()
        {
            MapGeneratorRlr.GenerateMap(20, new float[] {10, 8, 10, 8, 10}, 1.2f, 0.3f, SetAirCollider());

            // TODO: Remove Client dependancy
            GameControl.GameState.SafeZones = MapGeneratorRlr.GetSafeZones();
            LaneArea = MapGeneratorRlr.GetDroneSpawnArea();
        }

        protected virtual float SetAirCollider()
        {
            return 20;
        }

        public virtual void SetChasers()
        {
            RunlingChaser.SetChaserPlatforms(new DefaultDrone(5f, 1f, DroneColor.DarkGreen));
        }
    }
}
