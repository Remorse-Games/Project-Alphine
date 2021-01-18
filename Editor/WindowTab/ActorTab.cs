using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.Linq;
public class ActorTab : BaseTab
{
    //Having list of all actor exist in data.
    public List<ActorData> actor = new List<ActorData>();
    public List<TraitsData> traits = new List<TraitsData>();
    public List<TypeEquipmentData> equipmentType = new List<TypeEquipmentData>();
    public List<ArmorData> armors = new List<ArmorData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in ActorData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    public List<string> actorDisplayName = new List<string>();
    public List<string> traitDisplayName = new List<string>();
    public List<string> initialEquipName = new List<string>();

    public string[] equipDisplayName;
    public string[] classDisplayName;
    public string[] tempArmorList;
    //All GUIStyle variable initialization.
    GUIStyle actorStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    #region  DeleteLater

    //Index for selected Class.
    public int selectedClassIndex;

    //How many actor in ChangeMaximum Func
    public int armorSize;
    public int actorSize;
    public int equipmentTypeSize;
    public static int[] traitSize;

    //i don't know about this but i leave this to handle later.
    public static int index = 0;
    public static int traitIndex = 0;
    int indexTemp = -1;
    public static int traitIndexTemp = -1;
    public static int typeIndex = 0;
    public static int typeIndexTemp = -1;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;
    Vector2 equipmentScrollPos = Vector2.zero;
    Vector2 traitsScrollPos = Vector2.zero;

    //Image Area.
    Texture2D faceImage;
    Texture2D characterImage;
    Texture2D battlerImage;
    #endregion

    #region TempValues
    //we use actorSizeTemp because when
    //value updated immediately (since OnGUI work when we do input
    //either it mouse or keyboard) the real actor size will be updated and
    //cause some errors. to prevent it we add this temp value.
    //maybe we will change in the future.
    public int actorSizeTemp;
    #endregion


    public void Init()
    {
        //Clears List
        actor.Clear();
        traits.Clear();
        armors.Clear();
        equipmentType.Clear();

        //Resetting each index to 0, so that it won't have error (Index Out Of Range)
        index = 0;
        typeIndex = 0;
        traitIndex = 0;

        //Load Every List needed in ActorTab
        LoadGameData<ActorData>(ref actorSize, actor, PathDatabase.ActorRelativeDataPath);

        traitSize = new int[actorSize]; //Resets Trait Sizing
        LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.ActorTraitRelativeDataPath + (index + 1));

        LoadGameData<TypeEquipmentData>(ref equipmentTypeSize, equipmentType, PathDatabase.EquipmentRelativeDataPath);
        LoadGameData<ArmorData>(ref armorSize, armors, PathDatabase.ArmorTabRelativeDataPath);
        LoadClassList();

        //Create Folder For TraitsData and its sum is based on actorSize value
        FolderCreator(actorSize, "Assets/Resources/Data/ActorData", "TraitData");

