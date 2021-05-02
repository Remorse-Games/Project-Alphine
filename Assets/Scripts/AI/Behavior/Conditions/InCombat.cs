using UnityEngine;

namespace Remorse.AI
{
    /* Edited by Imandana */
    public class InCombatArea : Condition
    {
        public override bool CheckCondition(AIBehaviour state)
        {
            AICombat st = (AICombat)state;
            return st.IsInRange( st.distanceForCombat );
        }
    }
}
