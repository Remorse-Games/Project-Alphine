using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remorse.AI
{
    public static class AIPathFinder
    {

        public static Vector3 FindPath(Vector3 currentPos, Vector3 destination)
        {
            return new Vector3(
                currentPos.x,
                currentPos.y,
                destination.z
            );
        }

    }
}
