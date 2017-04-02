using Assets.Scripts.SLA.Levels;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class DroneStartPosition
    {
        public delegate Vector3 StartPositionDelegate(float size, Area boundary);

        // Random position for bounding drones
        public static Vector3 RandomPositionGround(float size, Area boundary)
        {
            var startPos = new Vector3(Random.Range(boundary.LeftBoundary + (0.5f + size / 2), boundary.RightBoundary - (0.5f + size / 2)), 0.4f, Random.Range(boundary.BottomBoundary + (0.5f + size / 2), boundary.TopBoundary - (0.5f + size / 2)));
            return startPos;
        }

        // One of the 4 corners for bouncing drones
        public static Vector3 RandomCornerGround(float size, Area boundary)
        {
            var startPos = new Vector3();
            var location = Random.Range(0, 4);

            switch (location)
            {
                case 0:
                    startPos.Set(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 1:
                    startPos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 2:
                    startPos.Set(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 3:
                    startPos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
            }

            return startPos;
        }


        // Random position for flying drones
        public static Vector3 RandomPositionAir(float size, Area boundary)
        {
            var startPos = new Vector3(Random.Range(boundary.LeftBoundary + (0.5f + size / 2), boundary.RightBoundary - (0.5f + size / 2)), 0.6f, Random.Range(boundary.BottomBoundary + (0.5f + size / 2), boundary.TopBoundary - (0.5f + size / 2)));
            return startPos;
        }

        // One of the 4 corners randomly for flying drones
        public static Vector3 RandomCornerAir(float size, Area boundary)
        {
            var startPos = new Vector3();
            var location = Random.Range(0, 4);

            switch (location)
            {
                case 0:
                    startPos.Set(boundary.LeftBoundary + (0.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 1:
                    startPos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 2:
                    startPos.Set(boundary.LeftBoundary + (0.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 3:
                    startPos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
            }

            return startPos;
        }

        public static Vector3 GetRandomTopSector(float size, Area boundary)
        {
            var pos = new Vector3();
            var location = Random.Range(0, 7);

            switch (location)
            {
                case 0:
                    pos.Set(boundary.LeftBoundary + (10.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 1:
                    pos.Set(boundary.RightBoundary - (10.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 2:
                    pos.Set(boundary.LeftBoundary + (20.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 3:
                    pos.Set(boundary.RightBoundary - (20.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 4:
                    pos.Set(boundary.LeftBoundary + (30.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 5:
                    pos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 6:
                    pos.Set(0, 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
            }

            return pos;
        }

        public static Vector3 GetRandomBottomSector(float size, Area boundary)
        {
            var pos = new Vector3();
            var location = Random.Range(0, 7);

            switch (location)
            {
                case 0:
                    pos.Set(boundary.LeftBoundary + (10.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 1:
                    pos.Set(boundary.RightBoundary - (10.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 2:
                    pos.Set(boundary.LeftBoundary + (20.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 3:
                    pos.Set(boundary.RightBoundary - (20.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 4:
                    pos.Set(boundary.LeftBoundary + (30.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 5:
                    pos.Set(boundary.RightBoundary - (30.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 6:
                    pos.Set(0, 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
            }

            return pos;
        }
    }
}
