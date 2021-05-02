using UnityEngine;
using System.Collections.Generic;
using Remorse.Utility;

namespace Remorse.AI
{
    /* note */
    /* Edited by Imandana */
    public class Patrol : StateActions
    {
        public State currentState;
        public List<GridVector> patrolArea = new List<GridVector>();
        
        public override void Execute(AIBehaviour states)
        {
            states.ExecuteAI();
        }
    }
}
