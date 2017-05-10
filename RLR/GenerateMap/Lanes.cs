using UnityEngine;

namespace Assets.Scripts.RLR.GenerateMap
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

    /********************************* CENTER LINE ****************************************************/
    public class LaneFirstOffCenter : ALane
    {
        private readonly float _centerSize;
        public LaneFirstOffCenter(Vector3 position, Vector3 rotation, Vector3 scale, float centerSize) : base(position, rotation, scale)
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
                    Vector3 localPos = ch.transform.localPosition;
                    Vector3 localScale = ch.transform.localScale;

                    // Scale walls and colliders
                    if (ch.tag == "Top" || ch.tag == "Bottom")
                    {
                        ch.transform.localScale = new Vector3(gap, localScale.y, WallSize);
                    }

                    // Scale ground
                    if (ch.tag == "Ground")
                    {
                        ch.transform.localScale = new Vector3(gap, localScale.y, LaneWidth);
                        ch.transform.localPosition = new Vector3(-(_centerSize + gap / 2), localPos.y, _centerSize - LaneWidth / 2);
                    }

                    // Position walls and colliders
                    if (ch.tag == "Top")
                    {
                        ch.transform.localPosition = new Vector3(-(_centerSize + gap / 2), localPos.y, _centerSize + WallSize / 2);
                    }
                    else if (ch.tag == "Bottom")
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
                    Vector3 localPos = ch.transform.localPosition;
                    Vector3 localScale = ch.transform.localScale;

                    // Scale ground
                    if (ch.tag == "Right")
                    {
                        //SetWallOrPlayerCollider(ch, child, (LaneLength + WallSize) / 2, WallSize / 2, WallSize, LaneWidth + WallSize);
                        ch.transform.localScale = new Vector3(WallSize, localScale.y, LaneWidth + WallSize);
                        ch.transform.localPosition = new Vector3((LaneLength + WallSize) / 2, localPos.y, WallSize / 2);
                    }
                    else if (ch.tag == "Bottom")
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
                    Vector3 localScale = ch.transform.localScale;
                    if (ch.tag == "Top")
                    {
                        SetWallOrPlayerCollider(ch, child, 0F, _centerSize + WallSize / 2, 2 * _centerSize, WallSize);
                    }

                    else if (ch.tag == "Right")
                    {
                        SetWallOrPlayerCollider(ch, child, _centerSize+WallSize/2, 0F, WallSize, _centerSize * 2 + 2 * WallSize);

                    }
                    else if (ch.tag == "Left")
                    {
                        SetWallOrPlayerCollider(ch, child, -(_centerSize + WallSize / 2) , -(LaneWidth / 2 + WallSize), WallSize, 2 * _centerSize - LaneWidth );
                        SetDroneCollider(ch, child, (LanePrefab.transform.position.x + LaneWidth / 2 - (Pos.x + LaneWidth / 2 + _centerSize) + WallSize/2), _centerSize-LaneWidth/2 - WallSize/2, WallSize, LaneWidth + WallSize);
                    }

                    // Scale ground and safezone
                    else if (ch.tag == "Finish" || ch.tag == "Ground" || ch.tag == "SafeZone")
                    {
                        ch.transform.localScale = new Vector3(_centerSize * 2, localScale.y, _centerSize * 2);
                    }

                    // Position walls and colliders
                    else if (ch.tag == "Bottom")
                    {
                        SetWallOrPlayerCollider(ch, child, 0F, -(_centerSize+WallSize/2), 2*_centerSize, WallSize);
                    }

                    // Set material tiling
                    if (ch.tag == "Ground")
                    {
                        SetPlatformTexture(ch);
                    }
                }
            }
        }
    }
    /************************************** SECOND LINE OF CENTER *************************************************/ 
    public class LaneSecondOffCenter : ALane
    {
        public LaneSecondOffCenter(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
        {

        }

        public override void SetGroundAndWallsParameters()
        {
            base.SetGroundAndWallsParameters();
            foreach (Transform child in LanePrefab.transform)
            {
                foreach (Transform ch in child)
                {
                    Vector3 localPos = ch.transform.localPosition;
                    Vector3 localScale = ch.transform.localScale;

                    // Scale ground
                    if (ch.tag == "Ground")
                    {
                        ch.transform.localScale = new Vector3(LaneLength, localScale.y, LaneWidth);
                    }

                    // Position walls and colliders
                    if (ch.tag == "Top")
                    {
                        ch.transform.localScale = new Vector3(LaneLength + WallSize, localScale.y, WallSize);
                        ch.transform.localPosition = new Vector3(0, localPos.y, (LaneWidth + WallSize) / 2);
                    }
                    else if (ch.tag == "Bottom")
                    {
                        ch.transform.localScale = new Vector3(LaneLength - LaneWidth, localScale.y, WallSize);
                        ch.transform.localPosition = new Vector3(-LaneWidth / 2, localPos.y, -(LaneWidth + WallSize) / 2);
                    }
                    else if (ch.tag == "Right")
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
