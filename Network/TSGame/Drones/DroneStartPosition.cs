using Launcher;
using TrueSync;

namespace MP.TSGame.Drones
{
    public delegate TSVector StartPositionDelegate(float size, Area boundary);

    public class DroneStartPosition
    {
        // Random position
        public static TSVector GetRandomPosition(float size, Area boundary)
        {
            return new TSVector(GameControl.GameState.Random.Next(boundary.LeftBoundary + (0.5f + size / 2), boundary.RightBoundary - (0.5f + size / 2)), 0.4f, GameControl.GameState.Random.Next(boundary.BottomBoundary + (0.5f + size / 2), boundary.TopBoundary - (0.5f + size / 2)));
        }

        // One of the 4 corners
        public static TSVector GetRandomCorner(float size, Area boundary)
        {
            var startPos = new TSVector();
            var location = GameControl.GameState.Random.Next(0, 4);

            switch (location)
            {
                case 0:
                    startPos = GetTopLeftCorner(size, boundary);
                    break;
                case 1:
                    startPos = GetTopRightCorner(size, boundary);
                    break;
                case 2:
                    startPos = GetBottomLeftCorner(size, boundary);
                    break;
                case 3:
                    startPos = GetBottomRightCorner(size, boundary);
                    break;
            }

            return startPos;
        }

        public static TSVector GetRandomBottomSector(float size, Area boundary)
        {
            return new TSVector(GameControl.GameState.Random.Next((boundary.LeftBoundary + (0.5f + size / 2)), (boundary.RightBoundary - (0.5f + size / 2))), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
        }

        public static TSVector GetRandomTopSector(float size, Area boundary)
        {
            return new TSVector(GameControl.GameState.Random.Next((boundary.LeftBoundary + (0.5f + size / 2)), (boundary.RightBoundary - (0.5f + size / 2))), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
        }

        public static TSVector GetRandomLeftSector(float size, Area boundary)
        {
            return new TSVector(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, GameControl.GameState.Random.Next((boundary.BottomBoundary + (0.5f + size / 2)), boundary.TopBoundary - (0.5f + size / 2)));
        }

        public static TSVector GetRandomRightSector(float size, Area boundary)
        {
            return new TSVector(boundary.RightBoundary - (0.5f + size / 2), 0.4f, GameControl.GameState.Random.Next((boundary.BottomBoundary + (0.5f + size / 2)), boundary.TopBoundary - (0.5f + size / 2)));
        }

        public static TSVector GetTopLeftCorner(float size, Area boundary)
        {
            return new TSVector(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
        }

        public static TSVector GetTopRightCorner(float size, Area boundary)
        {
            return new TSVector(boundary.RightBoundary - (0.5f + size / 2), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
        }

        public static TSVector GetBottomLeftCorner(float size, Area boundary)
        {
            return new TSVector(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
        }

        public static TSVector GetBottomRightCorner(float size, Area boundary)
        {
            return new TSVector(boundary.RightBoundary - (0.5f + size / 2), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
        }
    }
}
