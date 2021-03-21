using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class Button : BaseControll
    {
        /* Enum Replacement */
        public static class ButtonEvent
        {
            public const int ONCLICK = 0;
            public const int ONKEYUP = 1;
            public const int ONKEYDOWN = 2;
            public const int SUM = 3;
        }

        public Button(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {
            listEvents = new List<Action>( ButtonEvent.SUM ) {null, null, null  };
            condition = new List<bool>( ButtonEvent.SUM ) { false, false, false };
            
            guiStyle = new GUIStyle("button");
        }
        
        public void AddEvent(int type, Action func)
        {
            listEvents[type] = func;
        }
        
        private void OnClickMethod()
        {
            if(condition[ButtonEvent.ONCLICK])
                listEvents[ButtonEvent.ONCLICK]();
            
            condition[ButtonEvent.ONCLICK] = false;
        }
        
        protected override void ExecuteEvents()
        {
            if(listEvents[ButtonEvent.ONCLICK] != null)
                OnClickMethod();
        }
        
        public override void Draw()
        {
            GUILayout.BeginArea(rect);
            condition[ButtonEvent.ONCLICK] = GUILayout.Button(guiContent, guiStyle, options);
            GUILayout.EndArea();
                   
            ExecuteEvents();      
        }
    }
}
