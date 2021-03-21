using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class Label : BaseControll
    {
        public Label(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {
            guiStyle = new GUIStyle("label");
        }
        
        public override void Draw()
        {
            GUILayout.BeginArea(rect);
            GUILayout.Label(guiContent, guiStyle, options);
            GUILayout.EndArea();
            
            ExecuteEvents(); 
        }
    }
}