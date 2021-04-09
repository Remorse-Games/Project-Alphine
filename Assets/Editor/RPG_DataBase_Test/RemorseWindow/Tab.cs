using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class Tab : BaseControll
    {
        public enum Mode { LEFTVERTICAL, RIGHTVERTICAL, TOPHORIZONTAL, BOTTOMHORIZONTAL, SUM};
        public Tab(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect, Rect contentRect)
        : base(currentEditorWindow, parent, name, rect)
        {   
            theWindows = new List<BaseControll>();
            listWinName =  new List<String>();
            tabHeader = new TabHeader( currentEditorWindow, this, name + " Header", rect );
            
            this.contentRect = contentRect;
            tabHeader.Init(listWinName);
        }

        private List<BaseControll> theWindows;
        private List<String> listWinName;
        
        public Rect contentRect;
        public TabHeader tabHeader;
        
        public void AddTabWindow(BaseControll window)
        {
            theWindows.Add(window);
            listWinName.Add(window.name);
        }
        
        public override void Draw()
        {
            tabHeader.Draw();
            theWindows[ tabHeader.selectedTab ].Draw();
        }
        
        /* INNER CLASS */
            public class TabHeader : BaseControll
            {
                public TabHeader(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
                : base(currentEditorWindow, parent, name, rect)
                {
                }
                
                public List<String> winNames;
                public int selectedTab;
                
                public void Init(List<String> listWinName)
                {
                    winNames = listWinName;
                    selectedTab = 0;
                    
        /*             rect.width = rect.width*0.5f;
                    rect.height = rect.height*0.2f; */
                }
                
                public override void Draw()
                {
                    GUILayout.BeginVertical("Box");
                    selectedTab = GUILayout.SelectionGrid(selectedTab, winNames.ToArray(), 1, GUILayout.Width(rect.width), GUILayout.Height(rect.height));
                    GUILayout.EndVertical();
                }
            }
    }
}