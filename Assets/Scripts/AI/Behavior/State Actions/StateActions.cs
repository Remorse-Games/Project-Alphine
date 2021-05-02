using UnityEngine;

namespace Remorse.AI
{
     /* Edited by Imandana */
    /* This is a base class of Scriptable object for All derivated StateActions */
    /* Like Attack, Chase, etc Action */
    public abstract class StateActions : ScriptableObject
    {
        public string nameObject;
        public abstract void Execute(AIBehaviour states);
    }
}
