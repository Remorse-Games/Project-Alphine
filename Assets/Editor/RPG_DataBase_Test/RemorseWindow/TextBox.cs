using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class TextBox : BaseControll
    {
        public enum Mode { NORMAL, INTFIELD};
        
        public TextBox(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {
            text = name;
            mode = Mode.NORMAL;
        }
        
        public int textInt;
        public String text;
        public Mode mode;
        
        private void NormalTextBox()
        {
            GUILayout.BeginArea(rect);
            text = GUILayout.TextField(text, GUILayout.Width(rect.width), GUILayout.Height(rect.height));
            GUILayout.EndArea();
        }
        private void IntTextBox()
        {
            GUILayout.BeginArea(rect);
            textInt = EditorGUILayout.IntField(textInt, GUILayout.Width(rect.width), GUILayout.Height(rect.height));
            GUILayout.EndArea();
        }
        
        
        public override void Draw()
        {
            switch(mode)
            {
                case Mode.NORMAL:
                    NormalTextBox();
                break;
                case Mode.INTFIELD:
                    IntTextBox();
                break;
            }
        }
    }
}