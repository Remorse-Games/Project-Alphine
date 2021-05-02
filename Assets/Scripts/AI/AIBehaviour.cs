using UnityEngine;
using System.Collections.Generic;
using Remorse.Utility;

namespace Remorse.AI
{
    /* note */
    /* Edited by Imandana */
    public class AIBehaviour : MonoBehaviour
    {
        public GameObject aiObject;
        public GameObject [] othersObject;
        
        public State currentState;
        
        public float radius;
        public float fov;
        
        public virtual void ExecuteAI()
        {}
        
        public int GetVisibleTarget()
        {
            return 0;
        }        
        
    }
}
