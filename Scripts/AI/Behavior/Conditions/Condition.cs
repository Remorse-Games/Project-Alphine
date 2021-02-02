using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remorse.AI
{
    public abstract class Condition : ScriptableObject
    {
		public string description;

        public abstract bool CheckCondition(AIBehaviour state);

    }
}
