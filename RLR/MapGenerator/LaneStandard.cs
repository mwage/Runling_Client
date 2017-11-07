using UnityEngine;

namespace RLR.MapGenerator
{
    public class LaneStandard : ALane
    {
        public LaneStandard(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
        {
        }

        public override void SetGroundAndWallsParameters()
        {
            base.SetGroundAndWallsParameters();
            foreach (Transform child in LanePrefab.transform)
            {
                foreach (Transform ch in child)
                {
                    // Scale ground
                    switch (ch.tag)
                    {
                        case "Right":
                            SetWallOrPlayerCollider(ch, child, (LaneLength + WallSize) / 2, WallSize / 2, WallSize, LaneWidth + WallSize);
                            break;
                        case "Bottom":
                            SetWallOrPlayerCollider(ch, child, 0F, -(LaneWidth / 2 + WallSize / 2), LaneLength, WallSize);
                            break;
                    } 
                }
            }
        }
    }
}
