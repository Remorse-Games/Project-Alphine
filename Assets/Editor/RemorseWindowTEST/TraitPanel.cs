using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

/*  Our Remorse Window Framework */
using RemorseWindow;

namespace Remorse.Tools.RPGDatabaseTest
{
    public class TraitPanel : BaseControll
    {
        
        public TraitPanel(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {   
            panel1 = new Panel(currentEditorWindow, this, "Trait Panel", new Rect(0, 0, 900, 600) );
            
            button1 = new Button(currentEditorWindow, panel1, "Delete All Data ", 
                        new Rect( panel1.rect.width*0.5f - 70, panel1.rect.height - 70, 190, 70) );
            
            /* Here Add some events for button and the function */
            button1.AddEvent(Button.ButtonEvent.ONCLICK, button1_OnClick);
            
            button1.guiStyle.normal.background = button1.GetRedTexture;
            button1.guiStyle.normal.textColor  = Color.white;
                               
            listDraw =  new List<BaseControll>();
            listDraw.Add( panel1 );
            listDraw.Add( button1 );

        }
        
        Panel panel1;
        Button button1;
        
        bool changing = true;
        /* Button OnCLick Event */
        public void button1_OnClick()
        {   
            if( changing )
            {
                panel1.guiStyle.normal.background = panel1.GetBlueTexture;
                changing = !changing;
            }
            else
            {
                panel1.guiStyle.normal.background = panel1.GetGreenTexture;
                changing = !changing;
            }
        }
        
    }
}