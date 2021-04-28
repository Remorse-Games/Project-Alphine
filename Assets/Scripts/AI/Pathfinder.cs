using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remorse.AI
{
    public static class Pathfinder
    {

        public static Vector3[] FindPath(Vector3 currentPos, Vector3 destination)
        {
            Vector3[] result =
            {
                currentPos,
                new Vector3(currentPos.x, (currentPos.y + destination.z) / 2, destination.z),
                destination
            };

            return result;
        }

    }
}
