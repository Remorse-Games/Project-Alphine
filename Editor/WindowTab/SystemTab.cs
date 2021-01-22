using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SystemTab : BaseTab
{
    //scrollpos
    Vector2 scrollStartParty = Vector2.zero;
    Vector2 scrollSVMagic = Vector2.zero;

    GUIStyle systemStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;


    int actorSize, skillSize;
    public static int selectedStartingPartyIndex = -1;
    public static int selectedMagicSkillIndex = -1;
    public SystemData system;
    public void Init()
    {
        system = Resources.Load<SystemData>("Data/SystemData/SystemData");
        if(system == null)
        {
            ScriptableObject newSystemData = ScriptableObject.CreateInstance<SystemData>();
            AssetDatabase.CreateAsset(newSystemData, "Assets/Resources/Data/SystemData/SystemData.asset");
            AssetDatabase.SaveAssets();
            system = Resources.Load<SystemData>("Data/SystemData/SystemData");
        }

        LoadList();

        if (system.startingParty.Count <= 0 || (system.startingParty.Count < actorSize && system.startingParty[system.startingParty.Count - 1] != ""))
        {
            system.startingParty.Add("");
        }

        if (system.magicSkills.Count <= 0 || (system.magicSkills.Count < skillSize && system.magicSkills[system.magicSkills.Count - 1] != ""))
        {
            system.magicSkills.Add("");
        }
    }
    public void OnRender(Rect position)
    {
        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////START REGION OF VALUE INIT/////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        float tabWidth = position.width * .85f;
        float tabHeight = position.height - 10f;

        float firstTabWidth = tabWidth * 3 / 10;

        //Style area.
        systemStyle = new GUIStyle(GUI.skin.box);
        systemStyle.normal.background = CreateTexture(1, 1, Color.gray);
        columnStyle = new GUIStyle(GUI.skin.box);
        columnStyle.normal.background = CreateTexture(1, 1, new Color32(99, 100, 100, 200));
        tabStyle = new GUIStyle(GUI.skin.box);
        if(EditorGUIUtility.isProSkin)
        tabStyle.normal.background = CreateTexture(1, 1, new Color32(76, 76, 76, 200));
        else
        tabStyle.normal.background = CreateTexture(1,1, new Color32(200,200,200,200));

        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////END REGION OF VALUE INIT///////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        #region Starting of Systembase
            GUILayout.BeginArea(new Rect(position.width/7, 5, tabWidth, tabHeight));
                GUILayout.Box(" ", systemStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));
                
                    #region UpperTab
                    GUILayout.BeginArea(new Rect(5, 5, tabWidth-10, tabHeight/2-10), columnStyle);
                        
                        GUILayout.BeginHorizontal();

                            #region Begin Starting Party Tab
                            Rect startingPartyTab = new Rect(5,5, tabWidth*1/4, tabHeight*2/4 - 10);
                            GUILayout.BeginArea(startingPartyTab, tabStyle);
                                GUILayout.Space(2);
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Starting Party", EditorStyles.boldLabel);
                                    GUILayout.Space(2);
                                    scrollStartParty = GUILayout.BeginScrollView(scrollStartParty, false, true, GUILayout.Width(startingPartyTab.width-5), GUILayout.Height(startingPartyTab.height-30));
                                        EditorGUI.BeginDisabledGroup(selectedStartingPartyIndex != -1);
                                        selectedStartingPartyIndex = GUILayout.SelectionGrid(selectedStartingPartyIndex, system.startingParty.ToArray(), 1, GUILayout.Height(startingPartyTab.height/10 * system.startingParty.Count));
                                        EditorGUI.EndDisabledGroup();
                
                                        if(selectedStartingPartyIndex != -1)
                                        {
                                            SelectWindow.ShowWindow(system, selectedStartingPartyIndex, SelectType.Actor);
                                        }
                
                                    GUILayout.EndScrollView();
                                GUILayout.EndVertical();
                            GUILayout.EndArea();
                            #endregion

                            GUILayout.BeginVertical();
                                #region Begin GameName Tab
                                Rect gameTitleTab = new Rect(startingPartyTab.width + 10, 5, tabWidth*2/4-20, tabHeight*1/6 - 10);
                                GUILayout.BeginArea(gameTitleTab, tabStyle);
                                    GUILayout.Space(2);
                                    GUILayout.BeginVertical();
                                        GUILayout.Label("Game Title", EditorStyles.boldLabel);
                                        GUILayout.Space(gameTitleTab.height/3);
                                        system.gameTitle = GUILayout.TextArea(system.gameTitle, GUILayout.Width(gameTitleTab.width - 30), GUILayout.Height(gameTitleTab.height/6));
                                    GUILayout.EndVertical();
                                GUILayout.EndArea();
                                #endregion

                                #region Begin Currency Tab
                                Rect currencyTab = new Rect(startingPartyTab.width + 10, gameTitleTab.height+10, tabWidth*2/4-20, tabHeight*1/6-10);
                                GUILayout.BeginArea(currencyTab, tabStyle);
                                    GUILayout.Space(2);
                                    GUILayout.BeginVertical();
                                        GUILayout.Label("Currency",EditorStyles.boldLabel);
                                        GUILayout.Space(currencyTab.height/3);
                                        system.currencyUnit = GUILayout.TextArea(system.currencyUnit, GUILayout.Width(currencyTab.width-30), GUILayout.Height(currencyTab.height/6));
                                    GUILayout.EndVertical();
                                GUILayout.EndArea();
                                #endregion

                                #region Begin Window Color
                                Rect windowColorTab = new Rect(startingPartyTab.width + 10, gameTitleTab.height+currencyTab.height +15, tabWidth*2/4-20, tabHeight*1/6-10);
                                GUILayout.BeginArea(windowColorTab, tabStyle);
                                    GUILayout.Space(2);
                                    GUILayout.Label("Window Color", EditorStyles.boldLabel);
                                    GUILayout.Space(windowColorTab.height/10);
                                    system.windowColor =  EditorGUILayout.ColorField(system.windowColor, GUILayout.Width(windowColorTab.width*99/100), GUILayout.Height(windowColorTab.height*65/100));
                                    //GUILayout.Button(CreateTexture(Mathf.RoundToInt(windowColorTab.width-10),Mathf.RoundToInt(windowColorTab.height-20), system.windowColor));
                                    //GUILayout.Box(CreateTexture(Mathf.RoundToInt(windowColorTab.width-10),Mathf.RoundToInt(windowColorTab.height-20), system.windowColor));
                                GUILayout.EndArea();
                                #endregion
                            GUILayout.EndVertical();

                            #region SV Magic Skills
                            Rect svMagicSkillsTab = new Rect(startingPartyTab.width +gameTitleTab.width +15, 5, tabWidth*1/4, tabHeight*2/4 - 15);
                            GUILayout.BeginArea(svMagicSkillsTab, tabStyle);
                                GUILayout.Space(2);
                                GUILayout.BeginVertical();
                                    GUILayout.Label("[SV] Magic Skills", EditorStyles.boldLabel);
                                    GUILayout.Space(2);
                                    scrollSVMagic = GUILayout.BeginScrollView(scrollSVMagic, false, true, GUILayout.Width(svMagicSkillsTab.width-5), GUILayout.Height(svMagicSkillsTab.height-30));
                                        EditorGUI.BeginDisabledGroup(selectedMagicSkillIndex != -1);
                                        selectedMagicSkillIndex = GUILayout.SelectionGrid(selectedMagicSkillIndex, system.magicSkills.ToArray(), 1, GUILayout.Height(svMagicSkillsTab.height/10 * system.magicSkills.Count));
                                        EditorGUI.EndDisabledGroup();

                                        if(selectedMagicSkillIndex != -1)
                                        {
                                            SelectWindow.ShowWindow(system, selectedMagicSkillIndex, SelectType.Skill);
                                        }
                                    GUILayout.EndScrollView();
                                GUILayout.EndVertical();
                            GUILayout.EndArea();
                            #endregion

                        GUILayout.EndHorizontal();             

                    GUILayout.EndArea();
                    #endregion

                    #region BottomTab
                    GUILayout.BeginArea(new Rect(5, tabHeight/2, tabWidth-10, tabHeight/2-20), columnStyle);

                        #region MusicTab
                        Rect musicTab = new Rect(5,5,tabWidth/2-12.5f,tabHeight/2-25);
                        GUILayout.BeginArea(musicTab,tabStyle);
                            GUILayout.Space(2);
                            GUILayout.Label("Music", EditorStyles.boldLabel);
                            GUILayout.Space(musicTab.height/15);
                            GUILayout.BeginHorizontal();
                                GUILayout.Label("Type");
                                GUILayout.Label("File Name");
                            GUILayout.EndHorizontal();

                            GUILayout.BeginScrollView(Vector2.zero, false, true, GUILayout.Width(musicTab.width*99/100), GUILayout.Height(musicTab.height*8/10));
                                //selection grid
                            GUILayout.EndScrollView();

                        GUILayout.EndArea();
                        #endregion

                        #region SoundTab
                        Rect soundTab = new Rect(musicTab.width + 10,5,tabWidth/2-12.5f,tabHeight/2-25);
                        GUILayout.BeginArea(soundTab,tabStyle);
                            GUILayout.Space(2);
                            GUILayout.Label("Sounds", EditorStyles.boldLabel);
                            GUILayout.Space(musicTab.height/15);
                            GUILayout.BeginHorizontal();
                                GUILayout.Label("Type");
                                GUILayout.Label("File Name");
                            GUILayout.EndHorizontal();

                            GUILayout.BeginScrollView(Vector2.zero, false, true, GUILayout.Width(musicTab.width*99/100), GUILayout.Height(musicTab.height*8/10));
                                //selection grid
                            GUILayout.EndScrollView();
                        GUILayout.EndArea();
                        #endregion
                    GUILayout.EndArea();
                    #endregion

            GUILayout.EndArea();
        #endregion
        EditorUtility.SetDirty(system);
    }

    public override void ItemTabLoader(int index)
    {
        throw new System.NotImplementedException();
    }
    
    private void LoadList()
    {
        ActorData[] actor = Resources.LoadAll<ActorData>(PathDatabase.ActorRelativeDataPath);
        actorSize = actor.Length;
        
        for (int i = 0; i < system.startingParty.Count; i++)
        {
            bool removeThisElement = true;

            int j;
            for(j = 0; j < actorSize; j++)
            {
                if(actor[j].actorName == system.startingParty[i])
                {
                    removeThisElement = false;
                    break;
                }
            }

            if (removeThisElement)
            {
                system.startingParty.RemoveAt(i);
                i--;
            }
        }


        TypeSkillData[] skill = Resources.LoadAll<TypeSkillData>(PathDatabase.SkillRelativeDataPath);
        skillSize = skill.Length;

        for (int i = 0; i < system.magicSkills.Count; i++)
        {
            bool removeThisElement = true;

            int j;
            for (j = 0; j < skillSize; j++)
            {
                if (skill[j].dataName == system.magicSkills[i])
                {
                    removeThisElement = false;
                    break;
                }
            }

            if (removeThisElement)
            {
                system.magicSkills.RemoveAt(i);
                i--;
            }
        }
    }
}
