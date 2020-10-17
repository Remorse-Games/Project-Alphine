using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class TermTab : BaseTab
{
    public List<TermData> term = new List<TermData>();

    public int termSize = 1;
    public int index = 0;

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle enemyStyle;

    //Scroll position. Is this necessary
    Vector2 traitsScrollPos = Vector2.zero;

    //dataPath where the game data will be saved as a .assets
    private string _dataPath = "Data/TermData";

    public void Init()
    {
        LoadGameData<TermData>(ref termSize, term, _dataPath);
    }

    public void OnRender(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in enemysTab itself.
        ///So make sure you understand that tabbing in here does not
        ///have any corelation with DatabaseMain.cs Tab system.
        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////START REGION OF VALUE INIT/////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        float tabWidth = position.width * .85f;
        float tabHeight = position.height - 10f;
        float firstTabWidth = tabWidth * 3 / 10;
        //Style area.
        enemyStyle = new GUIStyle(GUI.skin.box);
        enemyStyle.normal.background = CreateTexture(1, 1, Color.gray);
        columnStyle = new GUIStyle(GUI.skin.box);
        columnStyle.normal.background = CreateTexture(1, 1, new Color32(99, 100, 100, 200));
        tabStyle = new GUIStyle(GUI.skin.box);
        if (EditorGUIUtility.isProSkin)
            tabStyle.normal.background = CreateTexture(1, 1, new Color32(76, 76, 76, 200));
        else
            tabStyle.normal.background = CreateTexture(1, 1, new Color32(200, 200, 200, 200));


        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////END REGION OF VALUE INIT///////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        #region Entry Of enemysTab GUILayout
        //Start drawing the whole enemyTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the enemysTab? yes, this one.
            GUILayout.Box(" ", enemyStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/2
            Rect firstTab = new Rect(0, 0, tabWidth * .75f, tabHeight - 18);
                GUILayout.BeginArea(firstTab, columnStyle);
                    #region BasicStatus
                    Rect basicStatuses = new Rect(5, 5, firstTab.width * .49f, firstTab.height * .43f);
                    GUILayout.BeginArea(basicStatuses, tabStyle);
                        GUILayout.Label("Basic Statuses", EditorStyles.boldLabel);
                        GUILayout.BeginHorizontal();
                            float fieldWidth = basicStatuses.width * .48f;
                            float fieldHeight = basicStatuses.height * .11f;
                            GUILayout.BeginVertical();
                                GUILayout.Label("Level:");
                                term[index].termLevel = GUILayout.TextField(term[index].termLevel, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("HP:");
                                term[index].termHP = GUILayout.TextField(term[index].termHP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("MP:");
                                term[index].termMP = GUILayout.TextField(term[index].termMP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("TP:");
                                term[index].termTP = GUILayout.TextField(term[index].termTP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("EXP:");
                                term[index].termEXP = GUILayout.TextField(term[index].termEXP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                            GUILayout.EndVertical();
                            GUILayout.BeginVertical();
                                GUILayout.Label("Level (abbr.):");
                                term[index].termLevelabbr = GUILayout.TextField(term[index].termLevelabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("HP (abbr.):");
                                term[index].termHPabbr = GUILayout.TextField(term[index].termHPabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("MP (abbr.):");
                                term[index].termMPabbr = GUILayout.TextField(term[index].termMPabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("TP (abbr.):");
                                term[index].termTPabbr = GUILayout.TextField(term[index].termTPabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("EXP (abbr.):");
                                term[index].termEXPabbr = GUILayout.TextField(term[index].termEXPabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                            GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                    GUILayout.EndArea();
                    #endregion

                    #region Parameters
                    Rect parameterBox = new Rect(basicStatuses.width + 10, 5, firstTab.width * .49f, basicStatuses.height);
                    GUILayout.BeginArea(parameterBox, tabStyle);
                        GUILayout.Label("Parameter", EditorStyles.boldLabel);
                        GUILayout.BeginHorizontal();
                            GUILayout.BeginVertical();
                                GUILayout.Label("Max. HP:");
                                term[index].termMaxHP = GUILayout.TextField(term[index].termMaxHP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("Attack:");
                                term[index].termAttack = GUILayout.TextField(term[index].termAttack, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("M. Attack:");
                                term[index].termMAttack = GUILayout.TextField(term[index].termMAttack, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("Agility:");
                                term[index].termAgility = GUILayout.TextField(term[index].termAgility, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("Hit Rate:");
                                term[index].termHitRate = GUILayout.TextField(term[index].termHitRate, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                            GUILayout.EndVertical();
                            GUILayout.BeginVertical();
                                GUILayout.Label("Max. MP:");
                                term[index].termMaxMP = GUILayout.TextField(term[index].termMaxMP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("Defense:");
                                term[index].termDefense = GUILayout.TextField(term[index].termDefense, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("M. Defense:");
                                term[index].termMDefense = GUILayout.TextField(term[index].termMDefense, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("Luck:");
                                term[index].termLuck = GUILayout.TextField(term[index].termLuck, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                                GUILayout.Label("Evasion Rate:");
                                term[index].termEvasionRate = GUILayout.TextField(term[index].termEvasionRate, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                            GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                    #endregion
                GUILayout.EndArea();
                #endregion

                

        GUILayout.EndArea();
        #endregion
        EditorUtility.SetDirty(term[index]);
    }

    #region Features
    public override void ItemTabLoader(int index)
    {
        Debug.Log(index + "index");
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {

        }
    }

    #endregion
}
