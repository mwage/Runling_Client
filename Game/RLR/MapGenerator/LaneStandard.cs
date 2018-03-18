using UnityEngine;

namespace Game.Scripts.RLR.MapGenerator
{
    public class LaneStandard : ALane
    {
        public LaneStandard(Vector3 position, Vector3 rotation, Vector3 scale, MapGeneratorRLR mapGenerator) : base(position, rotation, scale, mapGenerator)
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
