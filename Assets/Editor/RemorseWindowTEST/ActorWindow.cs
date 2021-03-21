using System;

using UnityEngine;
using UnityEditor;

/*  Our Remorse Window Framework */
using RemorseWindow;

namespace Remorse.Tools.RPGDatabaseTest
{
    public class ActorWindow : BaseControll
    {
        
        public ActorWindow(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        : base(currentEditorWindow, parent, name, rect)
        {
            
            panel1 = new Panel(currentEditorWindow, this, "Actors Panel", new Rect(0, 0, 100, 100) );
            panel2 = new Panel(currentEditorWindow, this, "Actors Panel 2",
                                      new Rect(panel1.rect.width , 0, 400, 100) );
            panel3 = new Panel(currentEditorWindow, this, "Actors Panel 3",
                                      new Rect(panel1.rect.width + panel2.rect.width , 0, 100, 100) );
        }
        Panel panel1;
        Panel panel2;
        Panel panel3;
        
        public override void Draw()
        {
            panel1.Draw();
            panel2.Draw();
            panel3.Draw();
        }
    }
}