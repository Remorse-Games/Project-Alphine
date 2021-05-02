using UnityEngine;

namespace Remorse.AI
{
    /* note */
    /* Edited by Imandana */
    public class Movement : StateActions
    {
        public float moveSpeed;
        public float runSpeed;
        public float rotateSpeed;
        
        public override void Execute(AIBehaviour states)
        {
            states.ExecuteAI();
        }
    }
}
