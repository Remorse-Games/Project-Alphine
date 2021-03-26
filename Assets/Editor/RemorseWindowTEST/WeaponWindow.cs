using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

/*  Our Remorse Window Framework */
using RemorseWindow;

namespace Remorse.Tools.RPGDatabaseTest
{
    public class WeaponWindow : BaseControll
    {
        
        public WeaponWindow(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {   
            panel1 = new Panel(currentEditorWindow, this, "Weapon Panel", new Rect(0, 0, 300, 600) );
            panel2 = new Panel(currentEditorWindow, this, "Weapon Panel2", new Rect( panel1.rect.width + 5, 0, 100, 600) );
            
            button1 = new Button(currentEditorWindow, panel2, "RED", 
                        new Rect( 0, 20, 100, 70) );
            
            /* Here Add some events for button and the function */
            button1.AddEvent(Button.ButtonEvent.ONCLICK, button1_OnClick);
            button1.guiStyle.normal.background = button1.GetRedTexture;
            button1.guiStyle.normal.textColor  = Color.white;
            
            button2 = new Button(currentEditorWindow, panel2, "GREEN", 
                        new Rect( 0, 90, 100, 70) );
                                    /* Here Add some events for button and the function */
            button2.AddEvent(Button.ButtonEvent.ONCLICK, button2_OnClick);
            button2.guiStyle.normal.background = button1.GetGreenTexture;
            button2.guiStyle.normal.textColor  = Color.white;
                        
            button3 = new Button(currentEditorWindow, panel2, "BLUE", 
                        new Rect( 0, 160, 100, 70) );
                                    /* Here Add some events for button and the function */
            button3.AddEvent(Button.ButtonEvent.ONCLICK, button3_OnClick);
            button3.guiStyle.normal.background = button1.GetBlueTexture;
            button3.guiStyle.normal.textColor  = Color.white;
                        
            button4 = new Button(currentEditorWindow, panel2, "GRAY", 
                        new Rect( 0, 230, 100, 70) );
                                    /* Here Add some events for button and the function */
            button4.AddEvent(Button.ButtonEvent.ONCLICK, button4_OnClick);
            button4.guiStyle.normal.background = button1.GetGrayTexture;
            button4.guiStyle.normal.textColor  = Color.white;          
                    
            dropdown1 =  new DropDown(currentEditorWindow, panel2, "DropDown", 
                                new Rect( 0 , button4.rect.y + button4.rect.height + 20, 100, 25) );
            
            List<String> test = new List<String>();
            test.Add("RED");
            test.Add("GREEN");
            test.Add("BLUE");
            test.Add("GRAY");
            
            dropdown1.SetListData(test);
            dropdown1.AddEvent(DropDown.DropDownEvent.ONSELECTED, dropdown1_OnSelected);        
                    
            traitPanel1 = new TraitPanel(currentEditorWindow, panel2, "Trait Panel", 
                        new Rect( panel2.rect.width + 10, 0, 50, 50) );        
                 
                 
            listDraw =  new List<BaseControll>();
            listDraw.Add( panel1 );
            listDraw.Add( panel2 );
            listDraw.Add( button1 );
            listDraw.Add( button2 );
            listDraw.Add( button3 );
            listDraw.Add( button4 );
            listDraw.Add( dropdown1 );
            listDraw.Add( traitPanel1 );
        }
        
        Panel panel1;
        Panel panel2;
        Button button1;
        Button button2;
        Button button3;
        Button button4;
        
        DropDown dropdown1;
        TraitPanel traitPanel1;
        
        /* Button OnCLick Event */
        public void button1_OnClick()
        {
            panel1.guiStyle.normal.background = panel1.GetRedTexture;
        }
                /* Button OnCLick Event */
        public void button2_OnClick()
        {
            panel1.guiStyle.normal.background = panel1.GetGreenTexture;
        }
                /* Button OnCLick Event */
        public void button3_OnClick()
        {
            panel1.guiStyle.normal.background = panel1.GetBlueTexture;
        }
                /* Button OnCLick Event */
        public void button4_OnClick()
        {
            panel1.guiStyle.normal.background = panel1.GetGrayTexture;
        }
        
        /* dropdown1_OnSelected Event */
        public void dropdown1_OnSelected()
        {
            switch( dropdown1.selectedList )
            {
                case 0:
                    panel1.guiStyle.normal.background = panel1.GetRedTexture;
                    break;
                    
                case 1:
                     panel1.guiStyle.normal.background = panel1.GetGreenTexture;
                    break;
                    
                case 2:
                    panel1.guiStyle.normal.background = panel1.GetBlueTexture;
                    break;
                    
                case 3:
                     panel1.guiStyle.normal.background = panel1.GetGrayTexture;
                    break;
                
            }
      
        }
    }
}