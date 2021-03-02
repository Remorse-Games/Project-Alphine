using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
public class StateTab : BaseTab
{

    //Having list of all states exist in data.
    public List<StateData> state = new List<StateData>();
    public List<TraitsData> traits = new List<TraitsData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in stateData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> stateDisplayName = new List<string>();
    List<string> traitDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle stateStyle;

    //Arrays
    public string[] stateRestriction =
    {
        "None",
        "Attack On Enemy",
        "Attack Anyone",
        "Attack On Ally",
        "Cannot Move",
    };
    public string[] stateSVMotion =
    {
        "Normal",
        "Abnormal",
        "Sleep",
        "Dead",
    };
    public string[] stateSVOverlay =
    {
        "None",
        "Poison",
        "Blind",
        "Silence",
        "Rage",
        "Confusion",
        "Add Other Manually..",
    };

    public string[] stateAutoRemovalList =
    {
        "None",
        "Action End",
        "Turn End",
    };

    //How many state in ChangeMaximum Func
    public int stateSize;
    public int stateSizeTemp;
    public static int[] traitSize;

    //i don't know about this but i leave this to handle later.
    public static int index = 0;
    int indexTemp = -1;
    public static int traitIndex = 0;
    public static int traitIndexTemp = -1;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;
    Vector2 traitsScrollPos = Vector2.zero; 

    //Icon
    Texture2D stateIcon;

    public void Init()
    {
        //Clears List
        state.Clear();
        traits.Clear();

        //Resetting each index to 0, so that it won't have error (Index Out Of Range)
        index = 0;
        traitIndex = 0;

        LoadGameData<StateData>(ref stateSize, state, PathDatabase.StateRelativeDataPath);

        traitSize = new int[stateSize]; //Resets Trait Sizing
        LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.StateTraitRelativeDataPath + (index + 1));

        //Create Folder For TraitData and its sum is based on weaponSize value
        FolderCreator(stateSize, "Assets/Resources/Data/StateData", "TraitData");

