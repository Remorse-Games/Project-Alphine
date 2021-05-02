using UnityEngine;
using System.Collections.Generic;
using Remorse.Utility;

namespace Remorse.AI
{
    public class AICombat : AIBehaviour
    {
        [HideInInspector] public float distanceForCombat;
        [HideInInspector] public float combatCooldown;
        
        public GameObject bullet;
        public Transform spawnBullet;

        [HideInInspector] public int index;
        [HideInInspector] public bool editPatrolArea;

        /* [HideInInspector] public FieldOfView fieldOfView; */
        private float tempCooldown;
        
        public bool IsInRange(float distance)
        {
            
            return false;
        }

        /* Execute Combat here */
        public override void ExecuteAI()
        {
            
        }
    }
}
