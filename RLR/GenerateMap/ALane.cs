using UnityEngine;

namespace Assets.Scripts.RLR.GenerateMap
{
    public abstract class ALane
    {
        public Vector3 Pos, Rot, Sc;
        public GameObject PlatformPrefab;
        public GameObject LanePrefab;
        public static float WallSize, ColliderWallSize;
        public readonly float LaneWidth, LaneLength;

        protected ALane(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Pos = position;
            Rot = rotation;
            Sc = scale;
            LaneWidth = scale.z;
            LaneLength = scale.x;
        }

        public virtual GameObject InstatiateCompleteLanePrefab(GameObject firstLineOfCenterPrefab, GameObject platformPrefabP, Transform terrain)
        {
            LanePrefab = Object.Instantiate(firstLineOfCenterPrefab, terrain); // assign prefab to local ref
            return PlatformPrefab = Object.Instantiate(platformPrefabP, terrain);
        }

        public virtual void InstantiateRamp(GameObject ramp, Transform terrain)
        {
        }

        public virtual void SetGroundAndWallsParameters()
        {
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

                    // Position and scale walls and colliders
                    if (ch.tag == "Top")
                    {
                        ch.transform.localScale = new Vector3(LaneLength, localScale.y, WallSize);
                        ch.transform.localPosition = new Vector3(0, localPos.y, (LaneWidth + WallSize) / 2);
                    }
                    else if (ch.tag == "Bottom")
                    {
                        ch.transform.localScale = new Vector3(LaneLength, localScale.y, WallSize);
                        ch.transform.localPosition = new Vector3(0, localPos.y, -(LaneWidth + WallSize) / 2);
                    }
                }
            }

            // Set position and rotation of the lane
            LanePrefab.transform.position = Pos;
            LanePrefab.transform.localEulerAngles = Rot;
        }

        public virtual void SetSafeZoneParameters(float nextLineWidth)
        {
            foreach (Transform child in PlatformPrefab.transform)
            {
                foreach (Transform ch in child)
                {
                    Vector3 localPos = ch.transform.localPosition;
                    Vector3 localScale = ch.transform.localScale;

                    // Scale walls and colliders
                    if (ch.tag == "Top") 
                    {
                        SetDroneCollider(ch, child, 0F, (LaneWidth + ColliderWallSize) / 2, nextLineWidth, WallSize);
                        SetWallOrPlayerCollider(ch, child, 0F, (LaneWidth + WallSize) / 2, nextLineWidth, WallSize);
                    }

                    else if (ch.tag == "Right")
                    {
                        SetDroneCollider(ch, child, (nextLineWidth - WallSize) / 2, 0F, ColliderWallSize, LaneWidth);
                    }
                    // scale individual 
                    
                    else if (ch.tag == "Bottom")
                    {
                        SetDroneCollider(ch, child, 0F, -(LaneWidth  - WallSize ) / 2, nextLineWidth, ColliderWallSize);
                        SetWallOrPlayerCollider(ch, child, -WallSize/2, -(LaneWidth  + WallSize) / 2, nextLineWidth + WallSize, ColliderWallSize);
                    }

                    else if (ch.tag == "Left")
                    {
                        SetWallOrPlayerCollider(ch, child, -(nextLineWidth + WallSize) / 2, WallSize/2, WallSize, LaneWidth+WallSize);
                        SetDroneCollider(ch, child, -(nextLineWidth + WallSize) / 2, 0F, WallSize, LaneWidth);
                    }

                    // Scale ground
                    // suitable for standards, 2nd of center
                    if (ch.tag == "Ground" || ch.tag == "SafeZone")
                    {
                        SetWallOrPlayerCollider(ch, child, 0F, 0F, nextLineWidth, LaneWidth);
                    }
                }
            }

            // Set position and rotation of the lane
            PlatformPrefab.transform.position = Pos - LanePrefab.transform.right * (LaneLength + nextLineWidth) / 2;
            PlatformPrefab.transform.localEulerAngles = Rot;
        }

        protected void SetDroneCollider(Transform collider, Transform colliderParent, float posX, float posZ, float scX, float scZ)
        {  
            if (colliderParent.name == "DroneCollider")
            {
                collider.transform.localScale = new Vector3(scX, collider.transform.localScale.y, scZ);
                collider.transform.localPosition = new Vector3(posX, collider.transform.localPosition.y, posZ);
            }
        }

        protected void SetWallOrPlayerCollider(Transform obj, Transform objParent, float posX, float posZ, float scX, float scZ)
        {
            if (objParent.name != "DroneCollider")
            {
                obj.transform.localScale = new Vector3(scX, obj.transform.localScale.y, scZ);
                obj.transform.localPosition = new Vector3(posX, obj.transform.localPosition.y, posZ);
            }
        }
    }
}
