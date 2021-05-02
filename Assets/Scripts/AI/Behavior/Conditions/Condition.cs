using UnityEngine;

namespace Remorse.AI
{
    /* Edited by Imandana */
    /* This is a base class of Scriptable object for All derivated Condition */
    /* Like InCombat, InSight, etc Condition */
    public abstract class Condition : ScriptableObject
    {
		public string description;
        public abstract bool CheckCondition(AIBehaviour state);

    }
}
