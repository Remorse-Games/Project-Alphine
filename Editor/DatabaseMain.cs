using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using SFB;

public class DatabaseMain : EditorWindow
{
    #region Init Values
    //Editor Classes
    ActorTab actorTab;
    ClassTab classTab;
    SkillsTab skillTab;
    ItemTab itemTab;
    WeaponTab weaponTab;
    ArmorTab armorTab;
    EnemyTab enemyTab;
    TroopTab troopTab;
    StateTab stateTab;
    TermTab termTab;

    //Tab Area. DO NOT CHANGE anything in here.
    public static float tabAreaWidth;
    public static float tabAreaHeight;

    //Here we setup the tab selection button.
    private int selectedTab = 0;
    private string[] tabNames = {
        "Actors",
        "Classes" ,
        "Skills",
        "Item",
        "Weapons",
        "Armors",
        "Enemies",
        "Troops",
        "States",
        "Animations",
        "Tilesets",
        "Common Events",
        "System",
        "Types",
        "Terms"
    };
    #endregion

    #region Init Function
    //Initialize the Editor.
    [MenuItem("Database/Database")]
    static void Init()
    {
        DatabaseMain dbMain = (DatabaseMain)EditorWindow.GetWindow(typeof(DatabaseMain));
        dbMain.minSize = new Vector2(1280f, 720f);
        dbMain.Show();
    }

    public void OnEnable()
    {
        ValueInit();
        BaseTab.FolderChecker();
        actorTab.Init();
        skillTab.Init();
        troopTab.Init();
        stateTab.Init();
        termTab.Init();
    }

    //////////////////////////////////////////////////

    /// <summary>
    /// OnGUI Method. Initialize all variables and function in here.
    /// </summary>
    private void OnGUI()
    {
        DBTab();
        TabSelection(selectedTab);        
    }

    //////////////////////////////////////////////////


    /// <summary>
    /// We Initialize all value that doesn't static in here.
    /// </summary>
    private void ValueInit()
    {
        actorTab = new ActorTab();
        classTab = new ClassTab();
        skillTab = new SkillsTab();
        itemTab = new ItemTab();
        weaponTab = new WeaponTab();
        armorTab = new ArmorTab();
        enemyTab = new EnemyTab();
        troopTab = new TroopTab();
        stateTab = new StateTab();
        termTab = new TermTab();
    }

    //////////////////////////////////////////////////

    /// <summary>
    /// Database Tab. Create selection grid so we can choose which tab is
    /// active currently.
    /// </summary>
    private void DBTab()
    {
        tabAreaWidth = position.width / 8;
        tabAreaHeight = position.height * .75f;
        
        #region  BoxUI
        GUILayout.BeginVertical("Box");
        selectedTab = GUILayout.SelectionGrid(selectedTab, tabNames, 1, GUILayout.Width(tabAreaWidth), GUILayout.Height(tabAreaHeight));
        GUILayout.EndVertical();
        #endregion
    }

    //////////////////////////////////////////////////

    /// <summary>
    /// Switch statement for tab selection.
    /// Maybe will refactor later.
    /// </summary>
    /// <param name="selectedTab"></param>
    private void TabSelection(int selectedTab)
    {
        switch (selectedTab)
        {
            case 14:
                termTab.OnRender(position);
                break;
            case 13:
                actorTab.OnRender(position);
                break;
            case 12:
                actorTab.OnRender(position);
                break;
            case 11:
                actorTab.OnRender(position);
                break;
            case 10:
                actorTab.OnRender(position);
                break;
            case 9:
                actorTab.OnRender(position);
                break;
            case 8:
                stateTab.OnRender(position);
                break;
            case 7:
                troopTab.OnRender(position);
                break;
            case 6:
                enemyTab.Init(position);
                break;
            case 5:
                armorTab.Init(position);
                break;
            case 4:
                weaponTab.Init(position);
                break;
            case 3:
                itemTab.Init(position);
                break;
            case 2:
                skillTab.OnRender(position);
                break;
            case 1:
                classTab.Init(position);
                break;
            case 0:
                actorTab.OnRender(position);
                break;
        }
    }

    #endregion
}
