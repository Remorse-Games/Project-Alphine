using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;

public class SkillsTab
{
    //All GUIStyle variable initialization.
    GUIStyle skillStyle;
    //Having list of all skills exist in data.
    public List<SkillData> skill = new List<SkillData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in SkillData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> skillDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    public string[] skillTypeList =
    {
        "None",
        "Magic",
        "Special",
    };


    public string[] skillScopeList =
    {
        "None",
        "1 Enemy",
        "All Enemies",
        "1 Random Enimies",
        "2 Random Enimies",
        "3 Random Enimies",
        "4 Random Enimies",
        "1 Ally",
        "All Allies",
        "1 Ally (Dead)",
        "The Allies (Dead)",
        "The User",
    };

    public string[] skillOccasion =
    {
        "Always",
        "Battle Screen",
        "Menu Screen",
        "Never",
    };

    public string[] skillHitType =
    {
        "Certain Hit",
        "Pyhsical Hit",
        "Magical Hit",
    };

    public string[] skillAnimation =
    {
        "Normal Attack",
        "None",
        "Hit Pyhsical",
        "...",
    };

    public string[] skillWeaponType =
    {
        "None",
        "Dagger",
        "Sword",
        "...",
    };
    //Index for selected Class.
    public int selectedClassIndex;

    //How many skill in ChangeMaximum Func
    public int skillSize;

    //i don't know about this but i leave this to handle later.
    int index = 0;
    int indexTemp = -1;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;
    Vector2 equipmentScrollPos = Vector2.zero;
    Vector2 traitsScrollPos = Vector2.zero;

    //Image Area.
    Texture2D Icon;

    public int skillSizeTemp;
    public void Init(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in SkillsTab itself.
        ///So make sure you understand that tabbing in here does not
        ///have any corelation with DatabaseMain.cs Tab system.
        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////START REGION OF VALUE INIT/////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        float tabWidth = position.width * .85f;
        float tabHeight = position.height - 10f;

        //Style area.
        skillStyle = new GUIStyle(GUI.skin.box);

        float firstTabWidth = tabWidth * 3 / 10;


        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////END REGION OF VALUE INIT///////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        #region Entry Of SkillsTab GUILayout
        //Start drawing the whole SkillsTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

        //The black box behind the SkillsTab? yes, this one.
        GUILayout.Box(" ", skillStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));


        GUILayout.EndArea(); // End of drawing SkillsTab
        #endregion
    }
}
