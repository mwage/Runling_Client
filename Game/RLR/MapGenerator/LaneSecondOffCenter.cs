using UnityEngine;

namespace Game.Scripts.RLR.MapGenerator
{
    public class LaneSecondOffCenter : ALane
    {
        public LaneSecondOffCenter(Vector3 position, Vector3 rotation, Vector3 scale, MapGeneratorRLR mapGenerator) : base(position, rotation, scale, mapGenerator)
        {
        }

        public override void SetGroundAndWallsParameters()
        {
            base.SetGroundAndWallsParameters();
            foreach (Transform child in LanePrefab.transform)
            {
                foreach (Transform ch in child)
                {
                    var localPos = ch.transform.localPosition;
                    var localScale = ch.transform.localScale;

                    // Scale ground
                    if (ch.CompareTag("Ground"))
                    {
                        ch.transform.localScale = new Vector3(LaneLength, localScale.y, LaneWidth);
                    }

                    // Position walls and colliders
                    if (ch.CompareTag("Top"))
                    {
                        ch.transform.localScale = new Vector3(LaneLength + WallSize, localScale.y, WallSize);
                        ch.transform.localPosition = new Vector3(0, localPos.y, (LaneWidth + WallSize) / 2);
                    }
                    else if (ch.CompareTag("Bottom"))
                    {
                        ch.transform.localScale = new Vector3(LaneLength - LaneWidth, localScale.y, WallSize);
                        ch.transform.localPosition = new Vector3(-LaneWidth / 2, localPos.y, -(LaneWidth + WallSize) / 2);
                    }
                    else if (ch.CompareTag("Right"))
                    {
                        ch.transform.localScale = new Vector3(WallSize, localScale.y, LaneWidth + WallSize);
                        ch.transform.localPosition = new Vector3((LaneLength + WallSize) / 2, localPos.y, WallSize / 2);
                    }
                }
            }

            // Set position and rotation of the lane
            LanePrefab.transform.position = Pos;
            LanePrefab.transform.localEulerAngles = Rot;
        }
    }
}