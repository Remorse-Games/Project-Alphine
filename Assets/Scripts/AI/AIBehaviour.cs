using UnityEngine;

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
        public GameObject bullet;
        public Transform spawnBullet;
        public float combatCooldown;

        [Header("Patrol")]
        public State currentState;
        public Transform[] patrolArea;
        [HideInInspector] public int index;

        [Header("Vision")]
        [Range(0, 20)] public float radius;
        [Range(0, 360)] public float fov;

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

        private void OnDrawGizmosSelected()
        {
            // draw circle gizmo
            float defaultDeg = 360 / 36;

            for (int i = 0; i < 36; i++)
            {
                float deg = defaultDeg * i * Mathf.Deg2Rad;
                float nextDeg = defaultDeg * (i == 36 ? 0 : i + 1) * Mathf.Deg2Rad;

                Vector3 pos = new Vector3(
                    Mathf.Sin(deg) * radius,
                    0,
                    Mathf.Cos(deg) * radius
                );

                Vector3 nextPos = new Vector3(
                    Mathf.Sin(nextDeg) * radius,
                    0,
                    Mathf.Cos(nextDeg) * radius
                );

                Gizmos.DrawLine(pos + transform.position, nextPos + transform.position);
            }

            // draw FOV
            defaultDeg = fov / 2;

            Vector3 rPos = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * defaultDeg) * radius,
                0,
                Mathf.Cos(Mathf.Deg2Rad * defaultDeg) * radius
            );

            Vector3 lPos = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * (360 - defaultDeg)) * radius,
                0,
                Mathf.Cos(Mathf.Deg2Rad * (360 - defaultDeg)) * radius
            );

            Gizmos.DrawLine(transform.position, rPos + transform.position);
            Gizmos.DrawLine(transform.position, lPos + transform.position);
        }
    }
}
