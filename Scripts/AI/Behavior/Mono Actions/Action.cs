using UnityEngine;

namespace Remorse.AI
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Execute();
    }
}
