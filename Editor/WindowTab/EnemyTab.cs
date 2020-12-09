﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class EnemyTab : BaseTab
{

    //Having list of all enemys exist in data.
    public List<EnemyData> enemy = new List<EnemyData>();
    public List<TraitsData> traits = new List<TraitsData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in enemyData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> enemyDisplayName = new List<string>();
    List<string> traitDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle enemyStyle;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;
    Vector2 actionScrollPos = Vector2.zero;
    Vector2 traitsScrollPos = Vector2.zero;

    //How many enemy in ChangeMaximum Func
    public int enemySize;
    public int enemySizeTemp;
    public static int[] traitSize;

    //i don't know about this but i leave this to handle later.
    public static int index = 0;
    public static int traitIndex = 0;
    int indexTemp = -1;
    public static int traitIndexTemp = -1;

    Texture2D enemyImage;
    public void Init()
    {
        //Clears List
        enemy.Clear();
        traits.Clear();

        //Resetting each index to 0, so that it won't have error (Index Out Of Range)
        index = 0;
        traitIndex = 0;

        //Load Every List needed in ActorTab
        LoadGameData<EnemyData>(ref enemySize, enemy, PathDatabase.EnemyRelativeDataPath);
        
        traitSize = new int[enemySize]; //Resets Trait Sizing
        LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.EnemyTraitRelativeDataPath + (index + 1));

        //Create Folder For TraitsData and its sum is based on actorSize value
        FolderCreator(enemySize, "Assets/Resources/Data/EnemyData", "TraitData");

        //Check if TraitsData_(index) is empty, if it is empty then create a SO named Trait_1
        if (traitSize[index] <= 0)
        {
            traitIndex = 0;
            ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.EnemyTraitExplicitDataPath + (index + 1) + "/Trait_");
        }
        ClearNullScriptableObjects(); //Clear Trait SO without a value
        ListReset(); //Resets List
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

            #region Tab 1/3
            //First Tab of three
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));

                GUILayout.Box("Enemies", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

                //Scroll View
                #region ScrollView
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
                index = GUILayout.SelectionGrid(index, enemyDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * enemySize));
                GUILayout.EndScrollView();
                #endregion

                //Happen everytime selection grid is updated
                if (GUI.changed && index != indexTemp)
                {
                    indexTemp = index;
                    traitIndex = traitIndexTemp = 0;
                    ItemTabLoader(indexTemp);

                    //Load TraitsData
                    traits.Clear();
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.EnemyTraitRelativeDataPath + (index + 1));
                    //Check if TraitsData folder is empty
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.EnemyTraitExplicitDataPath + (index + 1) + "/Trait_");
                        traitIndexTemp = 0;
                    }
                    ClearNullScriptableObjects();
                    ListReset();
                    indexTemp = -1;
                }

                // Change Maximum field and button
                enemySizeTemp = EditorGUILayout.IntField(enemySizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
                {
                    enemySize = enemySizeTemp;
                    index = indexTemp = 0;
                    FolderCreator(enemySize, "Assets/Resources/Data/EnemyData", "TraitData");
                    ChangeMaximum<EnemyData>(enemySize, enemy, PathDatabase.EnemyExplicitDataPath);
                    
                    //New TraitSize array length
                    int[] tempArr = new int[traitSize.Length];
                    for (int i = 0; i < traitSize.Length; i++)
                        tempArr[i] = traitSize[i];

                    traitSize = new int[enemySize];

                    #region FindSmallestBetween
                        int smallestValue;
                        if (tempArr.Length < enemySize) smallestValue = tempArr.Length;
                        else smallestValue = enemySize;
                    #endregion

                    for (int i = 0; i < smallestValue; i++)
                        traitSize[i] = tempArr[i];

                    //Reload Data and Check SO
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.EnemyTraitRelativeDataPath + (index + 1));
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.EnemyTraitExplicitDataPath + (index + 1) + "/Trait_");
                    }

                    ClearNullScriptableObjects();
                    ListReset();
                }

            GUILayout.EndArea();
            #endregion // End Of First Tab

        
            #region Tab 2/3
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 25), columnStyle);

                Rect generalBox = new Rect(5, 5, firstTabWidth + 60, position.height / 4 + 60);
                    #region GeneralSettings
                    GUILayout.BeginArea(generalBox, tabStyle); // Start of General Settings tab
                        GUILayout.Label("General Settings", EditorStyles.boldLabel); // General Settings label
                        GUILayout.BeginHorizontal();
                            GUILayout.BeginVertical();
                                GUILayout.Label("Name:"); // Name label
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyName = GUILayout.TextField(enemy[index].enemyName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                    enemyDisplayName[index] = enemy[index].enemyName;
                                }
                                else
                                {
                                    GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                }
                                GUILayout.Label("Image:"); // Image labe;
                                GUILayout.Box(enemyImage, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height * .50f - 10)); // Image Box preview
                                if (GUILayout.Button("Edit Image", GUILayout.Height(20), GUILayout.Width(generalBox.width / 2 - 15))) // Image changer Button
                                {
                                    enemy[index].Image = ImageChanger(
                                    index,
                                    "Choose Image",
                                    "Assets/Resources/Image"
                                    );
                                    ItemTabLoader(index);
                                }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("Max HP:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyMaxHP = EditorGUILayout.IntField(enemy[index].enemyMaxHP, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                                GUILayout.Label("Attack:");
                                if (enemySize > 0)
                                { 
                                    enemy[index].enemyAttack = EditorGUILayout.IntField(enemy[index].enemyAttack, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8)); 
                                }
                                else
                                { 
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8)); 
                                }

                                GUILayout.Label("M.Attack:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyMAttack = EditorGUILayout.IntField(enemy[index].enemyMAttack, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                GUILayout.Label("Agility:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyAgility = EditorGUILayout.IntField(enemy[index].enemyAgility, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("Max MP:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyMaxMP = EditorGUILayout.IntField(enemy[index].enemyMaxMP, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                                GUILayout.Label("Defense:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyDefense = EditorGUILayout.IntField(enemy[index].enemyDefense, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                                GUILayout.Label("M.Defense:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyMDefense = EditorGUILayout.IntField(enemy[index].enemyMDefense, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                                GUILayout.Label("Luck:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyLuck = EditorGUILayout.IntField(enemy[index].enemyLuck, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                            GUILayout.EndVertical();
                        GUILayout.EndHorizontal();


                    GUILayout.EndArea(); // End of GeneralSettings Tab
                    #endregion

                Rect rewardsBox = new Rect(5, generalBox.height + 10, firstTabWidth - 200, tabHeight / 5 - 10);
                    #region RewardsBox
                    GUILayout.BeginArea(rewardsBox, tabStyle);
                        GUILayout.BeginVertical();
                            GUILayout.Label("Rewards", EditorStyles.boldLabel);
                            GUILayout.Label("EXP:");
                            if (enemySize > 0)
                            {
                                enemy[index].enemyEXP = EditorGUILayout.IntField(enemy[index].enemyEXP, GUILayout.Width(firstTabWidth - 210), GUILayout.Height(generalBox.height / 8));
                            }
                            else
                            {
                                EditorGUILayout.IntField(-1, GUILayout.Width(firstTabWidth - 210), GUILayout.Height(generalBox.height / 8));
                            }
                            GUILayout.Label("Gold:");
                            if (enemySize > 0)
                            {
                                enemy[index].enemyGold = EditorGUILayout.IntField(enemy[index].enemyGold, GUILayout.Width(firstTabWidth - 210), GUILayout.Height(generalBox.height / 8));
                            }
                            else
                            {
                                EditorGUILayout.IntField(-1, GUILayout.Width(firstTabWidth - 210), GUILayout.Height(generalBox.height / 8));
                            }
                        GUILayout.EndVertical();
                    GUILayout.EndArea();
                    #endregion

                Rect dropItemBox = new Rect(rewardsBox.width + 10, generalBox.height + 10, firstTabWidth + 55 - rewardsBox.width, tabHeight / 5 - 10);
                    #region DropItems
                    GUILayout.BeginArea(dropItemBox, tabStyle);
                        GUILayout.Label("Drop Items", EditorStyles.boldLabel);
                        GUILayout.Space(10);
                        if (enemySize > 0)
                        {
                            if (GUILayout.Button(StringMaker(0), GUILayout.Width(dropItemBox.width - 10), GUILayout.Height(dropItemBox.height / 3 - 15)))
                            {
                                EnemyDropWindow.ShowWindow(enemy[index], 0);
                            }
                            if (GUILayout.Button(StringMaker(1), GUILayout.Width(dropItemBox.width - 10), GUILayout.Height(dropItemBox.height / 3 - 15)))
                            {
                                EnemyDropWindow.ShowWindow(enemy[index], 1);
                            }
                            if (GUILayout.Button(StringMaker(2), GUILayout.Width(dropItemBox.width - 10), GUILayout.Height(dropItemBox.height / 3 - 15)))
                            {
                                EnemyDropWindow.ShowWindow(enemy[index], 2);
                            }
                        }
                        else
                        {
                            if (GUILayout.Button("None", GUILayout.Width(dropItemBox.width - 10), GUILayout.Height(dropItemBox.height / 3 - 15)))
                            {
                            }
                            if (GUILayout.Button("None", GUILayout.Width(dropItemBox.width - 10), GUILayout.Height(dropItemBox.height / 3 - 15)))
                            {
                            }
                            if (GUILayout.Button("None", GUILayout.Width(dropItemBox.width - 10), GUILayout.Height(dropItemBox.height / 3 - 15)))
                            {
                            }
                        }
    
                    GUILayout.EndArea();
                    #endregion

                Rect actionBox = new Rect(5, dropItemBox.height + generalBox.height + 15, firstTabWidth + 60, position.height - (generalBox.height + rewardsBox.height + 60));
                    #region Effects
                    GUILayout.BeginArea(actionBox, tabStyle);
                        GUILayout.Label("Action Patterns", EditorStyles.boldLabel);
                        GUILayout.Space(2);

                        #region Horizontal For Type And Content
                        GUILayout.BeginHorizontal();
                            GUILayout.Label("Skill", GUILayout.Width(actionBox.width * .44f));
                            GUILayout.Label("Condition", GUILayout.Width(actionBox.width * .44f));
                            GUILayout.Label("R", GUILayout.Width(actionBox.width * .12f));

                        GUILayout.EndHorizontal();
                        #endregion
                        
                        #region ScrollView
                        actionScrollPos = GUILayout.BeginScrollView(
                            actionScrollPos,
                            false,
                            true,
                            GUILayout.Width(actionBox.width - 8),
                            GUILayout.Height(actionBox.height * 0.80f)
                            );
                        GUILayout.EndScrollView();
                        #endregion
                    GUILayout.EndArea();
                    #endregion        

            GUILayout.EndArea();
            #endregion // End of Second Tab



            #region Tab 3/3
            //Third Column
            GUILayout.BeginArea(new Rect(position.width - (position.width - firstTabWidth * 2) + 77, 0, firstTabWidth + 25, tabHeight - 15), columnStyle);
            
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
                            TraitWindow.ShowWindow(traits, traitIndex, traitSize[index], TabType.Enemy);
                            
                            traitIndexTemp = traitIndex;
                        }
                    }

                    //Create Empty SO if there isn't any null SO left
                    if ((traits[traitSize[index] - 1].traitName != null && traits[traitSize[index] - 1].traitName != "") && traitSize[index] > 0)
                    {
                        traitIndex = 0;
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.EnemyTraitExplicitDataPath + (index + 1) + "/Trait_");
                    }

                    //Delete All Data Button
                    if (GUILayout.Button("Delete All Data", GUILayout.Width(traitsBox.width * .3f), GUILayout.Height(traitsBox.height * .055f)))
                    {
                        if (EditorUtility.DisplayDialog("Delete All Trait Data", "Are you sure want to delete all Trait Data?", "Yes", "No"))
                        {
                            traitIndex = 0;
                            traitSize[index] = 1;
                            ChangeMaximum<TraitsData>(0, traits, PathDatabase.EnemyTraitExplicitDataPath + (index + 1) + "/Trait_");
                            ChangeMaximum<TraitsData>(1, traits, PathDatabase.EnemyTraitExplicitDataPath + (index + 1) + "/Trait_");
                        }
                    }
                GUILayout.EndArea();
                #endregion //End of TraitboxArea


                //Notes
                Rect notesBox = new Rect(5, traitsBox.height + 10, firstTabWidth + 15, position.height * 2.5f / 8 - 7);
                #region NoteBox
                GUILayout.BeginArea(notesBox, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("Notes", EditorStyles.boldLabel);
                    GUILayout.Space(notesBox.height / 50);
                    if (enemySize > 0)
                    {
                        enemy[index].notes = GUILayout.TextArea(enemy[index].notes, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.9f));
                    }
                    else
                    {
                        GUILayout.TextArea("Null", GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.85f));
                    }
                GUILayout.EndArea();
                #endregion //End of notebox area

            GUILayout.EndArea();
            #endregion // End of third column


        GUILayout.EndArea();
        #endregion
        EditorUtility.SetDirty(enemy[index]);
    }

    #region Features
    ///<summary>
    ///Clears out the displayName list and add it with new value
    ///</summary>
    private void ListReset()
    {
        enemyDisplayName.Clear();
        for (int i = 0; i < enemySize; i++)
        {
            enemyDisplayName.Add(enemy[i].enemyName);
        }
        //Trait Reset
        traitDisplayName.Clear();
        for (int i = 0; i < traitSize[index]; i++)
        {
            traitDisplayName.Add(traits[i].traitName);
        }
    }


    public string StringMaker(int indexx)
    {
        string outputString = "None";
        int selectedToggle = enemy[index].selectedToggle[indexx];

        if (selectedToggle > 0)
        {
            if (selectedToggle == 1)
            {
                outputString = enemy[index].enemyItem[enemy[index].selectedIndex[indexx]];
            }
            else if (selectedToggle == 2)
            {
                outputString = enemy[index].enemyWeapon[enemy[index].selectedIndex[indexx]];
            }
            else
            {
                outputString = enemy[index].enemyArmor[enemy[index].selectedIndex[indexx]];
            }
            outputString += " : 1/" + enemy[index].enemyProbability[indexx].ToString();
        }

        return outputString;
    }
    public override void ItemTabLoader(int index)
    {
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if (enemySize > 0)
            {
                if (enemy[index].Image == null)
                    enemyImage = defTex;
                else
                    enemyImage = TextureToSprite(enemy[index].Image);
            }
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
                    ChangeMaximum<TraitsData>(--traitSize[index], traits, PathDatabase.EnemyTraitExplicitDataPath + (index + 1) + "/Trait_");
                    i--;
                }
            }
        }
    }

    #endregion
}
