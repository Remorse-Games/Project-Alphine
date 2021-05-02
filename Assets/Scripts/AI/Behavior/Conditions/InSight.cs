using UnityEngine;

namespace Remorse.AI
{
    /* Edited by Imandana */
    public class InSight : Condition
    {
        public override bool CheckCondition(AIBehaviour state)
        {
            return state.GetVisibleTarget() > 0;
        }
    }
}