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
    }
}
