using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

/*  Our Remorse Window Framework */
using RemorseWindow;

namespace Remorse.Tools.RPGDatabaseTest
{
    public class DatabaseMainTest : EditorWindow
    {
        Tab tab;
        public void OnEnable()
        {
            tab = new Tab(this, null, "MAIN TAB", 
                                new Rect(0,0 , position.width*0.5f, position.height*0.8f),
                                new Rect(position.width*0.5f + 20.0f,0 ,180 , position.height) );
            
            /* Add Your Window Class Here */
            tab.AddTabWindow( new ActorWindow(this, tab, "Actors Tab", tab.contentRect ) );
            tab.AddTabWindow( new ClassWindow(this, tab, "Classes Tab", tab.contentRect)  );
            tab.AddTabWindow( new SkillWindow(this, tab, "Skills Tab", tab.contentRect)  );
            tab.AddTabWindow( new ItemsWindow(this, tab, "Items Tab", tab.contentRect)  );
            tab.AddTabWindow( new WeaponWindow(this, tab, "Weapon Tab", tab.contentRect)  );
            
        }
        private void OnGUI()
        {
            tab.Draw();
        }
    }
    
    public class Main
    {
        [MenuItem("Remorse/RemorseWindow TEST")]
        public static void InitWindow()
        {
            DatabaseMainTest dbMain = ScriptableObject.CreateInstance<DatabaseMainTest>();
            dbMain.minSize = new Vector2(1280f, 720f);
            dbMain.Show();
        }
        
    }
}