        //Check if TraitData_(index) is empty, if it is empty then create a SO named Trait_1
        if (traitSize[index] <= 0)
        {
            traitIndex = 0;
            ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.StateTraitExplicitDataPath + (index + 1) + "/Trait_");
        }

        ClearNullScriptableObjects();
        ListReset();
    }
    public void OnRender(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in stateTab itself.
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
        stateStyle = new GUIStyle(GUI.skin.box);
        stateStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

        #region Entry Of StateTab GUILayout
        //Start drawing the whole stateTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the stateTab? yes, this one.
            GUILayout.Box(" ", stateStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

        
            #region Tab 1/3

            //First Tab of three
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
                GUILayout.Box("States", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

                //Scroll View
                #region ScrollView
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
                index = GUILayout.SelectionGrid(index, stateDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * stateSize));
                GUILayout.EndScrollView();
                #endregion

                //Happen everytime selection grid is updated
                if (GUI.changed && index != indexTemp)
                {
                    indexTemp = index;
                    traitIndex = 0;
                    traitIndexTemp = -1;

                    //Load TraitsData
                    traits.Clear();
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.StateTraitRelativeDataPath + (index + 1));
                    
                    //Check if TraitsData folder is empty
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.StateTraitExplicitDataPath + (index + 1) + "/Trait_");
                        traitIndexTemp = 0;
                    }
                    ClearNullScriptableObjects();

                    ListReset();
                    ItemTabLoader(indexTemp);
                    indexTemp = -1;
                }

                // Change Maximum field and button
                stateSizeTemp = EditorGUILayout.IntField(stateSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)) && stateSizeTemp > 0)
                {
                    stateSize = stateSizeTemp;
                    index = indexTemp = 0;
                    FolderCreator(stateSizeTemp, "Assets/Resources/Data/StateData", "TraitData");
                    ChangeMaximum<StateData>(stateSizeTemp, state, PathDatabase.StateExplicitDataPath);
                    
                    //Remove Name Duplicates
                    if (stateSize < stateSizeTemp)
                    {
                        int oldStateSize = stateSizeTemp;
                        stateSize = stateSizeTemp;
                        ListReset();

                        for (int i = oldStateSize; i < stateSizeTemp; i++)
                        {
                            //Function Calling from BaseTab to check same names
                            state[i].stateName = RemoveDuplicates(stateSize, i, state[i].stateName, stateDisplayName);
                            ListReset();
                        }
                    }

                    stateSize = stateSizeTemp;
                    //New TraitSize array length
                    int[] tempArr = new int[traitSize.Length];
                    for (int i = 0; i < traitSize.Length; i++)
                        tempArr[i] = traitSize[i];

                    traitSize = new int[stateSize];

                    #region FindSmallestBetween
                        int smallestValue;
                        if (tempArr.Length < stateSize) smallestValue = tempArr.Length;
                        else smallestValue = stateSize;
                    #endregion

                    for (int i = 0; i < smallestValue; i++)
                        traitSize[i] = tempArr[i];

                    //Reload Data and Check SO
                    traits.Clear();
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.StateTraitRelativeDataPath + (index + 1));
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.StateTraitExplicitDataPath + (index + 1) + "/Trait_");
                    }

                    ClearNullScriptableObjects();
                    ListReset();
                }
                else if(stateSizeTemp <= 0){
                    stateSizeTemp = stateSize;
                }
            GUILayout.EndArea();
            #endregion // End Of First Tab

            #region Tab 2/3
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 25), columnStyle);

                Rect generalBox = new Rect(5, 5, firstTabWidth + 60, position.height * .28f);
                    #region GeneralSettings
                    GUILayout.BeginArea(generalBox, tabStyle); // Start of General Settings tab
                        GUILayout.Label("General Settings", EditorStyles.boldLabel); // General Settings label
                        #region Vertical
                        GUILayout.BeginVertical();
                            #region Horizontal
                            GUILayout.BeginHorizontal();
                                #region Name
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Name:"); // Name label
                                    if (stateSize > 0)
                                    {
                                        state[index].stateName = GUILayout.TextField(state[index].stateName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                        stateDisplayName[index] = state[index].stateName;

                                        
                                        //Function Calling from BaseTab to check same names
                                        state[index].stateName = RemoveDuplicates(stateSize, index, state[index].stateName, stateDisplayName);
                                    }
                                    else
                                    {
                                        GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                    }
                                GUILayout.EndVertical();
        #endregion
                                #region Icon
                                Rect iconBox = new Rect(generalBox.width / 2 - 3, generalBox.height * .05f + 5, firstTabWidth - 220, position.height / 2);
                                GUILayout.BeginArea(iconBox); // Icon Area
                                GUILayout.BeginHorizontal();
                                    GUILayout.BeginVertical();
                                        GUILayout.Label("Icon:"); // Icon label
                                    GUILayout.EndVertical();

                                    GUILayout.BeginVertical();
                                        GUILayout.Box(stateIcon, GUILayout.Width(61), GUILayout.Height(61)); // Icon Box preview
                                        Color tempColorIcon = GUI.backgroundColor;
                                        GUI.backgroundColor = Color.green;
                                        if (GUILayout.Button("Edit Icon", GUILayout.Height(20), GUILayout.Width(61))) // Icon changer Button
                                        {
                                            state[index].icon = ImageChanger(
                                            index,
                                            "Choose Icon",
                                            "Assets/Resources/Image"
                                            );
                                            ItemTabLoader(index);
                                        }
                                        GUI.backgroundColor = tempColorIcon;
                                    GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                GUILayout.EndArea();
                                #endregion
                            GUILayout.EndHorizontal();
                            #endregion

                            GUILayout.Space(40);

                            #region Horizontal
                            GUILayout.BeginHorizontal();  
        
                                #region Restriction Priority
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Restiction:"); // state Type class label
                                    if (stateSize > 0)
                                    {
                                        state[index].selectedRestriction = EditorGUILayout.Popup(state[index].selectedRestriction, stateRestriction, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                    }
                                    else
                                    {
                                        EditorGUILayout.Popup(0, stateRestriction, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                    }
                                GUILayout.EndVertical();
                                #endregion

                                #region Priority
                                GUILayout.BeginVertical();
        
                                    GUILayout.Label("Priority:"); // MP Cost class label
                                    if (stateSize > 0)
                                    { 
                                        state[index].statePriority = EditorGUILayout.IntField(state[index].statePriority, 
                                                                                                GUILayout.Width(generalBox.width / 4 - 2), 
                                                                                                GUILayout.Height(generalBox.height / 8 )
                                                                                             ); 
                                    }
                                    else
                                    { 
                                        EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 )); 
                                    }
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Space(generalBox.width * .5f);
                                    GUILayout.EndVertical();
                                GUILayout.EndVertical();                  
                                #endregion

                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();

                                #region [SV] Motion & Overlay
                                GUILayout.BeginVertical();
                                    GUILayout.Label("[SV] Motion:"); // Scope class label
                                    if (stateSize > 0)
                                    {
                                        state[index].selectedSVMotion = EditorGUILayout.Popup(state[index].selectedSVMotion, stateSVMotion, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                    }
                                    else
                                    {
                                        EditorGUILayout.Popup(0, stateSVMotion, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                    }
                                GUILayout.EndVertical();

                                GUILayout.BeginVertical();
                                    GUILayout.Label("[SV] Overlay:"); // Occasion class label
                                    if (stateSize > 0)
                                    {
                                        state[index].selectedSVOverlay = EditorGUILayout.Popup(state[index].selectedSVOverlay, stateSVOverlay, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2));
                                    }
                                    else
                                    {
                                        EditorGUILayout.Popup(0, stateSVOverlay, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2));
                                    }
                                GUILayout.EndVertical();
                                #endregion

                            GUILayout.EndHorizontal();
                            #endregion

                        GUILayout.EndVertical();
                        #endregion
                    GUILayout.EndArea();
                    #endregion

                Rect removalConditions = new Rect(5, generalBox.height + 10, firstTabWidth + 60, position.height * .27f);
                    #region RemovalCondition
                    GUILayout.BeginArea(removalConditions, tabStyle);
                        GUILayout.Label("Removal Conditions", EditorStyles.boldLabel);

                        GUILayout.BeginVertical();
                            GUILayout.BeginHorizontal();
                                if (EditorGUILayout.Toggle("Remove At Battle End", state[index].stateRemoveAt))
                                {
                                    state[index].stateRemoveAt = true;
                                }
                                else
                                {
                                    state[index].stateRemoveAt = false;
                                }
                                if (EditorGUILayout.Toggle("Remove By Restriction", state[index].stateRemoveByRestriction))
                                {
                                    state[index].stateRemoveByRestriction = true;
                                }
                                else
                                {
                                    state[index].stateRemoveByRestriction = false;
                                }
       
                            GUILayout.EndHorizontal();
                            
                            GUILayout.Space(5);
                            DrawUILine(Color.gray, 2, 5);
                            GUILayout.Space(5);
                            
                            GUILayout.BeginVertical();

                                GUILayout.BeginHorizontal();
                                    GUILayout.Label("Auto-Removal Timing:");
                                    if (stateSize > 0)
                                    {
                                        state[index].selectedAutoRemoval = EditorGUILayout.Popup(state[index].selectedAutoRemoval, stateAutoRemovalList, GUILayout.Height(generalBox.height / 8 ), GUILayout.Width((generalBox.width /2)));
                                    }
                                    else
                                    {
                                        EditorGUILayout.Popup(0, stateAutoRemovalList, GUILayout.Height(generalBox.height / 8 ), GUILayout.Width(generalBox.width / 2));
                                    }

                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                    GUILayout.Label("Duration in Turns:");
                                    GUILayout.Space(generalBox.width * .20f);
                                    if(state[index].selectedAutoRemoval > 0)
                                    {
                                        state[index].durationInTurnsA = EditorGUILayout.IntField(state[index].durationInTurnsA, GUILayout.Width(generalBox.width / 2 * .4f), GUILayout.Height(generalBox.height / 8));
                                        GUILayout.Label(" ~");
                                        state[index].durationInTurnsB = EditorGUILayout.IntField(state[index].durationInTurnsB, GUILayout.Width(generalBox.width / 2 * .4f), GUILayout.Height(generalBox.height / 8));
                                    }
                                    else
                                    {
                                        EditorGUILayout.IntField(1, GUILayout.Width(generalBox.width / 2 * .4f), GUILayout.Height(generalBox.height / 8));
                                        GUILayout.Label(" ~");
                                        EditorGUILayout.IntField(1, GUILayout.Width(generalBox.width / 2 * .4f), GUILayout.Height(generalBox.height / 8 ));
                                    }
                                GUILayout.EndHorizontal();
                               
                                GUILayout.Space(5);
                                DrawUILine(Color.gray, 2, 5);
                                GUILayout.Space(5);
                                
                                GUILayout.BeginHorizontal();
                                    if (EditorGUILayout.Toggle("Remove By Damage", state[index].stateRemoveByDamage))
                                    {
                                        state[index].stateRemoveByDamage = true;
                                        state[index].removeByDamageValue = EditorGUILayout.IntField(state[index].removeByDamageValue, GUILayout.Width(generalBox.width / 2 * .4f), GUILayout.Height(generalBox.height / 8));
                                    }
                                    else
                                    {
                                        state[index].stateRemoveByDamage = false;
                                    }
                                GUILayout.EndHorizontal();
                                GUILayout.Space(removalConditions.height * .03f);
                                GUILayout.BeginHorizontal();
                                    if (EditorGUILayout.Toggle("Remove By Walking", state[index].stateRemoveByWalking))
                                    {
                                        state[index].stateRemoveByWalking = true;
                                        state[index].removeByWalkingValue = EditorGUILayout.IntField(state[index].removeByWalkingValue, GUILayout.Width(generalBox.width / 2 * .4f), GUILayout.Height(generalBox.height / 8));
                                    }
                                    else
                                    {
                                        state[index].stateRemoveByWalking = false;
                                    }
                                GUILayout.EndHorizontal();

                            GUILayout.EndVertical();

                        GUILayout.EndVertical();

                GUILayout.EndArea();
                #endregion

                Rect messageBox = new Rect(5, generalBox.height + removalConditions.height + 15, firstTabWidth + 60, position.height * .375f);
                    #region Message
                    GUILayout.BeginArea(messageBox, tabStyle);
                        GUILayout.BeginVertical();
                            GUILayout.Label("Messages", EditorStyles.boldLabel);

                            float textFieldWidth = messageBox.width * .60f;
                            float textFieldHeight = messageBox.height * .14f;
                            float leftSpaceWidth = messageBox.width * .08f;

                            GUILayout.Label("If an actor is inflicted with the state:");
                            GUILayout.BeginHorizontal();
                                GUILayout.Space(leftSpaceWidth);
                                GUILayout.Label("(Target Name)");
                                if (stateSize > 0)
                                {
                                    state[index].firstMessageTarget = GUILayout.TextField(state[index].firstMessageTarget, 
                                                                                            GUILayout.Width(textFieldWidth), 
                                                                                            GUILayout.Height(textFieldHeight)
                                                                                          );
                                }
                                else
                                {
                                    GUILayout.TextField("Null", GUILayout.Width(textFieldWidth), GUILayout.Height(textFieldHeight));
                                }
                            GUILayout.EndHorizontal();

                            GUILayout.Label("If an enemy is inflicted with the state:");
                            GUILayout.BeginHorizontal();
                                GUILayout.Space(leftSpaceWidth);
                                GUILayout.Label("(Target Name)");
                                if (stateSize > 0)
                                {
                                    state[index].secondMessageTarget = GUILayout.TextField(state[index].secondMessageTarget, 
                                                                                            GUILayout.Width(textFieldWidth), 
                                                                                            GUILayout.Height(textFieldHeight)
                                                                                           );
                                }
                                else
                                {
                                    GUILayout.TextField("Null", GUILayout.Width(textFieldWidth), GUILayout.Height(textFieldHeight));
                                }
                            GUILayout.EndHorizontal();

                            GUILayout.Label("If the state persists:");
                            GUILayout.BeginHorizontal();
                                GUILayout.Space(leftSpaceWidth);
                                GUILayout.Label("(Target Name)");
                                if (stateSize > 0)
                                {
                                    state[index].thirdMessageTarget = GUILayout.TextField(state[index].thirdMessageTarget, 
                                                                                            GUILayout.Width(textFieldWidth), 
                                                                                            GUILayout.Height(textFieldHeight)
                                                                                          );
                                }
                                else
                                {
                                    GUILayout.TextField("Null", GUILayout.Width(textFieldWidth), GUILayout.Height(textFieldHeight));
                                }
                            GUILayout.EndHorizontal();

                            GUILayout.Label("If the state is removed:");
                            GUILayout.BeginHorizontal();
                                GUILayout.Space(leftSpaceWidth);
                                GUILayout.Label("(Target Name)");
                                if (stateSize > 0)
                                {
                                    state[index].fourthMessageTarget = GUILayout.TextField(state[index].fourthMessageTarget, 
                                                                                            GUILayout.Width(textFieldWidth), 
                                                                                            GUILayout.Height(textFieldHeight)
                                                                                           );
                                }
                                else
                                {
                                    GUILayout.TextField("Null", GUILayout.Width(textFieldWidth), GUILayout.Height(textFieldHeight));
                                }
                            GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    GUILayout.EndArea();
                    #endregion

            GUILayout.EndArea();
            #endregion

            
            #region Tab 3/3
            //Third Column
            GUILayout.BeginArea(new Rect(position.width - (position.width - firstTabWidth * 2) + 77, 0, firstTabWidth + 25, tabHeight - 25), columnStyle);

                //Traits
                Rect traitsBox = new Rect(5, 5, firstTabWidth + 15, position.height * 5 / 8);
                #region Traits
                ListReset();
                GUILayout.BeginArea(traitsBox, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("Traits", EditorStyles.boldLabel);
                    GUILayout.Space(5);
                    #region Horizontal For Type And Content
                    GUILayout.BeginHorizontal();
                       GUILayout.Label(PadString("Type", string.Format("{0}", "  Content")), GUILayout.Width(traitsBox.width));
                    GUILayout.EndHorizontal();
                    #endregion
                    #region ScrollView
                    traitsScrollPos = GUILayout.BeginScrollView(
                                traitsScrollPos, 
                                false, 
                                true, 
                                GUILayout.Width(firstTabWidth + 5), 
                                GUILayout.Height(traitsBox.height * 0.83f)
                                );
        
                            GUI.changed = false;
                            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                            traitIndex = GUILayout.SelectionGrid(
                                traitIndex,
                                traitDisplayName.ToArray(),
                                1,
                                GUILayout.Width(firstTabWidth - 20),
                                GUILayout.Height(position.height / 24 * traitSize[index]
                                ));
                            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                    GUILayout.EndScrollView();
                    #endregion

                    //Happen everytime selection grid is updated
                        if (GUI.changed)
                        {
                            if (traitIndex != traitIndexTemp)
                            {
                                TraitWindow.ShowWindow(traits, traitIndex, traitSize[index], TabType.State);
                            
                                traitIndexTemp = traitIndex;
                            }
                        }

                        //Create Empty SO if there isn't any null SO left
                        if ((traits[traitSize[index] - 1].traitName != null && traits[traitSize[index] - 1].traitName != "") && traitSize[index] > 0)
                        {
                            traitIndex = 0;
                            ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.StateTraitExplicitDataPath + (index + 1) + "/Trait_");
                        }

                        Color tempColor = GUI.backgroundColor;
                        GUI.backgroundColor = Color.red;
                        //Delete All Data Button
                        if (GUILayout.Button("Delete All Data", GUILayout.Width(traitsBox.width * .3f), GUILayout.Height(traitsBox.height * .055f)))
                        {
                            if (EditorUtility.DisplayDialog("Delete All Trait Data", "Are you sure want to delete all Trait Data?", "Yes", "No"))
                            {
                                traitIndex = 0;
                                traitSize[index] = 1;
                                ChangeMaximum<TraitsData>(0, traits, PathDatabase.StateTraitExplicitDataPath + (index + 1) + "/Trait_");
                                ChangeMaximum<TraitsData>(1, traits, PathDatabase.StateTraitExplicitDataPath + (index + 1) + "/Trait_");
                            }
                        }
                        GUI.backgroundColor = tempColor;
                GUILayout.EndArea();
                #endregion //End of TraitboxArea


                //Notes
                Rect notesBox = new Rect(5, traitsBox.height + 10, firstTabWidth + 15, position.height * 2.5f / 8 - 7);
                #region NoteBox
                GUILayout.BeginArea(notesBox, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("Notes", EditorStyles.boldLabel);
                    GUILayout.Space(notesBox.height / 50);
                    if (stateSize > 0)
                    {
                        state[index].notes = GUILayout.TextArea(state[index].notes, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * .85f));
                    }
                    else
                    {
                        GUILayout.TextArea("Null", GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.85f));
                    }
                GUILayout.EndArea();
                #endregion //End of notebox area

            GUILayout.EndArea();
            #endregion // End of third column

        GUILayout.EndArea(); //End drawing the whole StateTab
        #endregion 
        EditorUtility.SetDirty(state[index]);

    }

    #region Features
    public override void ItemTabLoader(int index)
    {
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if (stateSize > 0)
            {
                if (state[index].icon == null)
                    stateIcon = defTex;
                else
                    stateIcon = TextureToSprite(state[index].icon);

            }
        }
    }

    ///<summary>
    ///Clears out the displayName list and add it with new value
    ///</summary>
    private void ListReset()
    {
        stateDisplayName.Clear();
        for (int i = 0; i < stateSize; i++)
        {
            stateDisplayName.Add(state[i].stateName);
        }
        //Trait Reset
        traitDisplayName.Clear();
        for (int i = 0; i < traitSize[index]; i++)
        {
            traitDisplayName.Add(traits[i].traitName);
        }
    }

    private void ClearNullScriptableObjects()
    {
        bool availableNull = true;
        while (availableNull)
        {
            availableNull = false;
            for (int i = 0; i < traitSize[index] - 1; i++)
            {
                if (string.IsNullOrEmpty(traits[i].traitName))
                {
                    availableNull = true;
                    for (int j = i; j < traitSize[index] - 1; j++)
                    {
                        traits[j].traitName = traits[j + 1].traitName;
                        traits[j].selectedTabToggle = traits[j + 1].selectedTabToggle;
                        traits[j].selectedTabIndex = traits[j + 1].selectedTabIndex;
                        traits[j].selectedArrayIndex = traits[j + 1].selectedArrayIndex;
                        traits[j].traitValue = traits[j + 1].traitValue;
                    }
                    ChangeMaximum<TraitsData>(--traitSize[index], traits, PathDatabase.StateTraitExplicitDataPath + (index + 1) + "/Trait_");
                    i--;
                }
            }
        }
    }

    #endregion
}