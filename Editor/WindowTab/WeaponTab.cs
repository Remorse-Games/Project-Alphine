using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class WeaponTab : BaseTab
{
    //Having list of all weapons exist in data.
    public List<WeaponData> weapon = new List<WeaponData>();
    public List<TraitsData> traits = new List<TraitsData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in weaponData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> weaponDisplayName = new List<string>();
    List<string> traitDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle weaponStyle;

    public string[] weaponTypeList;

    public string[] weaponAnimationList =
    {
        "Normal Attack",
        "None",
        "Hit Pyhsical",
        "Other... (Add More Manually)",
    };

    //How many weapon in ChangeMaximum Func
    public int weaponSize;
    public int weaponSizeTemp;
    public static int[] traitSize;

    //i don't know about this but i leave this to handle later.
    public static int index = 0;
    int indexTemp = -1;
    public static int traitIndex = 0;
    public static int traitIndexTemp = -1;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;
    Vector2 equipmentScrollPos = Vector2.zero;
    Vector2 traitsScrollPos = Vector2.zero;

    //Image Area.
    Texture2D weaponIcon;

    public void Init()
    {
        //Clears List
        weapon.Clear();
        traits.Clear();

        //Resetting each index to 0, so that it won't have error (Index Out Of Range)
        index = 0;
        traitIndex = 0;

        //Getting Actual weaponSize
        LoadGameData<WeaponData>(ref weaponSize, weapon, PathDatabase.WeaponTabRelativeDataPath);

        traitSize = new int[weaponSize]; //Resets Trait Sizing
        LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.WeaponTraitRelativeDataPath + (index + 1));

        //Create Folder For TraitData and its sum is based on weaponSize value
        FolderCreator(weaponSize, "Assets/Resources/Data/WeaponData", "TraitData");

        //Check if TraitData_(index) is empty, if it is empty then create a SO named Trait_1
        if (traitSize[index] <= 0)
        {
            traitIndex = 0;
            ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.WeaponTraitExplicitDataPath + (index + 1) + "/Trait_");
        }

        LoadWeaponList();
        ClearNullScriptableObjects();
        ListReset();
    }

    public void OnRender(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in weaponsTab itself.
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
        weaponStyle = new GUIStyle(GUI.skin.box);
        weaponStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

        #region Entry Of weaponsTab GUILayout
        //Start drawing the whole WeaponTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the weaponsTab? yes, this one.
            GUILayout.Box(" ", weaponStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/3
            //First Tab of three
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
                
                GUILayout.Box("Weapons", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

                //Scroll View
                #region ScrollView
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
                index = GUILayout.SelectionGrid(index, weaponDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * weaponSize));
                GUILayout.EndScrollView();
                #endregion

                //Happen everytime selection grid is updated
                if (GUI.changed && index != indexTemp)
                {
                    indexTemp = index;
                    traitIndex = traitIndexTemp = 0;
                    
                    //Load TraitsData
                    traits.Clear();
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.WeaponTraitRelativeDataPath + (index + 1));
                    
                    //Check if TraitsData folder is empty
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.WeaponTraitExplicitDataPath + (index + 1) + "/Trait_");
                        traitIndexTemp = 0;
                    }
                    ClearNullScriptableObjects();

                    ListReset();

                    ItemTabLoader(index);
                    indexTemp = -1;
                }

                // Change Maximum field and button
                weaponSizeTemp = EditorGUILayout.IntField(weaponSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
                {
                    weaponSize = weaponSizeTemp;
                    index = indexTemp = 0;
                    FolderCreator(weaponSize, "Assets/Resources/Data/WeapomData", "TraitData");
                    ChangeMaximum<WeaponData>(weaponSize, weapon, PathDatabase.WeaponTabExplicitDataPath);
                    
                    //New TraitSize array length
                    int[] tempArr = new int[traitSize.Length];
                    for (int i = 0; i < traitSize.Length; i++)
                        tempArr[i] = traitSize[i];

                    traitSize = new int[weaponSize];

                    #region FindSmallestBetween
                        int smallestValue;
                        if (tempArr.Length < weaponSize) smallestValue = tempArr.Length;
                        else smallestValue = weaponSize;
                    #endregion

                    for (int i = 0; i < smallestValue; i++)
                        traitSize[i] = tempArr[i];

                    //Reload Data and Check SO
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.WeaponTraitRelativeDataPath + (index + 1));
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.WeaponTraitExplicitDataPath + (index + 1) + "/Trait_");
                    }

                    ClearNullScriptableObjects();
                    ListReset();
                }

            GUILayout.EndArea();
            #endregion // End Of First Tab

            #region Tab 2/3
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 25), columnStyle);

                Rect generalBox = new Rect(5, 5, firstTabWidth + 60, position.height / 4 + 120);
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
                                    if (weaponSize > 0)
                                    {
                                        weapon[index].weaponName = GUILayout.TextField(weapon[index].weaponName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                        weaponDisplayName[index] = weapon[index].weaponName;
                                    }
                                    else
                                    {
                                        GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                    }
                                GUILayout.EndVertical();
                                #endregion
                                #region Icon
                                GUILayout.BeginArea(new Rect(generalBox.width / 2 - 3, generalBox.height * .05f + 5, firstTabWidth - 220, position.height / 2)); // Icon Area
                                    GUILayout.BeginHorizontal();
                                        GUILayout.BeginVertical();
                                            GUILayout.Label("Icon:"); // Icon label
                                        GUILayout.EndVertical();

                                        GUILayout.BeginVertical();
                                            GUILayout.Box(weaponIcon, GUILayout.Width(61), GUILayout.Height(61)); // Icon Box preview
                                            if (GUILayout.Button("Edit Icon", GUILayout.Height(20), GUILayout.Width(61))) // Icon changer Button
                                            {
                                                weapon[index].Icon = ImageChanger(
                                                index,
                                                "Choose Icon",
                                                "Assets/Resources/Image"
                                                );
                                                ItemTabLoader(index);
                                            }
                                        GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                GUILayout.EndArea();
                                #endregion
                            GUILayout.EndHorizontal();
                            #endregion

                            GUILayout.Space(30);

                            #region Description
                            GUILayout.Label("Description:"); // Description label
                            if (weaponSize > 0)
                            {
                                weapon[index].weaponDescription = GUILayout.TextArea(weapon[index].weaponDescription, GUILayout.Width(firstTabWidth + 53), GUILayout.Height(generalBox.height / 5 + 5));
                            }
                            else
                            {
                                GUILayout.TextArea("Null", GUILayout.Width(firstTabWidth + 53), GUILayout.Height(generalBox.height / 5 + 5));
                            }
                            #endregion
                            GUILayout.Space(5);

                            #region weaponType 
                            GUILayout.BeginHorizontal();
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Weapon Type:"); // Weapon Type class label
                                    if (weaponSize > 0)
                                    {
                                        weapon[index].selectedweaponTypeIndex = EditorGUILayout.Popup(weapon[index].selectedweaponTypeIndex, weaponTypeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                    }
                                    else
                                    {
                                        EditorGUILayout.Popup(0, weaponTypeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                    }
                                GUILayout.EndVertical();
                                #endregion
                                #region Price
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Price:"); // Price label
                                    if (weaponSize > 0)
                                    { 
                                        weapon[index].weaponPrice = EditorGUILayout.IntField(weapon[index].weaponPrice, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9)); 
                                    }
                                    else
                                    { 
                                        EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9)); 
                                    }
                                GUILayout.EndVertical();
                                
                                GUILayout.Space(generalBox.width / 4 - 2);
                            GUILayout.EndHorizontal();
                            #endregion  

                            GUILayout.Space(12);
                            DrawUILine(Color.black, 7, 2);

                        GUILayout.EndVertical();
                        #endregion
                    GUILayout.EndArea(); // End of GeneralSettings Tab
                    #endregion

                Rect parameterChangesBox = new Rect(5, generalBox.height + 10, firstTabWidth + 60, position.height / 4 - 65);
                    #region ParameterChangesBox
                    GUILayout.BeginArea(parameterChangesBox, tabStyle);
                        #region Vertical
                        GUILayout.BeginVertical();
                            GUILayout.Label("Parameter Changes", EditorStyles.boldLabel);

                            #region InitialLevel Success Repeat TPGain
                            GUILayout.BeginHorizontal();
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Attack:");
                                    if (weaponSize > 0)
                                    { weapon[index].weaponAttack = EditorGUILayout.IntField(weapon[index].weaponAttack, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                    else
                                    { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                GUILayout.EndVertical();

                                GUILayout.BeginVertical();
                                    GUILayout.Label("Defense:");
                                    if (weaponSize > 0)
                                    { weapon[index].weaponDefense = EditorGUILayout.IntField(weapon[index].weaponDefense, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                    else
                                    { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                GUILayout.EndVertical();

                                GUILayout.BeginVertical();
                                    GUILayout.Label("M.Attack:");
                                    if (weaponSize > 0)
                                    { weapon[index].weaponMAttack = EditorGUILayout.IntField(weapon[index].weaponMAttack, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                    else
                                    { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                GUILayout.EndVertical();

                                GUILayout.BeginVertical();
                                    GUILayout.Label("M.Defense:");
                                    if (weaponSize > 0)
                                    { weapon[index].weaponMDefense = EditorGUILayout.IntField(weapon[index].weaponMDefense, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                    else
                                    { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                GUILayout.EndVertical();

                            GUILayout.EndHorizontal();
        #endregion

                            #region Agility Luck MaxHP MaxMP
                            GUILayout.BeginHorizontal();
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Agility:");
                                    if (weaponSize > 0)
                                    { weapon[index].weaponAgility = EditorGUILayout.IntField(weapon[index].weaponAgility, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                    else
                                    { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                GUILayout.EndVertical();

                                GUILayout.BeginVertical();
                                    GUILayout.Label("Luck:");
                                    if (weaponSize > 0)
                                    { weapon[index].weaponLuck = EditorGUILayout.IntField(weapon[index].weaponLuck, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                    else
                                    { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                GUILayout.EndVertical();

                                GUILayout.BeginVertical();
                                    GUILayout.Label("Max HP:");
                                    if (weaponSize > 0)
                                    { weapon[index].weaponMaxHP = EditorGUILayout.IntField(weapon[index].weaponMaxHP, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                    else
                                    { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                GUILayout.EndVertical();

                                GUILayout.BeginVertical();
                                    GUILayout.Label("Max MP:");
                                    if (weaponSize > 0)
                                    { weapon[index].weaponMaxMP = EditorGUILayout.IntField(weapon[index].weaponMaxMP, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                    else
                                    { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
                                GUILayout.EndVertical();

                            GUILayout.EndHorizontal();
                            #endregion

                        GUILayout.EndVertical();
                        #endregion

                    GUILayout.EndArea();
                    #endregion // End Of Invocation Settings


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
                            TraitWindow.ShowWindow(traits, traitIndex, traitSize[index], TabType.Weapon);
                            
                            traitIndexTemp = traitIndex;
                        }
                    }

                    //Create Empty SO if there isn't any null SO left
                    if ((traits[traitSize[index] - 1].traitName != null && traits[traitSize[index] - 1].traitName != "") && traitSize[index] > 0)
                    {
                        traitIndex = 0;
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.WeaponTraitExplicitDataPath + (index + 1) + "/Trait_");
                    }

                    //Delete All Data Button
                    if (GUILayout.Button("Delete All Data", GUILayout.Width(traitsBox.width * .3f), GUILayout.Height(traitsBox.height * .055f)))
                    {
                        if (EditorUtility.DisplayDialog("Delete All Trait Data", "Are you sure want to delete all Trait Data?", "Yes", "No"))
                        {
                            traitIndex = 0;
                            traitSize[index] = 1;
                            ChangeMaximum<TraitsData>(0, traits, PathDatabase.WeaponTraitExplicitDataPath + (index + 1) + "/Trait_");
                            ChangeMaximum<TraitsData>(1, traits, PathDatabase.WeaponTraitExplicitDataPath + (index + 1) + "/Trait_");
                        }
                    }
                GUILayout.EndArea();
                #endregion //End of TraitboxArea

                //Notes
                Rect notesBox = new Rect(5, traitsBox.height + 15, firstTabWidth + 15, position.height * 2.5f / 8);
                    #region NoteBox
                    GUILayout.BeginArea(notesBox, tabStyle);
                        GUILayout.Space(2);
                        GUILayout.Label("Notes", EditorStyles.boldLabel);
                        GUILayout.Space(notesBox.height / 50);
                        if (weaponSize > 0)
                        {
                            weapon[index].notes = GUILayout.TextArea(weapon[index].notes, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.9f));
                        }
                        else
                        {
                            GUILayout.TextArea("Null", GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.85f));
                        }
                    GUILayout.EndArea();
                    #endregion //End of notebox area

            GUILayout.EndArea();
            #endregion // End of third column



        GUILayout.EndArea(); // End of drawing WeaponTab
        #endregion
        EditorUtility.SetDirty(weapon[index]);
    }


    #region Features

    ///<summary>
    ///Clears out the displayName list and add it with new value
    ///</summary>
    private void ListReset()
    {
        weaponDisplayName.Clear();
        for (int i = 0; i < weaponSize; i++)
        {
            weaponDisplayName.Add(weapon[i].weaponName);
        }
        //Trait Reset
        traitDisplayName.Clear();
        for (int i = 0; i < traitSize[index]; i++)
        {
            traitDisplayName.Add(traits[i].traitName);
        }
    }

    private void LoadWeaponList()
    {
        TypeWeaponData[] weaponData = Resources.LoadAll<TypeWeaponData>(PathDatabase.WeaponRelativeDataPath);
        weaponTypeList = new string[weaponData.Length];
        for (int i = 0; i < weaponTypeList.Length; i++)
        {
            weaponTypeList[i] = weaponData[i].dataName;
        }
    }
    public override void ItemTabLoader(int index)
    {
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if(weaponSize > 0)
            {
                if (weapon[index].Icon == null)
                    weaponIcon = defTex;
                else
                    weaponIcon = TextureToSprite(weapon[index].Icon);
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
                    ChangeMaximum<TraitsData>(--traitSize[index], traits, PathDatabase.WeaponTraitExplicitDataPath + (index + 1) + "/Trait_");
                    i--;
                }
            }
        }
    }


    #endregion
}
