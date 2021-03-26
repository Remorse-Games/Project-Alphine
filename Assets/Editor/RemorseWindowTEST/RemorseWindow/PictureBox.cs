using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class PictureBox : BaseControll
    {
        public PictureBox(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {
            guiStyle = new GUIStyle("box");
        }
        
        public override void Draw()
        {
            GUILayout.BeginArea(rect);
            GUILayout.Box(guiContent, guiStyle,  options);
            GUILayout.EndArea();
            
            ExecuteEvents(); 
        }
    }
}