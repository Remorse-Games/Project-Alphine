using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class Button : BaseControll
    {
        public enum ButtonEvent {ONCLICK =0, ONKEYUP, ONKEYDOWN, SUM};
        public Button(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {
            listEvents = new List<Action>( (int)ButtonEvent.SUM ) {null, null, null  };
            condition = new List<bool>((int)ButtonEvent.SUM) { false, false, false };
        }
        
        public void AddEvent(ButtonEvent type, Action func)
        {
            listEvents[(int)type] = func;
        }
        
        private void OnClickMethod()
        {
            if(condition[(int)ButtonEvent.ONCLICK])
                listEvents[(int)ButtonEvent.ONCLICK]();
            
            condition[(int)ButtonEvent.ONCLICK] = false;
        }
        
        protected override void ExecuteEvents()
        {
            if(listEvents[(int)ButtonEvent.ONCLICK] != null)
                OnClickMethod();
        }
        
        public override void Draw()
        {
            GUILayout.BeginArea(rect);
            condition[(int)ButtonEvent.ONCLICK] = GUILayout.Button(name, GUILayout.Width(rect.width), GUILayout.Height(rect.height));
            GUILayout.EndArea();
            
            
            ExecuteEvents();
            
        }
    }
}