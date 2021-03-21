using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class BaseControll
    {
        public BaseControll(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        {
            m_editorWindow = currentEditorWindow;
            this.parent = parent;
            this.name =  name;
            
            this.rect =  (this.parent != null)? 
                    new Rect( parent.rect.position + rect.position, rect.size) : rect;
                    
            listOptions = new List<GUILayoutOption>();
            guiContent  = new GUIContent();
            
            listOptions.Add( GUILayout.Width(rect.width) );
            listOptions.Add( GUILayout.Height(rect.height) );
            text = name;
            options = listOptions.ToArray();
            guiContent.text = text;
            
            isActive = true;
        }
        
        public String               name;
        public String               text;
        
        public bool                 isActive;
        public EditorWindow         m_editorWindow ;

        public Rect                 rect;
        public BaseControll         parent;
        
        public GUIContent           guiContent;
        public GUIStyle             guiStyle;
        protected GUILayoutOption[]    options;
        public List<GUILayoutOption>   listOptions;
        
        protected List<bool>        condition;
        public List<Action>         listEvents;
        public List<BaseControll>   listDraw;
        
        public virtual void Draw()
        { 
            for(int i = 0; i < listDraw.Count; i++)
            {
                if( listDraw[i].isActive )
                    listDraw[i].Draw();
            }
        }
        protected virtual void ExecuteEvents()
        { }
    }
}
