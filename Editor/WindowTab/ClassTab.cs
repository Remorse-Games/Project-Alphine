using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ClassTab : BaseTab
{
    //Make a classData List
    List<ClassesData> classes = new List<ClassesData>();
    List<TraitsData> traits = new List<TraitsData>();
    List<SkillsToLearn> skillToLearn = new List<SkillsToLearn>();

    //Make a string list filled with its name
    List<string> skillToLearnNames = new List<string>();
    List<string> traitDisplayName = new List<string>();
    List<string> classesNames = new List<string>();

    GUIStyle classStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    int classSize;

    public static int[] skillToLearnSize;
    public static int[] traitSize;

    public static int index = 0;
    int indexTemp = -1;

    public static int skillIndex = 0;
    public static int skillIndexTemp = -1;

    public static int traitIndex = 0;
    public static int traitIndexTemp = -1;

    //ScrollPos
    Vector2 scrollPos = Vector2.zero;
    Vector2 skillsScrollPos = Vector2.zero;
    Vector2 traitsScrollPos = Vector2.zero;

    #region TempValues
    int classSizeTemp;
    #endregion

    public void Init()
    {
        //Clears List
        classes.Clear();
        skillToLearn.Clear();
        traits.Clear();

        //Resetting each index to 0, so that it won't have error (Index Out Of Range)
        index = 0;
        skillIndex = 0;
        traitIndex = 0;

        LoadGameData<ClassesData>(ref classSize, classes, PathDatabase.ClassRelativeDataPath);
        
        traitSize = new int[classSize]; //Resets Trait Sizing
        LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.ClassTraitRelativeDataPath + (index + 1));

        //Create Folder For SkillData and its sum is based on classSize value
        FolderCreator(classSize, "Assets/Resources/Data/ClassesData", "SkillToLearnData");
        FolderCreator(classSize, "Assets/Resources/Data/ClassesData", "TraitData");

        skillToLearnSize = new int[classSize]; //Resets Skill Sizing
        LoadGameData<SkillsToLearn>(ref skillToLearnSize[index], skillToLearn, PathDatabase.SkillToLearnRelativeDataPath + (index + 1));
        
        //Check if SkillData_(index) is empty, if it is empty then create a SO named Skill_1
        if (skillToLearnSize[index] <= 0)
        {
            skillIndex = 0;
            ChangeMaximum<SkillsToLearn>(++skillToLearnSize[index], skillToLearn, PathDatabase.SkillToLearnExplicitDataPath + (index + 1) + "/SkillToLearn_");
        }

        //Check if TraitsData_(index) is empty, if it is empty then create a SO named Trait_1
        if (traitSize[index] <= 0)
        {
            traitIndex = 0;
            ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ClassTraitExplicitDataPath + (index + 1) + "/Trait_");
        }
        ClearNullScriptableObjects(); //Clear Trait SO without a value
        ListReset();
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
        classStyle = new GUIStyle(GUI.skin.box);
        classStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

        #region Entry Of ClassTab GUILayout
        //Start drawing the whole ClassTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));
        GUILayout.Box(" ", classStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/3
            //First Tab of three
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
                GUILayout.Box("Class", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

                //Scroll View
                #region ScrollView
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
                    index = GUILayout.SelectionGrid(index, classesNames.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * classSize));
                GUILayout.EndScrollView();
                #endregion

                //Happen everytime selection grid is updated
                if (GUI.changed && index != indexTemp)
                {
                    indexTemp = index;
                    traitIndex = skillIndex = 0;
                    traitIndexTemp = skillIndexTemp = -1;
                    
                    //Load SkillToLearnData
                    skillToLearn.Clear();
                    LoadGameData<SkillsToLearn>(ref skillToLearnSize[index], skillToLearn, PathDatabase.SkillToLearnRelativeDataPath + (index + 1));
                    //Check if SkillData folder is empty
                    if (skillToLearnSize[index] <= 0)
                    {
                        ChangeMaximum<SkillsToLearn>(++skillToLearnSize[index], skillToLearn, PathDatabase.SkillToLearnExplicitDataPath + (index + 1) + "/SkillToLearn_");
                        skillIndexTemp = 0;
                    }

                    //Load TraitsData
                    traits.Clear();
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.ClassTraitRelativeDataPath + (index + 1));;
                    //Check if TraitsData folder is empty
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ClassTraitExplicitDataPath + (index + 1) + "/Trait_");
                        traitIndexTemp = 0;
                    }

                    ClearNullScriptableObjects();
                    ListReset();
                    indexTemp = -1;
                }

                classSizeTemp = EditorGUILayout.IntField(classSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
                {
                    classSize = classSizeTemp;
                    index = indexTemp = 0;

                    FolderCreator(classSize, "Assets/Resources/Data/ClassesData", "SkillToLearnData");
                    FolderCreator(classSize, "Assets/Resources/Data/ClassesData", "TraitData");
                    ChangeMaximum<ClassesData>(classSize, classes, PathDatabase.ClassExplicitDataPath);
                    
                    //New SkillSize array length
                    int[] tempArr = new int[skillToLearnSize.Length];
                    for (int i = 0; i < skillToLearnSize.Length; i++)
                        tempArr[i] = skillToLearnSize[i];

                    skillToLearnSize = new int[classSize];

                    #region FindSmallestBetween
                    int smallestValue;
                    if (tempArr.Length < classSize) smallestValue = tempArr.Length;
                    else smallestValue = classSize;
                    #endregion

                    for (int i = 0; i < smallestValue; i++)
                        skillToLearnSize[i] = tempArr[i];

                    //New TraitSize array length
                    tempArr = new int[traitSize.Length];
                    for (int i = 0; i < traitSize.Length; i++)
                        tempArr[i] = traitSize[i];

                    traitSize = new int[classSize];

                    #region FindSmallestBetween
                        if (tempArr.Length < classSize) smallestValue = tempArr.Length;
                        else smallestValue = classSize;
                    #endregion

                    for (int i = 0; i < smallestValue; i++)
                        traitSize[i] = tempArr[i];

                    //Reload Data and Check SO
                    LoadGameData<SkillsToLearn>(ref skillToLearnSize[index], skillToLearn, PathDatabase.SkillToLearnRelativeDataPath + (index + 1));
                    if (skillToLearnSize[index] <= 0)
                    {
                        ChangeMaximum<SkillsToLearn>(++skillToLearnSize[index], skillToLearn, PathDatabase.SkillToLearnExplicitDataPath + (index + 1) + "/SkillToLearn_");
                    }
                    LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.ClassTraitRelativeDataPath + (index + 1));
                    if (traitSize[index] <= 0)
                    {
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ClassTraitExplicitDataPath + (index + 1) + "/Trait_");
                    }
                    ClearNullScriptableObjects();
                    ListReset();
                }
                else if(classSizeTemp <= 0){
                    classSizeTemp = classSize;
                }
            GUILayout.EndArea();
            #endregion

            //Second Column
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 15), columnStyle);

                //General Settings tab
                Rect generalSettingsBox = new Rect(5, 5, firstTabWidth + 60, position.height / 6); //general setings will be 1/6 of the height
                GUILayout.BeginArea(generalSettingsBox, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("General Settings", EditorStyles.boldLabel);
                    GUILayout.Space(generalSettingsBox.height / 5);
                    GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                            GUILayout.Label("Name: ");
                            if (classSize > 0)
                            {
                                classes[index].className = GUILayout.TextField(classes[index].className,
                                                                                GUILayout.Width(generalSettingsBox.width / 2 - 10),
                                                                                GUILayout.Height(generalSettingsBox.height * 0.25f));
                                classesNames[index] = classes[index].className;
                            }
                            else
                            {
                                GUILayout.TextField("Null",
                                                    GUILayout.Width(generalSettingsBox.width / 2 - 10),
                                                    GUILayout.Height(generalSettingsBox.height * 0.25f));
                            }
                        GUILayout.EndVertical();
                        GUILayout.Space(5);
                        GUILayout.BeginVertical();
                            GUILayout.Label("Exp Curve: ");
                            if (classSize > 0)
                            {
                                string bv = classes[index].baseValue.ToString();
                                string xv = classes[index].extraValue.ToString();
                                string aca = classes[index].accelA.ToString();
                                string acb = classes[index].accelB.ToString();
                                if (GUILayout.Button("[ " + bv + ", " + xv + ", " + aca + ", " + acb + " ]", GUILayout.Width(generalSettingsBox.width / 2 - 10),
                                                                        GUILayout.Height(generalSettingsBox.height * 0.25f)))
                                {
                                    ClassExpWindow.ShowWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Button("No Class Selected", GUILayout.Width(generalSettingsBox.width / 2 - 10),
                                                                    GUILayout.Height(generalSettingsBox.height * 0.25f));
                            }
                        GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                GUILayout.EndArea();

                //Parameter Curve Tab
                Rect parameterCurveBox = new Rect(5, generalSettingsBox.height + 10, firstTabWidth + 60, position.height / 2.8f);
                GUILayout.BeginArea(parameterCurveBox, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("Parameter Curve", EditorStyles.boldLabel);
                    float curveWidth = (parameterCurveBox.width - 10) / 4.2f;
                    float curveheight = (parameterCurveBox.height - 70) / 2.7f;
                    GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical(); //Max HP and M.Attack Curve
                            GUILayout.Label("Max HP:");
                            Rect curveValueRangeHP = new Rect(0, 0, 100, 9999);
                            if (classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].maxHPCurve, new Color32(255, 150, 0, 255), curveValueRangeHP, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if (GUILayout.Button("Edit Max Hp", GUILayout.Width(curveWidth)))
                                {
                                    MaxHPWindow maxHPWindow = new MaxHPWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Box("Null", GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                            }
                            GUILayout.Space(10);
                            //Add Under HP (M.Attack)
                            GUILayout.Label("M.Attack");
                            Rect curveValueRangeMAtk = new Rect(0, 0, 100, 250);
                            if (classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].mAttackCurve, new Color32(255, 0, 255, 255), curveValueRangeMAtk, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if (GUILayout.Button("Edit M.Attack", GUILayout.Width(curveWidth)))
                                {
                                    MAttackWindow mAttackWindow = new MAttackWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Box("Null", GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                            }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical(); //Max MP and M.Defense Curve
                            GUILayout.Label("Max MP:");
                            Rect curveValueRangeMP = new Rect(0, 0, 100, 2000);
                            if (classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].maxMPCurve, new Color32(0, 0, 255, 255), curveValueRangeMP, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if (GUILayout.Button("Edit Max MP", GUILayout.Width(curveWidth)))
                                {
                                    MaxMPWindow maxMPWindow = new MaxMPWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Box("Null", GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                            }
                            GUILayout.Space(10);
                            //Add Under MP (M.Defense)
                            GUILayout.Label("M.Defense");
                            Rect curveValueRangeMDef = new Rect(0, 0, 100, 250);
                            if (classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].mDefenseCurve, new Color32(11, 156, 49, 255), curveValueRangeMAtk, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if (GUILayout.Button("Edit M.Defense", GUILayout.Width(curveWidth)))
                                {
                                    MDefenseWindow mDefensekWindow = new MDefenseWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Box("Null", GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                            }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical(); //Attack and Agility
                            GUILayout.Label("Attack:");
                            Rect curveValueRangeAttack = new Rect(0, 0, 100, 250);
                            if (classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].AttackCurve, new Color32(255, 0, 0, 255), curveValueRangeAttack, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if (GUILayout.Button("Edit Attack", GUILayout.Width(curveWidth)))
                                {
                                    AttackWindow attackWindow = new AttackWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Box("Null", GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                            }
                            GUILayout.Space(10);
                            //Add Under Attack (Agility)
                            GUILayout.Label("Agility:");
                            Rect curveValueRangeAgi = new Rect(0, 0, 100, 500);
                            if (classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].agilityCurve, new Color32(0, 255, 255, 255), curveValueRangeAgi, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if (GUILayout.Button("Edit Agility", GUILayout.Width(curveWidth)))
                                {
                                    AgilityWindow agilityWindow = new AgilityWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Box("Null", GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                            }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical(); //Defense and Luck
                            GUILayout.Label("Defense: ");
                            Rect curveValueRangeDefense = new Rect(0, 0, 100, 250);
                            if (classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].DefenseCurve, new Color32(0, 255, 0, 255), curveValueRangeDefense, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if (GUILayout.Button("Edit Defense", GUILayout.Width(curveWidth)))
                                {
                                    DefenseWindow defenseWindow = new DefenseWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Box("Null", GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                            }
                            GUILayout.Space(10);
                            //Add Under Defense (Luck)
                            GUILayout.Label("Luck: ");
                            Rect curveValueRangeLuck = new Rect(0, 0, 100, 500);
                            if (classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].luckCurve, new Color32(255, 200, 0, 255), curveValueRangeLuck, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if (GUILayout.Button("Edit Luck", GUILayout.Width(curveWidth)))
                                {
                                    LuckWindow luckWindow = new LuckWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Box("Null", GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                            }
                        GUILayout.EndVertical();

                    GUILayout.EndHorizontal();
                GUILayout.EndArea();

                //SkillstoLearn TAB
                Rect skillsToLearnBox = new Rect(5, parameterCurveBox.height + generalSettingsBox.height +  20, firstTabWidth + 60, position.height / 2.8f + 20);
                #region Equipment
                ListReset();
                GUILayout.BeginArea(skillsToLearnBox, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("Skills to Learn", EditorStyles.boldLabel);
                    #region Vertical
                    GUILayout.BeginVertical();
                        GUILayout.Space(skillsToLearnBox.height / 10);
                        #region Horizontal
                        GUILayout.BeginHorizontal();
                            #region Title
                            string outputString = "";
                            string val = "";

                            val = string.Format("{0}", " Skill");
                            outputString = PadString("Level", val);
                            val = string.Format("{0}", " Notes");
                            outputString = PadString(outputString, val);

                            GUILayout.Label(outputString);
                            #endregion
                        GUILayout.EndHorizontal();
                        #endregion
                        #region ScrollView
                            skillsScrollPos = GUILayout.BeginScrollView(
                                skillsScrollPos,
                                false,
                                true,
                                GUILayout.Width(firstTabWidth + 50),
                                GUILayout.Height(skillsToLearnBox.height * 0.65f)
                                );
                            GUI.changed = false;
                            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                            skillIndex = GUILayout.SelectionGrid(
                                skillIndex,
                                skillToLearnNames.ToArray(),
                                1,
                                GUILayout.Width(firstTabWidth + 25),
                                GUILayout.Height(position.height / 24 * skillToLearnSize[index]
                                ));
                            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                        GUILayout.EndScrollView();
                        #endregion
                    GUILayout.EndVertical();
                    #endregion
                    
                    //Happen everytime selection grid is updated
                    if (GUI.changed)
                    {
                        if (skillIndex != skillIndexTemp)
                        {
                            
                            skillIndexTemp = skillIndex;
                            SkillsToLearnWindow.ShowWindow(skillToLearn, skillIndex, skillToLearnSize[index]);
                        }
                    }

                    //Create Empty SO if there isn't any null SO left
                    if ((skillToLearn[skillToLearnSize[index] - 1].skillToLearnName != null && skillToLearn[skillToLearnSize[index] - 1].skillToLearnName != "") && skillToLearnSize[index] > 0)
                    {
                        skillIndex = 0;
                        ChangeMaximum<SkillsToLearn>(++skillToLearnSize[index], skillToLearn, PathDatabase.SkillToLearnExplicitDataPath + (index + 1) + "/SkillToLearn_");
                    }

                    Color tempColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    //Delete All Data Button
                    if (GUILayout.Button("Delete All Data", GUILayout.Width(skillsToLearnBox.width * .27f), GUILayout.Height(skillsToLearnBox.height * .1f)))
                    {
                        if (EditorUtility.DisplayDialog("Delete All Skill Data", "Are you sure want to delete all Skill To Learn Data?", "Yes", "No"))
                        {
                            skillIndex = 0;
                            skillToLearnSize[index] = 1;
                            ChangeMaximum<SkillsToLearn>(0, skillToLearn, PathDatabase.SkillToLearnExplicitDataPath + (index + 1) + "/SkillToLearn_");
                            ChangeMaximum<SkillsToLearn>(1, skillToLearn, PathDatabase.SkillToLearnExplicitDataPath + (index + 1) + "/SkillToLearn_");
                        }
                    }
                    GUI.backgroundColor = tempColor;
                GUILayout.EndArea();
                #endregion

            GUILayout.EndArea(); // end of column 2

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
                            TraitWindow.ShowWindow(traits, traitIndex, traitSize[index], TabType.Classes);
                            
                            traitIndexTemp = traitIndex;
                        }
                    }

                    //Create Empty SO if there isn't any null SO left
                    if ((traits[traitSize[index] - 1].traitName != null && traits[traitSize[index] - 1].traitName != "") && traitSize[index] > 0)
                    {
                        traitIndex = 0;
                        ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ClassTraitExplicitDataPath + (index + 1) + "/Trait_");
                    }

                    Color tempColor2 = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    //Delete All Data Button
                    if (GUILayout.Button("Delete All Data", GUILayout.Width(traitsBox.width * .3f), GUILayout.Height(traitsBox.height * .055f)))
                    {
                        if (EditorUtility.DisplayDialog("Delete All Trait Data", "Are you sure want to delete all Trait Data?", "Yes", "No"))
                        {
                            traitIndex = 0;
                            traitSize[index] = 1;
                            ChangeMaximum<TraitsData>(0, traits, PathDatabase.ClassTraitExplicitDataPath + (index + 1) + "/Trait_");
                            ChangeMaximum<TraitsData>(1, traits, PathDatabase.ClassTraitExplicitDataPath + (index + 1) + "/Trait_");
                        }
                    }
                    GUI.backgroundColor = tempColor2;
                GUILayout.EndArea();
                #endregion //End of TraitboxArea


                //Notes
                Rect notesBox = new Rect(5, traitsBox.height + 15, firstTabWidth + 15, position.height * 2.5f / 8);
                #region NoteBox
                GUILayout.BeginArea(notesBox, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("Notes", EditorStyles.boldLabel);
                    GUILayout.Space(notesBox.height / 50);
                    if (classSize > 0)
                    {
                        classes[index].notes = GUILayout.TextArea(classes[index].notes, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.85f));
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
        EditorUtility.SetDirty(classes[index]);
    }



    #region Features
    ///<summary>
    ///Clears out the displayName list and add it with new value
    ///</summary>
    private void ListReset()
    {
        classesNames.Clear();
        for (int i = 0; i < classSize; i++)
        {
            classesNames.Add(classes[i].className);
        }
        skillToLearnNames.Clear();
        for (int i = 0; i < skillToLearnSize[index]; i++)
        {
            skillToLearnNames.Add(skillToLearn[i].skillToLearnName);
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
            for (int i = 0; i < skillToLearnSize[index] - 1; i++)
            {
                if (string.IsNullOrEmpty(skillToLearn[i].skillToLearnName))
                {
                    availableNull = true;
                    for (int j = i; j < skillToLearnSize[index] - 1; j++)
                    {
                        skillToLearn[j].skillToLearnName = skillToLearn[j + 1].skillToLearnName;
                        skillToLearn[j].level = skillToLearn[j + 1].level;
                        skillToLearn[j].notes = skillToLearn[j + 1].notes;
                    }
                    ChangeMaximum<SkillsToLearn>(--skillToLearnSize[index], skillToLearn, PathDatabase.SkillToLearnExplicitDataPath + (index + 1) + "/SkillToLearn_");
                    i--;
                }
            }
        }

        availableNull = true;
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
                    ChangeMaximum<TraitsData>(--traitSize[index], traits, PathDatabase.ClassTraitExplicitDataPath + (index + 1) + "/Trait_");
                    i--;
                }
            }
        }
    }
    public override void ItemTabLoader(int index)
    {

    }

    #endregion
}