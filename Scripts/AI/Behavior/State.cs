﻿using System.Collections.Generic;
using UnityEngine;

namespace LastBoss
{
    [CreateAssetMenu]
    public class State : ScriptableObject
    {
    	public StateActions[] onFixed;
        public StateActions[] onUpdate;
        public StateActions[] onEnter;
        public StateActions[] onExit;

        public int idCount;
        public List<Transition> transitions;

        public void OnEnter(AIBehaviour states)
        {
            ExecuteActions(states, onEnter);
        }
	
		public void FixedTick(AIBehaviour states)
		{
            ExecuteActions(states, onFixed);
		}

        public void Tick(AIBehaviour states)
        {
            ExecuteActions(states, onUpdate);
            CheckTransitions(states);
        }

        public void OnExit(AIBehaviour states)
        {
            ExecuteActions(states, onExit);
        }

        public void CheckTransitions(AIBehaviour states)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].disable)
                    continue;

                if (transitions[i].condition.CheckCondition(states))
                {
                    if (transitions[i].targetState != null)
                    {
                        states.currentState = transitions[i].targetState;
                        OnExit(states);
                        states.currentState.OnEnter(states);
                    }
                    return;
                }
            }
        }
        
        public void ExecuteActions(AIBehaviour states, StateActions[] l)
        {
            for (int i = 0; i < l.Length; i++)
            {                
                l[i].Execute(states);              
            }
        }

#if UNITY_EDITOR
        public Transition AddTransition()
        {
            Transition retVal = new Transition();
            transitions.Add(retVal);
            retVal.id = idCount;
            idCount++;
            return retVal;
        }

        public Transition GetTransition(int id)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].id == id)
                    return transitions[i];
            }

            return null;
        }

		public void RemoveTransition(int id)
		{
			Transition t = GetTransition(id);
			if (t != null)
				transitions.Remove(t);
		}
#endif
    }
}
