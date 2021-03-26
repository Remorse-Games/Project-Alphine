using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class DropDown : BaseControll
    {
        public static class DropDownEvent
        {
            public const int ONSELECTED = 0;
            public const int SUM = 1;
        }
        public DropDown(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {
            listEvents = new List<Action>( DropDownEvent.SUM ) { null };
            condition = new List<bool>( DropDownEvent.SUM ) { false };
        }
        
        public List<String> listNames;
        public int selectedList;
        
        public void AddEvent(int type, Action func)
        {
            listEvents[type] = func;
        }
        
        private void OnSelectedMethod()
        {
            if(condition[ DropDownEvent.ONSELECTED ])
                listEvents[ DropDownEvent.ONSELECTED ]();
            
            condition[ DropDownEvent.ONSELECTED ] = false;
        }
        
        protected override void ExecuteEvents()
        {
            if(listEvents[ DropDownEvent.ONSELECTED ] != null)
                OnSelectedMethod();
        }
        
        public void SetListData(List<String> listNames)
        {
            this.listNames = listNames;
            selectedList = 0;
        }
        
        public override void Draw()
        {
            GUILayout.BeginArea(rect);
            selectedList = EditorGUILayout.Popup(selectedList, listNames.ToArray(), options);
            GUILayout.EndArea();
            
            condition[ DropDownEvent.ONSELECTED ] = text == listNames[ selectedList ] ? false : true;
            
            text = listNames[ selectedList ];
            ExecuteEvents();
        }
    }
}
        