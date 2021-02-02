using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastBoss
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Execute();
    }
}
