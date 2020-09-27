using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ClassTab
{
    //Make a classData List
    List<ClassesData> classes = new List<ClassesData>();
    List<string> classesNames = new List<string>();
    //make a string list filled with its name

    GUIStyle classStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    int classSize;
    int index = 0;
    int indexTemp = -1;

    //ScrollPos
    Vector2 scrollPos = Vector2.zero;

    #region TempValues
        int classSizeTemp;
    #endregion


    public void Init(Rect position)
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
        if(EditorGUIUtility.isProSkin)
        tabStyle.normal.background = CreateTexture(1, 1, new Color32(76, 76, 76, 200));
        else
        tabStyle.normal.background = CreateTexture(1,1, new Color32(200,200,200,200));

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
                    index = GUILayout.SelectionGrid(index, classesNames.ToArray() , 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * classSize));
                GUILayout.EndScrollView();
                #endregion

                //Happen everytime selection grid is updated
                if (GUI.changed && index != indexTemp)
                {
                    indexTemp = index;
                    indexTemp = -1;
                }

                classSizeTemp = EditorGUILayout.IntField(classSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
                {
                    ChangeMaximum(classSize);
                }
            GUILayout.EndArea();
            #endregion

            //Second Column
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 15), columnStyle);

                //General Settings tab
                Rect generalSettingsClass = new Rect(5, 5, firstTabWidth + 60, position.height / 6 ); //general setings will be 1/6 of the height
                GUILayout.BeginArea(generalSettingsClass, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("General Settings", EditorStyles.boldLabel);
                    GUILayout.Space(generalSettingsClass.height/5);
                    GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                            GUILayout.Label("Name: ");
                            if(classSize > 0)
                            {
                                classes[index].className = GUILayout.TextField(classes[index].className,
                                                                                GUILayout.Width(generalSettingsClass.width/2 - 10),
                                                                                GUILayout.Height(generalSettingsClass.height*0.25f));
                                classesNames[index] = classes[index].className;
                            }
                            else
                            {
                                GUILayout.TextField("Null",
                                                    GUILayout.Width(generalSettingsClass.width/2 - 10),
                                                    GUILayout.Height(generalSettingsClass.height*0.25f));
                            }
                        GUILayout.EndVertical();
                        GUILayout.Space(5);
                        GUILayout.BeginVertical();
                            GUILayout.Label("Exp Curve: ");
                            if(classSize > 0)
                            {
                                string bv = classes[index].baseValue.ToString();
                                string xv = classes[index].extraValue.ToString();
                                string aca = classes[index].accelA.ToString();
                                string acb = classes[index].accelB.ToString();
                                if(GUILayout.Button("[ "+bv+", "+xv+", "+aca+", "+acb+" ]" , GUILayout.Width(generalSettingsClass.width/2 - 10),
                                                                          GUILayout.Height(generalSettingsClass.height*0.25f)))
                                {
                                    ClassExpWindow.ShowWindow(classes[index]);
                                }
                            }
                            else
                            {
                                GUILayout.Button("No Class Selected", GUILayout.Width(generalSettingsClass.width/2 - 10),
                                                                      GUILayout.Height(generalSettingsClass.height*0.25f));
                            }
                        GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                GUILayout.EndArea();

                //Parameter Curve Tab
                Rect parameterCurveBox = new Rect(5, generalSettingsClass.height + 10, firstTabWidth+60, position.height/2.8f);
                GUILayout.BeginArea(parameterCurveBox, tabStyle);
                    GUILayout.Space(2);
                    GUILayout.Label("Parameter Curve", EditorStyles.boldLabel);
                    float curveWidth = (parameterCurveBox.width-10)/4.2f;
                    float curveheight = (parameterCurveBox.height - 70)/2.7f;
                    GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical(); //Max HP and M.Attack Curve
                            GUILayout.Label("Max HP:");
                            Rect curveValueRangeHP = new Rect(0,0, 100, 9999);
                            if(classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].maxHPCurve,new Color32(208, 128, 96, 255), curveValueRangeHP, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if(GUILayout.Button("Edit Max Hp", GUILayout.Width(curveWidth)))
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
                            Rect curveValueRangeMAtk = new Rect(0,0, 100,250);
                            if(classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].mAttackCurve,new Color32(192, 80, 176, 255), curveValueRangeMAtk, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if(GUILayout.Button("Edit M.Attack", GUILayout.Width(curveWidth)))
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
                            Rect curveValueRangeMP = new Rect(0,0, 100, 2000);
                            if(classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].maxMPCurve,new Color32(80, 128, 224, 255), curveValueRangeMP, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if(GUILayout.Button("Edit Max MP", GUILayout.Width(curveWidth)))
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
                            Rect curveValueRangeMDef = new Rect(0,0, 100,250);
                            if(classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].mDefenseCurve,new Color32(64, 160, 64, 255), curveValueRangeMAtk, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if(GUILayout.Button("Edit M.Defense", GUILayout.Width(curveWidth)))
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
                            Rect curveValueRangeAttack = new Rect(0,0, 100, 250);
                            if(classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].AttackCurve,new Color32(192, 64, 96, 255), curveValueRangeAttack, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if(GUILayout.Button("Edit Attack", GUILayout.Width(curveWidth)))
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
                            Rect curveValueRangeAgi= new Rect(0,0, 100,500);
                            if(classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].agilityCurve,new Color32(80, 176, 224, 255), curveValueRangeAgi, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if(GUILayout.Button("Edit Agility", GUILayout.Width(curveWidth)))
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
                            Rect curveValueRangeDefense = new Rect(0,0, 100, 250);
                            if(classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].DefenseCurve,new Color32(144, 176, 96, 255), curveValueRangeDefense, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if(GUILayout.Button("Edit Defense", GUILayout.Width(curveWidth)))
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
                            Rect curveValueRangeLuck= new Rect(0,0, 100,500);
                            if(classSize > 0)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    EditorGUILayout.CurveField(classes[index].luckCurve,new Color32(208, 176, 64, 255), curveValueRangeLuck, GUILayout.Width(curveWidth), GUILayout.Height(curveheight));
                                }
                                if(GUILayout.Button("Edit Luck", GUILayout.Width(curveWidth)))
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

            GUILayout.EndArea();

        GUILayout.EndArea();
        #endregion
    }



    #region Features
    /// <summary>
    /// Create Texture for GUI skin.
    /// </summary>
    /// <param name="width">pixel width of GUI Skin.</param>
    /// <param name="height">pixel height of GUI Skin.</param>
    /// <param name="col">Color of GUI Skin.</param>
    /// <returns></returns>
    private Texture2D CreateTexture(int width, int height, Color col)
    {
        //Create array of color.
        Color[] colPixel = new Color[width * height];

        for (int i = 0; i < colPixel.Length; ++i)
        {
            colPixel[i] = col;
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(colPixel);
        result.Apply();
        return result;
    }


    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from classSize</param>
    int counter = 0;
    private void ChangeMaximum(int size)
    {
        classSize = classSizeTemp;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        while (counter <= classSize)
        {
            classes.Add(ScriptableObject.CreateInstance<ClassesData>());
            AssetDatabase.CreateAsset(classes[counter], "Assets/Resources/Data/ClassesData/Class_" + counter + ".asset");
            AssetDatabase.SaveAssets();
            classesNames.Add(classes[counter].className);
            counter++;
        }
        if (counter > classSize)
        {
            classes.RemoveRange(classSize, classes.Count - classSize);
            classesNames.RemoveRange(classSize, classesNames.Count - classSize);
            for (int i = classSize; i <= counter; i++)
            {
                AssetDatabase.DeleteAsset("Assets/Resources/Data/ClassesData/Class_" + i + ".asset");
            }
            AssetDatabase.SaveAssets();
            counter = classSize;
        }
    }
    #endregion

    private void Testing(int windowId)
    {

    }
}