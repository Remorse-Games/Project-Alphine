using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remorse.BehaviorEditor
{
    [CreateAssetMenu(menuName ="Editor/Settings")]
    public class EditorSettings : ScriptableObject
    {
        public BehaviorGraph currentGraph;
        public StateNode stateNode;
		public PortalNode portalNode;
        public TransitionNode transitionNode;
        public CommentNode commentNode;
        public bool makeTransition;
        public GUISkin skin;
		public GUISkin activeSkin;
        
         private void OnEnable()
        {
            /* Check if there is no assets data, then create a new Instance */
            
            skin = Resources.Load("GUISkin") as GUISkin;
            if(skin==null)
            skin = (GUISkin)ScriptableObject.CreateInstance( typeof(GUISkin) );
        
            activeSkin = skin;
            
            currentGraph = Resources.Load("BehaviorGraph") as BehaviorGraph;
            if(currentGraph==null)
            currentGraph = (BehaviorGraph)ScriptableObject.CreateInstance( typeof(BehaviorGraph) );
        
            stateNode = Resources.Load("StateNode") as StateNode;
            if(stateNode==null)
            stateNode = (StateNode)ScriptableObject.CreateInstance( typeof(StateNode) );
        
            portalNode = Resources.Load("PortalNode") as PortalNode;
            if(portalNode==null)
            portalNode = (PortalNode)ScriptableObject.CreateInstance( typeof(PortalNode) );
        
            transitionNode = Resources.Load("TransitionNode") as TransitionNode;
            if(transitionNode==null)
            transitionNode = (TransitionNode)ScriptableObject.CreateInstance( typeof(TransitionNode) );
        
            commentNode = Resources.Load("CommentNode") as CommentNode;
            if(commentNode==null)
            commentNode = (CommentNode)ScriptableObject.CreateInstance( typeof(CommentNode) );
            
            makeTransition = false;

		}
        
        public BaseNode AddNodeOnGraph(DrawNode type, float width,float height, string title, Vector3 pos)
        {
            BaseNode baseNode = new BaseNode();
            baseNode.drawNode = type;
            baseNode.windowRect.width = width;
            baseNode.windowRect.height = height;
            baseNode.windowTitle = title;
            baseNode.windowRect.x = pos.x;
            baseNode.windowRect.y = pos.y;
            currentGraph.windows.Add(baseNode);
            baseNode.transRef = new TransitionNodeReferences();
            baseNode.stateRef = new StateNodeReferences();
            baseNode.id = currentGraph.idCount;
            currentGraph.idCount++;
            return baseNode;
        }
    }
}
