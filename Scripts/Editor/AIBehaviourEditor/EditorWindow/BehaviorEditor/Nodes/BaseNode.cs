using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System;

#if UNITY_EDITOR
namespace LastBoss.BehaviorEditor
{
    [Serializable]
    public class BaseNode 
    {
        public int id;
        public DrawNode drawNode;
        public Rect windowRect;
        public string windowTitle;
        public int enterNode;
        public int targetNode;
        public bool isDuplicate;
        public string comment;
        public bool isAssigned;
		public bool showDescription;
		public bool isOnCurrent;

        public bool collapse;
		public bool showActions = true;
		public bool showEnterExit = false;
        [HideInInspector]
        public bool previousCollapse;

        [SerializeField]
        public StateNodeReferences stateRef;
        [SerializeField]
        public TransitionNodeReferences transRef;

        public void DrawWindow()
        {
            if(drawNode != null)
            {
                drawNode.DrawWindow(this);
            }
        }

        public void DrawCurve()
        {
            if (drawNode != null)
            {
                drawNode.DrawCurve(this);
            }
        }

    }

    [Serializable]
    public class StateNodeReferences
    { 
    //    [HideInInspector]
        public State currentState;
        [HideInInspector]
        public State previousState;
		public SerializedObject serializedState;
	    public ReorderableList onFixedList;
		public ReorderableList onUpdateList;
		public ReorderableList onEnterList;
		public ReorderableList onExitList;
	}

	[Serializable]
    public class TransitionNodeReferences
    {
        [HideInInspector]
        public Condition previousCondition;
        public int transitionId;
    }
}
#endif