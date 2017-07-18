using System.Collections.Generic;
using Characters.Types;
using UnityEngine;

namespace Launcher
{
    public class MapState
    {
        public List<GameObject> SafeZones;
        public bool[] VisitedSafeZones;

        public MapState()
        {
        }
    }
}
