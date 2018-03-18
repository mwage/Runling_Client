using UnityEngine;

namespace Game.Scripts.RLR.MapGenerator
{
    public class LaneFirstOffCenter : ALane
    {
        private readonly float _centerSize;
        public LaneFirstOffCenter(Vector3 position, Vector3 rotation, Vector3 scale, float centerSize, MapGeneratorRLR mapGenerator) 
            : base(position, rotation, scale, mapGenerator)
        {
            _centerSize = centerSize;
        }

        public override void InstantiateRamp(GameObject ramp, Transform terrain)
        {
            var thisLane = Object.Instantiate(ramp, terrain);
            thisLane.transform.position = Vector3.zero;
            var gap = -(Pos.x + LaneWidth / 2 + _centerSize);
            foreach (Transform child in thisLane.transform)
            {
                foreach (Transform ch in child)
                {
                    var localPos = ch.transform.localPosition;
                    var localScale = ch.transform.localScale;

                    // Scale walls and colliders
                    if (ch.CompareTag("Top") || ch.CompareTag("Bottom"))
                    {
                        ch.transform.localScale = new Vector3(gap, localScale.y, WallSize);
                    }

                    // Scale ground
                    if (ch.CompareTag("Ground"))
                    {
                        ch.transform.localScale = new Vector3(gap, localScale.y, LaneWidth);
                        ch.transform.localPosition = new Vector3(-(_centerSize + gap / 2), localPos.y, _centerSize - LaneWidth / 2);
                    }

                    // Position walls and colliders
                    if (ch.CompareTag("Top"))
                    {
                        ch.transform.localPosition = new Vector3(-(_centerSize + gap / 2), localPos.y, _centerSize + WallSize / 2);
                    }
                    else if (ch.CompareTag("Bottom"))
                    {
                        ch.transform.localPosition = new Vector3(-(_centerSize + gap / 2), localPos.y, _centerSize - LaneWidth - WallSize / 2);
                    }
                }
            }
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
                    if (ch.CompareTag("Right"))
                    {
                        //SetWallOrPlayerCollider(ch, child, (LaneLength + WallSize) / 2, WallSize / 2, WallSize, LaneWidth + WallSize);
                        ch.transform.localScale = new Vector3(WallSize, localScale.y, LaneWidth + WallSize);
                        ch.transform.localPosition = new Vector3((LaneLength + WallSize) / 2, localPos.y, WallSize / 2);
                    }
                    else if (ch.CompareTag("Bottom"))
                    {
                        ch.transform.localScale = new Vector3(LaneLength - LaneWidth, localScale.y, WallSize);
                        ch.transform.localPosition = new Vector3(localPos.x - LaneWidth / 2, localPos.y, -(LaneWidth / 2 + WallSize / 2));
                    }
                }
            }
        }

        public override void SetSafeZoneParameters(float nextLineWidth)
        {
            foreach (Transform child in PlatformPrefab.transform)
            {
                foreach (Transform ch in child)
                {
                    var localScale = ch.transform.localScale;
                    if (ch.CompareTag("Top"))
                    {
                        SetWallOrPlayerCollider(ch, child, 0F, _centerSize + WallSize / 2, 2 * _centerSize, WallSize);
                    }

                    else if (ch.CompareTag("Right"))
                    {
                        SetWallOrPlayerCollider(ch, child, _centerSize+WallSize/2, 0F, WallSize, _centerSize * 2 + 2 * WallSize);

                    }
                    else if (ch.CompareTag("Left"))
                    {
                        SetWallOrPlayerCollider(ch, child, -(_centerSize + WallSize / 2) , -(LaneWidth / 2 + WallSize), WallSize, 2 * _centerSize - LaneWidth );
                        SetDroneCollider(ch, child, (LanePrefab.transform.position.x + LaneWidth / 2 - (Pos.x + LaneWidth / 2 + _centerSize) + WallSize/2), _centerSize-LaneWidth/2 - WallSize/2, WallSize, LaneWidth + WallSize);
                    }

                    // Scale ground and safezone
                    else if (ch.CompareTag("Finish") || ch.CompareTag("Ground") || ch.CompareTag("SafeZone"))
                    {
                        ch.transform.localScale = new Vector3(_centerSize * 2, localScale.y, _centerSize * 2);
                    }

                    // Position walls and colliders
                    else if (ch.CompareTag("Bottom"))
                    {
                        SetWallOrPlayerCollider(ch, child, 0F, -(_centerSize+WallSize/2), 2*_centerSize, WallSize);
                    }

                    // Set material tiling
                    if (ch.CompareTag("Ground"))
                    {
                        SetPlatformTexture(ch);
                    }
                }
            }
        }
    }
}