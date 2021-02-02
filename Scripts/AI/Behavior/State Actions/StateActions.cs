using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remorse.AI
{
    public abstract class StateActions : ScriptableObject
    {
        public string nameObject;
        public abstract void Execute(AIBehaviour states);
    }
}
