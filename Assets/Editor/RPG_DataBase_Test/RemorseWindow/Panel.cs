using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class Panel : BaseControll
    {
        public Panel(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {
        }
        
        public override void Draw()
        {
            GUILayout.BeginArea(rect);
            GUILayout.Box(name, GUILayout.Width(rect.width), GUILayout.Height(rect.height));
            GUILayout.EndArea();
        }
    }
}