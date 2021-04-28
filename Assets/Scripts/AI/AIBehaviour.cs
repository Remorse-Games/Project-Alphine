using UnityEngine;
using System.Collections.Generic;
using Remorse.Utility;

namespace Remorse.AI
{
    public class AIBehaviour : MonoBehaviour
    {
        [Header("Character")]
        public float health;

        [Header("Movement")]
        public float moveSpeed;
        public float runSpeed;
        public float rotateSpeed;

        [Header("Combat")]
        public float distanceForCombat;
        public float combatCooldown;
        public GameObject bullet;
        public Transform spawnBullet;

        [Header("Patrol")]
        public State currentState;
        public List<Vector3> patrolArea = new List<Vector3>();

        public float radius;
        public float fov;

        [HideInInspector] public int index;
        [HideInInspector] public bool editPatrolArea;

        //[HideInInspector] public FieldOfView fov;
        private float tempCooldown;

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void Patrolling()
        {
        }

        public void Chase()
        {
        }

        public bool IsInRange(float distance)
        {
            return true;
        }

        public void Combat()
        {

        }

        private void Attack()
        {

        }

#if UNITY_EDITOR

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
