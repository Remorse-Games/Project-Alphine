using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

/*  Our Remorse Window Framework */
using RemorseWindow;

namespace Remorse.Tools.RPGDatabaseTest
{
    public class ClassWindow : BaseControll
    {
        
        public ClassWindow(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {   
            panel1 = new Panel(currentEditorWindow, this, "Classes Panel", new Rect(0, 0, 900, 600) );
            
            button1 = new Button(currentEditorWindow, panel1, "Click Me, Button Classes", 
                        new Rect( panel1.rect.width*0.5f - 70, panel1.rect.height*0.5f - 45, 190, 70) );
            
            /* Here Add some events for button and the function */
            button1.AddEvent(Button.ButtonEvent.ONCLICK, button1_OnClick);
            
            textboxt1 = new TextBox(currentEditorWindow, panel1, "This is a Free TextBoxt", 
                        new Rect( panel1.rect.width*0.5f - 70, panel1.rect.height*0.5f + 50, 140, 20) );
                        
            textboxt2 = new TextBox(currentEditorWindow, panel1, "This is a INT TextBoxt", 
                        new Rect( panel1.rect.width*0.5f - 70, panel1.rect.height*0.5f + 150, 140, 20) );
            textboxt2.mode = TextBox.Mode.INTFIELD;
            
            textboxt3 = new TextBox(currentEditorWindow, panel1, "TextBox for Change", 
                        new Rect( panel1.rect.width*0.5f - 70, panel1.rect.height*0.5f + 250, 140, 20) );
                        
                        
            listDraw =  new List<BaseControll>();
            listDraw.Add( panel1 );
            listDraw.Add( button1 );
            listDraw.Add( textboxt1 );
            listDraw.Add( textboxt2 );
            listDraw.Add( textboxt3);
        }
        
        Panel panel1;
        Button button1;
        TextBox textboxt1;
        
        TextBox textboxt2;
        TextBox textboxt3;
        
        /* Button OnCLick Event */
        public void button1_OnClick()
        {
            textboxt1.text = textboxt3.text;
        }
        
    }
}