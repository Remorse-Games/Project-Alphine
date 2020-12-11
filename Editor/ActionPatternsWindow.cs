using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Runtime.CompilerServices;
using System.IO;
using SFB;
using System.Linq;

public class ActionPatternsWindow : EditorWindow
{
    public string[] skillTabDisplayName;
    public string[] stateDisplayName;
    public string[] conditionNames =
    {
        "Always",
        "Turn",
        "HP",
        "MP",
        "State",
        "Party Level",
        "Switch",
    };
    public string[] switchNames =
    {
        "0001",
        "0002",
    };

    //BaseValue
    int i = 0;
    string firstActionName;
    int firstSelectedSkillIndex;
    int firstRatingValue;

    int firstSelectedConditionIndex;

    int firstAdditionalValue1;
    int firstAdditionalValue2;
    int firstAdditionalSelectedIndex;

    //Arrays
    bool[] tabToggle = new bool[7] { true, false, false, false, false, false, false };

    GUIStyle windowStyle;
    GUIStyle columnStyle;
    GUIStyle tabStyle;

    //Static Value
    static List<ActionPatternsData> thisClass;
    static int actionIndex;
    static int actionSize;

    public static void ShowWindow(List<ActionPatternsData> actionData, int index, int size)
    {
        var window = GetWindow<ActionPatternsWindow>();
        var position = window.position;
        //Sizing
        window.maxSize = new Vector2(400, 290);
        window.minSize = new Vector2(400, 290);
        window.titleContent = new GUIContent("Action");

        thisClass = actionData;
        actionIndex = index;
        actionSize = size;

        position.center = new Rect(Screen.width * -1 * .05f, Screen.height * -1 * .05f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        window.position = position;
        window.Show();
    }
    private void OnGUI()
    {
        BaseValue(i++);
        windowStyle = new GUIStyle(GUI.skin.box);
        windowStyle.normal.background = CreateTexture(1, 1, Color.gray);
        columnStyle = new GUIStyle(GUI.skin.box);
        columnStyle.normal.background = CreateTexture(1, 1, new Color32(70, 70, 70, 200));
        tabStyle = new GUIStyle(GUI.skin.box);
        if (EditorGUIUtility.isProSkin)
            tabStyle.normal.background = CreateTexture(1, 1, new Color32(150, 150, 150, 100));
        else
            tabStyle.normal.background = CreateTexture(1, 1, new Color32(200, 200, 200, 100));

        LoadSkillTabList();
        LoadStateList(); 

        #region PrimaryTab
        Rect primaryBox = new Rect(0, 0, 400, 290);
        GUILayout.BeginArea(primaryBox, windowStyle);

            #region MainTab
            Rect generalBox = new Rect(5, 7, 390, 287);
            GUILayout.BeginArea(generalBox, columnStyle);
                GUILayout.BeginVertical();
                    GUILayout.BeginVertical();
                        float widthSpace = generalBox.width * .37f;

                        #region Skill And Rating
                        Rect skillRating = new Rect(5, 5, generalBox.width - 10, position.height * .22f);
                        GUILayout.BeginArea(skillRating, tabStyle);
                            GUILayout.BeginVertical();
                                GUILayout.Label("Skill And Rating", EditorStyles.boldLabel);
                                GUILayout.BeginHorizontal();

                                    GUILayout.BeginVertical();
                                        GUILayout.Label("Skill:");
                                        thisClass[actionIndex].selectedSkillIndex = EditorGUILayout.Popup(thisClass[actionIndex].selectedSkillIndex, 
                                                                                                                skillTabDisplayName, 
                                                                                                                GUILayout.Width(skillRating.width * .45f), 
                                                                                                                GUILayout.Height(20));
                                    GUILayout.EndVertical();

                                    GUILayout.BeginVertical();
                                        GUILayout.Label("Rating:");
                                        thisClass[actionIndex].ratingValue = EditorGUILayout.IntField(thisClass[actionIndex].ratingValue, 
                                                                                                            GUILayout.Width(skillRating.width * .45f), 
                                                                                                            GUILayout.Height(20));
                                    GUILayout.EndVertical();

                                GUILayout.EndHorizontal();
                            GUILayout.EndVertical();
                        GUILayout.EndArea();
                        #endregion

                        #region Conditions
                        Rect conditionBox = new Rect(5, skillRating.height + 10, generalBox.width - 10, generalBox.height * .7f);
                            GUILayout.BeginArea(conditionBox, tabStyle);
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Conditions", EditorStyles.boldLabel);
                                    ConditionBox(conditionBox, widthSpace);
                                    thisClass[actionIndex].actionName = StringMaker(thisClass[actionIndex].selectedConditionIndex);
                                    
                                    GUILayout.Space(5);                            

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(generalBox.width * .155f);
                                    // OK Button
                                    if (GUILayout.Button("OK", GUILayout.Width(generalBox.width * .23f), GUILayout.Height(20)))
                                    {
                                        this.Close();
                                    }
                                    // Cancel Button
                                    if (GUILayout.Button("Cancel", GUILayout.Width(generalBox.width * .23f), GUILayout.Height(20)))
                                    {
                                        if(firstActionName != null && firstActionName != "")
                                        {
                                            thisClass[actionIndex].actionName = firstActionName;
                                            thisClass[actionIndex].selectedConditionIndex = firstSelectedConditionIndex;
                                            thisClass[actionIndex].selectedSkillIndex = firstSelectedSkillIndex;
                                            thisClass[actionIndex].additionalSelectedIndex = firstAdditionalSelectedIndex;
                                            thisClass[actionIndex].additionalValue1 = firstAdditionalValue1;
                                            thisClass[actionIndex].additionalValue2 = firstAdditionalValue2;
                                            this.Close();
                                        }
                                        else
                                        {
                                            this.Close();
                                            for (int i = actionIndex; i < EnemyTab.actionSize[EnemyTab.index] - 1; i++)
                                            {
                                                thisClass[i].actionName = thisClass[i + 1].actionName;
                                                thisClass[i].selectedConditionIndex = thisClass[i + 1].selectedConditionIndex;
                                                thisClass[i].selectedSkillIndex = thisClass[i + 1].selectedSkillIndex;
                                                thisClass[i].additionalSelectedIndex = thisClass[i + 1].additionalSelectedIndex;
                                                thisClass[i].additionalValue1 = thisClass[i + 1].additionalValue1;
                                                thisClass[i].additionalValue2 = thisClass[i + 1].additionalValue2;
                                            }
                                            actionIndex = 0;
                                            clear();
                                        }
                                    }
                                    if(firstActionName != null)
                                    { 
                                        if (GUILayout.Button("Clear", GUILayout.Width(generalBox.width * .23f), GUILayout.Height(20)))
                                        {
                                            this.Close();
                                            for (int i = actionIndex; i < EnemyTab.actionSize[EnemyTab.index] - 1; i++)
                                            {
                                                thisClass[i].actionName = thisClass[i + 1].actionName;
                                                thisClass[i].selectedConditionIndex = thisClass[i + 1].selectedConditionIndex;
                                                thisClass[i].selectedSkillIndex = thisClass[i + 1].selectedSkillIndex;
                                                thisClass[i].additionalSelectedIndex = thisClass[i + 1].additionalSelectedIndex;
                                                thisClass[i].additionalValue1 = thisClass[i + 1].additionalValue1;
                                                thisClass[i].additionalValue2 = thisClass[i + 1].additionalValue2;
                                            }
                                            actionIndex = 0;
                                            clear();
                                        }
                                    }
                                    else
                                    {
                                        if (GUILayout.Button("Unable To Clear", GUILayout.Width(generalBox.width * .23f), GUILayout.Height(20)))
                                        {

                                        }
                                    }
                                    GUILayout.EndHorizontal();
                                GUILayout.EndVertical();
                            GUILayout.EndArea();
                        #endregion
                        
                        
                        
                        GUILayout.Space(5);
                    GUILayout.EndVertical();
                GUILayout.EndVertical();
            GUILayout.EndArea();
            #endregion
        
        GUILayout.EndArea();
        #endregion
    }
    private void OnDestroy()
    {
        EnemyTab.actionIndex = 0;
        EnemyTab.actionIndexTemp = -1;
    }
    private void ConditionBox(Rect conditionBox, float widthSpace)
    {
        float fieldWidth = conditionBox.width * .2f;
        float fieldHeight = conditionBox.height * .12f;

        MemsetArray(thisClass[actionIndex].selectedConditionIndex, tabToggle);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(conditionNames[0], tabToggle[0], EditorStyles.radioButton))
        {
            MemsetArray(0, tabToggle);
            thisClass[actionIndex].selectedConditionIndex = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(conditionNames[1], tabToggle[1], EditorStyles.radioButton))
        {
            MemsetArray(1, tabToggle);
            GUILayout.BeginHorizontal();
                thisClass[actionIndex].additionalValue1 = EditorGUILayout.IntField(thisClass[actionIndex].additionalValue1, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                GUILayout.Label(" + ");
                thisClass[actionIndex].additionalValue2 = EditorGUILayout.IntField(thisClass[actionIndex].additionalValue2, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                GUILayout.Space(widthSpace);
            GUILayout.EndHorizontal();

            thisClass[actionIndex].selectedConditionIndex = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(conditionNames[2], tabToggle[2], EditorStyles.radioButton))
        {
            MemsetArray(2, tabToggle);
            GUILayout.BeginHorizontal();
                thisClass[actionIndex].additionalValue1 = EditorGUILayout.IntField(thisClass[actionIndex].additionalValue1, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                GUILayout.Label(" ~ ");
                thisClass[actionIndex].additionalValue2 = EditorGUILayout.IntField(thisClass[actionIndex].additionalValue2, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                GUILayout.Space(widthSpace);
            GUILayout.EndHorizontal();

            thisClass[actionIndex].selectedConditionIndex = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(conditionNames[3], tabToggle[3], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
            GUILayout.BeginHorizontal();
                thisClass[actionIndex].additionalValue1 = EditorGUILayout.IntField(thisClass[actionIndex].additionalValue1, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                GUILayout.Label(" ~ ");
                thisClass[actionIndex].additionalValue2 = EditorGUILayout.IntField(thisClass[actionIndex].additionalValue2, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                GUILayout.Space(widthSpace);
            GUILayout.EndHorizontal();
            thisClass[actionIndex].selectedConditionIndex = 3;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(conditionNames[4], tabToggle[4], EditorStyles.radioButton))
        {
            MemsetArray(4, tabToggle);
            GUILayout.BeginHorizontal();
                thisClass[actionIndex].additionalSelectedIndex = EditorGUILayout.Popup(thisClass[actionIndex].additionalSelectedIndex, stateDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                GUILayout.Space(widthSpace);
                thisClass[actionIndex].selectedConditionIndex = 4;
            GUILayout.EndHorizontal();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(conditionNames[5], tabToggle[5], EditorStyles.radioButton))
        {
            MemsetArray(5, tabToggle);
            GUILayout.BeginHorizontal();
                thisClass[actionIndex].additionalValue1 = EditorGUILayout.IntField(thisClass[actionIndex].additionalValue1, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                GUILayout.Label(" or above ");
                GUILayout.Space(widthSpace);
                thisClass[actionIndex].selectedConditionIndex = 5;
            GUILayout.EndHorizontal();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(conditionNames[6], tabToggle[6], EditorStyles.radioButton))
        {
            MemsetArray(6, tabToggle);
            GUILayout.BeginHorizontal();
                thisClass[actionIndex].additionalSelectedIndex = EditorGUILayout.Popup(thisClass[actionIndex].additionalSelectedIndex, stateDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                GUILayout.Space(widthSpace);
                thisClass[actionIndex].selectedConditionIndex = 6;
            GUILayout.EndHorizontal();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    #region Features
    private void clear()
    {
        ChangeMaximum<ActionPatternsData>(--actionSize, thisClass, PathDatabase.EnemyActionExplicitDataPath + (EnemyTab.index + 1) + "/Action_");

        if (actionSize <= 0)
        {
            ChangeMaximum<ActionPatternsData>(1, thisClass, PathDatabase.EnemyActionExplicitDataPath + (EnemyTab.index + 1) + "/Action_");
            actionSize = 1;
        }

        EnemyTab.actionSize[EnemyTab.index] = actionSize;
    }

    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from actorSize</param>
    /// <param name="listTabData">list of item that you want to display.</param>
    /// <param name="dataTabName">get size from actorSize</param>
    public void ChangeMaximum<GameData>(int dataSize, List<GameData> listTabData, string dataPath) where GameData : ScriptableObject
    {
        int counter = listTabData.Count;

        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        if (dataSize > listTabData.Count)
            while (dataSize > listTabData.Count)
            {
                listTabData.Add(ScriptableObject.CreateInstance<GameData>());
                AssetDatabase.CreateAsset(listTabData[listTabData.Count - 1], dataPath + (counter + 1) + ".asset");
                AssetDatabase.SaveAssets();
                counter++;
            }
        else
        {
            int tempListTabData = listTabData.Count;
            listTabData.RemoveRange(dataSize, listTabData.Count - dataSize);
            for (int i = tempListTabData; i > dataSize; i--)
            {
                AssetDatabase.DeleteAsset(dataPath + i + ".asset");
            }
            AssetDatabase.SaveAssets();
        }
    }

    public string StringMaker(int selectedToggleIndex)
    {
        string outputString = "";
        string val = string.Format("{0}", 
                                skillTabDisplayName[thisClass[actionIndex].selectedSkillIndex]);

        switch (selectedToggleIndex)
        {
            case 0:
                outputString = PadString(val, conditionNames[selectedToggleIndex]);
                break;
            case 1:
                outputString = PadString(val,
                                        conditionNames[selectedToggleIndex] + ' ' + thisClass[actionIndex].additionalValue1 + " + " + 
                                        thisClass[actionIndex].additionalValue2 + " * X");
                break;
            case 2:
                outputString = PadString(val,
                                        conditionNames[selectedToggleIndex] + ' ' + thisClass[actionIndex].additionalValue1 + "% ~ " +
                                        thisClass[actionIndex].additionalValue2 + " %");
                break;
            case 3:
                outputString = PadString(val,
                                        conditionNames[selectedToggleIndex] + ' ' + thisClass[actionIndex].additionalValue1 + "% ~ " +
                                        thisClass[actionIndex].additionalValue2 + "%");
                break;
            case 4:
                outputString = PadString(val, conditionNames[selectedToggleIndex] + ' ' + stateDisplayName[thisClass[actionIndex].additionalSelectedIndex]);
                break;
            case 5:
                outputString = PadString(val, conditionNames[selectedToggleIndex] + " >= " + thisClass[actionIndex].additionalValue1);
                break;
            default:
                outputString = PadString(val, conditionNames[selectedToggleIndex] + ' ' + stateDisplayName[thisClass[actionIndex].additionalSelectedIndex]);
                break;
        }

        val = string.Format("{0}", ('\t' + thisClass[actionIndex].ratingValue.ToString()));
        outputString = PadString(outputString, val);
        return outputString;
    }

    public string PadString(string key, string value)
    {
        int pad = 4 - (key.Length / 4);

        if (key.Length >= 11)
        {
            pad++;
        }
        if (key.Length >= 18)
        {
            pad++;
        }
        string format = key;

        for (int i = 0; i < pad; i++)
        {
            format += '\t';
        }
        return string.Format(format + "{0}", value);
    }

    /// <summary>
    /// Create Texture for GUI skin.
    /// </summary>
    /// <param name="width">pixel width of GUI Skin.</param>
    /// <param name="height">pixel height of GUI Skin.</param>
    /// <param name="col">Color of GUI Skin.</param>
    /// <returns></returns>
    public Texture2D CreateTexture(int width, int height, Color col)
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

    private void BaseValue(int i)
    {
        if (i == 0)
        {
            firstActionName = thisClass[actionIndex].actionName;
            firstSelectedConditionIndex = thisClass[actionIndex].selectedConditionIndex;
            firstSelectedSkillIndex = thisClass[actionIndex].selectedSkillIndex;
            firstAdditionalSelectedIndex = thisClass[actionIndex].additionalSelectedIndex;
            firstAdditionalValue1 = thisClass[actionIndex].additionalValue1;
            firstAdditionalValue2 = thisClass[actionIndex].additionalValue2;
        }
    }
    private void LoadSkillTabList()
    {
        SkillData[] skillTabData = Resources.LoadAll<SkillData>(PathDatabase.SkillTabRelativeDataPath);
        skillTabDisplayName = new string[skillTabData.Length];
        for (int i = 0; i < skillTabDisplayName.Length; i++)
        {
            skillTabDisplayName[i] = skillTabData[i].skillName;
        }
    }

    /// <summary>
    /// Create Texture for GUI skin.
    /// </summary>
    /// <param name="checkedTrue">bool index that will be checked as true</param>
    /// <returns></returns>
    public void MemsetArray(int checkedTrue, bool[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = false;
        }
        arr[(checkedTrue)] = true;
    }
    private void LoadStateList()
    {
        StateData[] stateData = Resources.LoadAll<StateData>(PathDatabase.StateRelativeDataPath);
        stateDisplayName = new string[stateData.Length];
        for (int i = 0; i < stateDisplayName.Length; i++)
        {
            stateDisplayName[i] = stateData[i].stateName;
        }
    }
    #endregion
}
