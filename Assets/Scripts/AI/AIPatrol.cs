using UnityEngine;
using System.Collections.Generic;
using Remorse.Utility;

namespace Remorse.AI
{
    public class AIPatrol : AIBehaviour
    {
        [HideInInspector] public List<GridVector> patrolArea = new List<GridVector>();

        [HideInInspector] public int index;
        [HideInInspector] public bool editPatrolArea;

        /* [HideInInspector] public FieldOfView fieldOfView; */
        private float tempCooldown;

        /* Execute Patrol here */
        public override void ExecuteAI()
        {
            
        }
        
#if UNITY_EDITOR
        /* Note from Imandana */
        /* Separating from base */
        private void OnDrawGizmosSelected()
        {
            if (!editPatrolArea)
            {
                // draw circle gizmo
                float defaultDeg = 360 / 36;

                for (int i = 0; i < 36; i++)
                {
                    float deg = defaultDeg * i;
                    float nextDeg = defaultDeg * (i == 36 ? 0 : i + 1);

                    Vector3 pos = Math.GetPositionByAngle(deg, radius, -0.5f);
                    Vector3 nextPos = Math.GetPositionByAngle(nextDeg, radius, -0.5f);

                    Gizmos.DrawLine(pos + transform.position, nextPos + transform.position);
                }

                // draw FOV
                defaultDeg = fov / 2;

                Vector3 rPos = Math.GetPositionByAngle(defaultDeg, radius, -0.5f);
                Vector3 lPos = Math.GetPositionByAngle(360 - defaultDeg, radius, -0.5f);

                Vector3 center = new Vector3(0, -0.5f, 0) + transform.position;

                Gizmos.DrawLine(center, rPos + transform.position);
                Gizmos.DrawLine(center, lPos + transform.position);
            }
        }

        private void OnDrawGizmos()
        {
            if (editPatrolArea && patrolArea.Count > 0)
            {
                Vector3 centerPoint = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

                Gizmos.DrawLine(
                    centerPoint,
                    patrolArea[0]
                );

                Gizmos.DrawLine(
                    patrolArea[patrolArea.Count - 1],
                    centerPoint
                );

                for (int i = 0; i < patrolArea.Count - 1; i++)
                {
                    Gizmos.DrawLine(patrolArea[i], patrolArea[i + 1]);
                }
            }
        }

#endif
        
    }
}