        //Check if TraitsData_(index) is empty, if it is empty then create a SO named Trait_1
        if (traitSize[index] <= 0)
        {
            traitIndex = 0;
            ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ActorTraitExplicitDataPath + (index + 1) + "/Trait_");
        }
        ClearNullScriptableObjects(); //Clear Trait SO without a value
        ListReset(); //Resets List
    }

    public void OnRender(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in ActorTab itself.
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
        actorStyle = new GUIStyle(GUI.skin.box);
        actorStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

        #region Entry Of ActorTab GUILayout
        //Start drawing the whole ActorTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the ActorTab? yes, this one.
            GUILayout.Box(" ", actorStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/3
            //First Tab of three
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
                GUILayout.Box("Actors", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

                //Scroll View
                #region ScrollView
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
                    index = GUILayout.SelectionGrid(
                        index, 
                        actorDisplayName.ToArray(), 
                        1, 
                        GUILayout.Width(firstTabWidth - 20), 
                        GUILayout.Height(position.height / 24 * actorSize
                        ));
                GUILayout.EndScrollView();
                #endregion

                //Happen everytime selection grid is updated
                if (GUI.changed && index != indexTemp)
                {
                    //Index Resetting
                    indexTemp = index;
                    traitIndex = typeIndex = 0;
                    traitIndexTemp = typeIndexTemp = -1;

                    ItemTabLoader(index);

                    //Load TraitsData
                    traits.Clear();
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.ActorTraitRelativeDataPath + (index + 1));
                    //Check if TraitsData folder is empty
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ActorTraitExplicitDataPath + (index + 1) + "/Trait_");
                        traitIndexTemp = 0;
                    }
                    ClearNullScriptableObjects();

                    //Resets the armor index array length
                    if (actor[index].allArmorIndexes == null)
                    {
                        actor[index].allArmorIndexes = new int[equipmentTypeSize];
                    }
                    ListReset();
                    indexTemp = -1;
                }

                //Int field of change Maximum
                actorSizeTemp = EditorGUILayout.IntField(actorSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
                {
                    actorSize = actorSizeTemp;
                    index = indexTemp = 0;
                    FolderCreator(actorSize, "Assets/Resources/Data/ActorData", "TraitData");
                    ChangeMaximum<ActorData>(actorSize, actor, PathDatabase.ActorExplicitDataPath);
                    
                    //New TraitSize array length
                    int[] tempArr = new int[traitSize.Length];
                    for (int i = 0; i < traitSize.Length; i++)
                        tempArr[i] = traitSize[i];

                    traitSize = new int[actorSize];

                    #region FindSmallestBetween
                        int smallestValue;
                        if (tempArr.Length < actorSize) smallestValue = tempArr.Length;
                        else smallestValue = actorSize;
                    #endregion

                    for (int i = 0; i < smallestValue; i++)
                        traitSize[i] = tempArr[i];

                    //Reload Data and Check SO
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.ActorTraitRelativeDataPath + (index + 1));
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ActorTraitExplicitDataPath + (index + 1) + "/Trait_");
                    }
                    //Resets the armor index array length
                    if (actor[index].allArmorIndexes == null)
                    {
                        actor[index].allArmorIndexes = new int[equipmentTypeSize];
                    }
                    ClearNullScriptableObjects();
                    ListReset();
                }
            GUILayout.EndArea();
            #endregion

            #region Tab 2/3
            //second Column
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 15), columnStyle);

            //GeneralSettings tab
                Rect generalBox = new Rect(5, 5, firstTabWidth + 60, position.height / 3 - 50);

                #region GeneralSettings
                GUILayout.BeginArea(generalBox, tabStyle); //Start of general settings tab
                    GUILayout.Label("General Settings", EditorStyles.boldLabel); //general settings label
                    #region Vertical
                    GUILayout.BeginVertical();
                        #region Horizontal
                        GUILayout.BeginHorizontal();
                            #region Name Classes
                            GUILayout.BeginVertical(); //Name label, name field, class label, and class popup
                                GUILayout.Label("Name:");
                                if (actorSize > 0)
                                {
                                    actor[index].actorName = GUILayout.TextField(actor[index].actorName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                    actorDisplayName[index] = actor[index].actorName;
                                }
                                else
                                { 
                                    GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8)); 
                                }
                                GUILayout.Space(generalBox.height / 20);
                                GUILayout.Label("Class:");
                                selectedClassIndex = EditorGUILayout.Popup(selectedClassIndex, classDisplayName, GUILayout.Height(generalBox.height / 8), GUILayout.Width(generalBox.width / 2 - 15));
                            GUILayout.EndVertical(); //Name label, name field, class label, and class popup (ending)
                            #endregion
                            #region Names Classes
                            GUILayout.BeginVertical(); //Nickname label, nickname field, initial level and max level label and field
                                GUILayout.Label("Nickname:");
                                if (actorSize > 0)
                                { actor[index].actorNickname = GUILayout.TextField(actor[index].actorNickname, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8)); }
                                else
                                {GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8)); }
                                GUILayout.Space(generalBox.height / 20);
                                #region Level Area
                                GUILayout.BeginHorizontal();
                                    #region Initial Level
                                    GUILayout.BeginVertical();
                                        GUILayout.Label("Initial Level:");
                                        if (actorSize > 0)
                                           {actor[index].initLevel = EditorGUILayout.IntField(actor[index].initLevel, GUILayout.Width(generalBox.width / 4 - 20), GUILayout.Height(generalBox.height / 8));}
                                        else
                                            {EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 20), GUILayout.Height(generalBox.height / 8));}
                                    GUILayout.EndVertical();
                                    #endregion
                                    #region MaxLevel
                                    GUILayout.BeginVertical();
                                        GUILayout.Label("Max Level:");
                                        if (actorSize > 0)
                                            {actor[index].maxLevel = EditorGUILayout.IntField(actor[index].maxLevel, GUILayout.Width(generalBox.width / 4 - 20), GUILayout.Height(generalBox.height / 8));}
                                        else
                                            {EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 20), GUILayout.Height(generalBox.height / 8));}
                                    GUILayout.EndVertical();
                                    #endregion
                                GUILayout.EndHorizontal();
                                #endregion
                            GUILayout.EndVertical(); //Nickname label, nickname field, initial level and max level label and field (ending)
                            #endregion
                        GUILayout.EndHorizontal();
                        #endregion
                        GUILayout.Label("Profile:");
                        if (actorSize > 0)
                            {
                                actor[index].description = GUILayout.TextArea
                                    (
                                    actor[index].description,
                                    GUILayout.Width(firstTabWidth + 50), 
                                    GUILayout.Height(generalBox.height / 5)
                                    );
                            }
                        else
                            {
                                GUILayout.TextArea
                                ("Null", 
                                GUILayout.Width(firstTabWidth + 50), 
                                GUILayout.Height(generalBox.height / 5)
                                );
                            }
                    GUILayout.EndVertical();
                    #endregion
                GUILayout.EndArea();
                #endregion


                //Images tab
                Rect imageBox = new Rect(5, generalBox.height + 10, firstTabWidth + 60, position.height / 3 - 30); //Second Row
                #region ImageArea
                GUILayout.BeginArea(imageBox, tabStyle); //Image Tab
                    GUILayout.Space(2);
                    GUILayout.Label("Images", EditorStyles.boldLabel); //Image Label
                    GUILayout.Space(imageBox.height / 15);
                    //Three image parts
                    #region ImagePart
                    GUILayout.BeginHorizontal();
                        #region Face
                        GUILayout.BeginVertical();
                            GUILayout.Label("Face:");
                            GUILayout.Box(faceImage, GUILayout.Width(imageBox.width / 3 - 10), GUILayout.Height(imageBox.width / 3 - 10));
                            if (GUILayout.Button("Edit Face", GUILayout.Height(imageBox.height / 10), GUILayout.Width(imageBox.width / 3 - 10))) 
                            {
                                    actor[index].face = ImageChanger(
                                    index,
                                    "Choose Face", 
                                    "Assets/Resources/Image"
                                    );
                                    ItemTabLoader(index);
                            }
                        GUILayout.EndVertical();
                        #endregion
                        #region Character
                        GUILayout.BeginVertical();
                            GUILayout.Label("Character:");
                            GUILayout.Box(characterImage, GUILayout.Width(imageBox.width / 3 - 10), GUILayout.Height(imageBox.width / 3 - 10));
                            if (GUILayout.Button("Edit Character", GUILayout.Height(imageBox.height / 10), GUILayout.Width(imageBox.width / 3 - 10))) 
                            {
                                    actor[index].characterWorld = ImageChanger(
                                    index,                                    
                                    "Choose Character",
                                    "Assets/Resources/Image"
                                    );
                                    ItemTabLoader(index);
                            }
                        GUILayout.EndVertical();
                        #endregion
                        #region SV Battler
                        GUILayout.BeginVertical();
                            GUILayout.Label("[SV] Battler: ");
                            GUILayout.Box(battlerImage, GUILayout.Width(imageBox.width / 3 - 10), GUILayout.Height(imageBox.width / 3 - 10));
                            if (GUILayout.Button("Edit Battler", GUILayout.Height(imageBox.height / 10), GUILayout.Width(imageBox.width / 3 - 10))) 
                            {
                                    actor[index].battler = ImageChanger(
                                    index, 
                                    "Choose Face", 
                                    "Assets/Resources/Image" 
                                    );
                                    ItemTabLoader(index);

                            }
                            GUILayout.EndVertical();
                                            #endregion
                                        GUILayout.EndHorizontal();
                                        #endregion
                                    GUILayout.EndArea();
                                    #endregion

                
                //Initial Equipment
                Rect equipmentBox = new Rect(5, generalBox.height + imageBox.height + 20, firstTabWidth + 60, position.height / 3 + 20);
                #region Equipment
                GUILayout.BeginArea(equipmentBox, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("Initial Equipment", EditorStyles.boldLabel);
                    #region Vertical
                    GUILayout.BeginVertical();
                        GUILayout.Space(equipmentBox.height / 10);
                        #region Horizontal
                        GUILayout.BeginHorizontal();
                            GUILayout.Label(PadString("Type", string.Format("{0}", " Equipment Item")), GUILayout.Width(equipmentBox.width));
                            //GUILayout.Label("Equipment Item", GUILayout.Width(equipmentBox.width * 0.35f));
                        GUILayout.EndHorizontal();
                        #endregion
                        #region ScrollView
                            equipmentScrollPos = GUILayout.BeginScrollView(
                                                equipmentScrollPos,
                                                false,
                                                true,
                                                GUILayout.Width(firstTabWidth + 50),
                                                GUILayout.Height(equipmentBox.height * 0.7f)
                                                );
                            GUI.changed = false;
                            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                            typeIndex = GUILayout.SelectionGrid(
                                                typeIndex, 
                                                initialEquipName.ToArray(), 
                                                1, 
                                                GUILayout.Width(equipmentBox.width * .90f),
                                                GUILayout.Height(position.height / 24 * equipmentTypeSize)
                                                );
                            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                        GUILayout.EndScrollView();
                        #endregion

                        if (GUI.changed)
                        {
                            if (typeIndex != typeIndexTemp)
                            {
                                LoadArmorList(typeIndex);
                                InitialEquipmentWindow.ShowWindow(actor[index], equipmentType[typeIndex].dataName, tempArmorList, typeIndex);
                                typeIndexTemp = typeIndex;
                            }
                        }
                    GUILayout.EndVertical();
                    #endregion
                GUILayout.EndArea();
                #endregion
            GUILayout.EndArea();
            #endregion


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
                            TraitWindow.ShowWindow(traits, traitIndex, traitSize[index], TabType.Actor);
                            
                            traitIndexTemp = traitIndex;
                        }
                    }

                    //Create Empty SO if there isn't any null SO left
                    if ((traits[traitSize[index] - 1].traitName != null && traits[traitSize[index] - 1].traitName != "") && traitSize[index] > 0)
                    {
                        traitIndex = 0;
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ActorTraitExplicitDataPath + (index + 1) + "/Trait_");
                    }

                    //Delete All Data Button
                    if (GUILayout.Button("Delete All Data", GUILayout.Width(traitsBox.width * .3f), GUILayout.Height(traitsBox.height * .055f)))
                    {
                        if (EditorUtility.DisplayDialog("Delete All Trait Data", "Are you sure want to delete all Trait Data?", "Yes", "No"))
                        {
                            traitIndex = 0;
                            traitSize[index] = 1;
                            ChangeMaximum<TraitsData>(0, traits, PathDatabase.ActorTraitExplicitDataPath + (index + 1) + "/Trait_");
                            ChangeMaximum<TraitsData>(1, traits, PathDatabase.ActorTraitExplicitDataPath + (index + 1) + "/Trait_");
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
                    if (actorSize > 0)
                    {
                        actor[index].notes = GUILayout.TextArea(actor[index].notes, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.9f));
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

        EditorUtility.SetDirty(actor[index]);
    }

    ///<summary>
    ///Clears out the displayName list and add it with new value
    ///</summary>
    private void ListReset()
    {
        //Actor Reset
        actorDisplayName.Clear();
        for (int i = 0; i < actorSize; i++)
        {
            actorDisplayName.Add(actor[i].actorName);
        }
        //Equip Reset
        initialEquipName.Clear();
        if(actor[index].allArmorIndexes == null)
        {
            actor[index].allArmorIndexes = new int[equipmentTypeSize];
        }

        for (int i = 0; i < equipmentTypeSize; i++)
        {
            LoadArmorList(i);
            string val = string.Format("{0}", tempArmorList[actor[index].allArmorIndexes[i]]);
            string outputString = PadString(equipmentType[i].dataName, val);
            initialEquipName.Add(outputString);
        }
        //Trait Reset
        traitDisplayName.Clear();
        for (int i = 0; i < traitSize[index]; i++)
        {
            traitDisplayName.Add(traits[i].traitName);
        }
    }
    private void LoadClassList()
    {
        ClassesData[] classData = Resources.LoadAll<ClassesData>(PathDatabase.ClassRelativeDataPath);
        classDisplayName = new string[classData.Length];
        for (int i = 0; i < classDisplayName.Length; i++)
        {
            classDisplayName[i] = classData[i].className;
        }
    }

    private void LoadArmorList(int searchedIndex)
    {
        ArmorData[] armorData = Resources.LoadAll<ArmorData>(PathDatabase.ArmorTabRelativeDataPath);
        tempArmorList = new string[armorData.Length + 10];

        int j = 1;
        tempArmorList[0] = "None";
        for (int i = 0; i < armorData.Length; i++)
        {
            if (armorData[i].selectedArmorEquipmentIndex == searchedIndex)
            {
                tempArmorList[j++] = armorData[i].armorName;
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
                    ChangeMaximum<TraitsData>(--traitSize[index], traits, PathDatabase.ActorTraitExplicitDataPath + (index + 1) + "/Trait_");
                    i--;
                }
            }
        }
    }

    public override void ItemTabLoader(int index)
    {
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if (actorSize > 0)
            {
                if (actor[index].face == null)
                    faceImage = defTex;
                else
                    faceImage = TextureToSprite(actor[index].face);


                if (actor[index].characterWorld == null)
                    characterImage = defTex;
                else
                    characterImage = TextureToSprite(actor[index].characterWorld);


                if (actor[index].battler == null)
                    battlerImage = defTex;
                else
                    battlerImage = TextureToSprite(actor[index].battler);
            }
        }
    }
}
