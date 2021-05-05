using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remorse.Utility
{

    public static class Math
    {

        /// <summary>
        /// Get Vector Position by Angle
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="radius"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 GetPositionByAngle(float angle, float radius, float y = 0)
        {
            return new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                y,
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius
            );
        }

        public static GridVector VectorAbs(GridVector vector)
        {
            return new GridVector(
                Mathf.Abs(vector.x),
                Mathf.Abs(vector.y),
                Mathf.Abs(vector.z)
            );
        }

    }

}