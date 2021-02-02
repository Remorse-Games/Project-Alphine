using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastBoss
{
    public abstract class StateActions : ScriptableObject
    {
        public string nameObject;
        public abstract void Execute(AIBehaviour states);
    }
}
