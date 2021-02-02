﻿using UnityEngine;

namespace LastBoss.AI
{
    [CreateAssetMenu(menuName = "AI/AI Movement")]
    public class EnemyMovement : StateActions
    {
        public override void Execute(AIBehaviour states)
        {
            states.Patrolling();
        }
    }
}