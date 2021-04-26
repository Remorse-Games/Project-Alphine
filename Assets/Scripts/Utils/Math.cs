using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remorse.Math
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
                Mathf.Sin(angle) * radius,
                y,
                Mathf.Cos(angle) * radius
            );
        }

    }

}