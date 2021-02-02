using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastBoss.AI
{
    [CreateAssetMenu(menuName ="AI/Condition/Sight")]
    public class EnemySight : Condition
    {
        public bool isEnemyVisible;
        public override bool CheckCondition(AIBehaviour state)
        {
            if (isEnemyVisible)
            {
                if (state.fov.visibleTarget.Count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                if (state.fov.visibleTarget.Count > 0)
                    return false;
                else
                    return true;
            }
        }
    }
}