using UnityEngine;

namespace Remorse.AI
{
    /* note */
    /* Edited by Imandana */
    public class Chase : StateActions
    {
        public override void Execute(AIBehaviour states)
        {
            states.ExecuteAI();
        }
    }
}
