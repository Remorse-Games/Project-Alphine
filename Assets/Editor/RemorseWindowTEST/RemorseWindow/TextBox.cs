using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class TextBox : BaseControll
    {
        /* Enum Replacement */
        public static class Mode
        {
            public const int NORMAL = 0;
            public const int INTFIELD = 1;
            public const int SUM = 2;
        }
        public TextBox(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {   
            guiStyle = new GUIStyle("textField");
            mode = Mode.NORMAL;
        }
        
        public int mode;
        private int intText = 0;
        
        private void NormalTextBox()
        {
            GUILayout.BeginArea(rect);
            text = GUILayout.TextField(text, guiStyle, options);
            GUILayout.EndArea();
        }
        private void IntTextBox()
        {
            GUILayout.BeginArea(rect);
            intText = EditorGUILayout.IntField( intText, guiStyle, options);
            GUILayout.EndArea();
            
            text = intText.ToString();
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
            
            ExecuteEvents(); 
        }
    }
}