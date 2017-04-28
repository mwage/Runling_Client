using Assets.Scripts.Drones;

namespace Assets.Scripts.RLR.Levels
{
    public class BoundariesRLR
    {
        public static Area BouncingSla = new Area
        {
            LeftBoundary = -35f,
            RightBoundary = 35f,
            TopBoundary = 5f,
            BottomBoundary = -5f
        };

        public static Area FlyingSla = new Area
        {
            LeftBoundary = -40f,
            RightBoundary = 40f,
            TopBoundary = 20f,
            BottomBoundary = -20f
        };

        public static Area Test = new Area
        {
            LeftBoundary = -15f,
            RightBoundary = 15f,
            TopBoundary = 2f,
            BottomBoundary = -2f
        };

        public static Area createAreaBouncing(float L, float R, float T, float B)
        {
            return new Drones.Area
            {
                LeftBoundary = L,
                RightBoundary = R,
                TopBoundary = T,
                BottomBoundary = B
            };

        }
    }
}