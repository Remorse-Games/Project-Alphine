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
        }
        
        public String name;
        public EditorWindow m_editorWindow ;
       /*  public Vector2 position; */
        public Rect rect;
        public BaseControll parent;
        
        protected List<bool> condition;
        public List<Action> listEvents;
        
        public virtual void Draw()
        { }
        protected virtual void ExecuteEvents()
        {
        }
    }
}