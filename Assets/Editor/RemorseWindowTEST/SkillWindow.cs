using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

/*  Our Remorse Window Framework */
using RemorseWindow;

namespace Remorse.Tools.RPGDatabaseTest
{
    public class SkillWindow : BaseControll
    {
        
        public SkillWindow(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {
            
            panel1 = new Panel(currentEditorWindow, this, "Skill Panel", new Rect(0, 0, 100, 100) );
            panel2 = new Panel(currentEditorWindow, this, "Skill Panel 2",
                                      new Rect(panel1.rect.width , 0, 400, 100) );
            panel3 = new Panel(currentEditorWindow, this, "Skill Panel 3",
                                      new Rect(panel1.rect.width + panel2.rect.width , 0, 100, 100) );
                                      
            label1 =  new Label(currentEditorWindow, panel3, "Im Label", new Rect(0, 20, 100, 100) );
            dropdown1 =  new DropDown(currentEditorWindow, panel3, "DropDown", new Rect(0, 20, 100, 25) );
            
            List<String> test = new List<String>();
            test.Add("FIRE");
            test.Add("WATER");
            test.Add("EARTH");
            
            dropdown1.SetListData(test);
            dropdown1.AddEvent(DropDown.DropDownEvent.ONSELECTED, dropdown1_OnSelected);
            
            button1 = new Button(currentEditorWindow, panel1, "Click Me", 
                        new Rect( 0, 20, 100, 30) );
            
            /* Here Add some events for button and the function */
            button1.AddEvent(Button.ButtonEvent.ONCLICK, button1_OnClick);
            
            textboxt1 = new TextBox(currentEditorWindow, panel1, "Whatss!", 
                        new Rect( 0, 50, 140, 20) );
            textboxt2 = new TextBox(currentEditorWindow, panel2, "AUTO", 
                        new Rect( 0, 50, 140, 20) );
                        
            listDraw = new List<BaseControll>();
            listDraw.Add( panel1 );
            listDraw.Add( panel2 );
            listDraw.Add( panel3);
            listDraw.Add( label1 );
            listDraw.Add( dropdown1 );
            listDraw.Add( button1);
            listDraw.Add( textboxt1 );
            listDraw.Add( textboxt2 );
        }
        Panel panel1;
        Panel panel2;
        Panel panel3;
        
        Label label1;
        DropDown dropdown1;
        
        Button button1;
        TextBox textboxt1;
        TextBox textboxt2;
        
        /* Button OnCLick Event */
        public void button1_OnClick()
        {
            textboxt1.text = dropdown1.text;
        }
        
        /* DropDown OnSelected Event */
        public void dropdown1_OnSelected()
        {
            textboxt2.text = dropdown1.text;
            Debug.Log("Selected");
        }
    }
}
