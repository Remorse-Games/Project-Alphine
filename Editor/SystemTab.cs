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
    public void Init()
    {

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
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
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
                                        GUILayout.TextArea("Null", GUILayout.Width(gameTitleTab.width - 30), GUILayout.Height(gameTitleTab.height/6));
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
                                        GUILayout.TextArea("Null", GUILayout.Width(currencyTab.width-30), GUILayout.Height(currencyTab.height/6));
                                    GUILayout.EndVertical();
                                GUILayout.EndArea();
                                #endregion

                                #region Begin Window Color
                                Rect windowColorTab = new Rect(startingPartyTab.width + 10, gameTitleTab.height+currencyTab.height +15, tabWidth*2/4-20, tabHeight*1/6-10);
                                GUILayout.BeginArea(windowColorTab, tabStyle);
                                    GUILayout.Space(2);
                                    GUILayout.Label("Window Color", EditorStyles.boldLabel);
                                    GUILayout.Space(windowColorTab.height/10);
                                    //put colored box
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
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                        GUILayout.Button("Placeholder");
                                    GUILayout.EndScrollView();
                                GUILayout.EndVertical();
                            GUILayout.EndArea();
                            #endregion

                        GUILayout.EndHorizontal();             

                    GUILayout.EndArea();
                    #endregion

                    #region BottonTab
                    GUILayout.BeginArea(new Rect(5, tabHeight/2, tabWidth-10, tabHeight/2-20), columnStyle);
                        #region MusicTab
                        Rect musicTab = new Rect(5,5,tabWidth/2-12.5f,tabHeight/2-25);
                        GUILayout.BeginArea(musicTab,tabStyle);
                            GUILayout.Space(2);
                            GUILayout.Label("Music", EditorStyles.boldLabel);

                        GUILayout.EndArea();
                        #endregion

                        #region SoundTab
                        Rect soundTab = new Rect(musicTab.width + 10,5,tabWidth/2-12.5f,tabHeight/2-25);
                        GUILayout.BeginArea(soundTab,tabStyle);
                            GUILayout.Space(2);
                            GUILayout.Label("Sounds", EditorStyles.boldLabel);
                            
                        GUILayout.EndArea();
                        #endregion
                    GUILayout.EndArea();
                    #endregion

            GUILayout.EndArea();
        #endregion
    }

    public override void ItemTabLoader(int index)
    {
        throw new System.NotImplementedException();
    }
}
