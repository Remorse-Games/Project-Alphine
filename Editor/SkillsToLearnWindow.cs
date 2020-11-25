using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Runtime.CompilerServices;
using System.IO;
using SFB;
using System.Linq;

public class SkillsToLearnWindow : EditorWindow
{
    public string[] skillTabDisplayName;

    //Base Value
    int i = 0;
    string firstSkillName;
    int firstSelectedArray;
    int firstLevel;
    string firstNotes;

    //GUI Styles
    GUIStyle windowStyle;
    GUIStyle columnStyle;
    GUIStyle tabStyle;

    //Data(s) reference
    static List<SkillsToLearn> thisClass;
    static int skillIndex;
    static int skillSize;
    public static void ShowWindow(List<SkillsToLearn> skillToLearnData, int index, int size)
    {
        var window = GetWindow<SkillsToLearnWindow>();
        var position = window.position;
        //Sizing
        window.maxSize = new Vector2(400, 150);
        window.minSize = new Vector2(400, 150);
        window.titleContent = new GUIContent("Skills To Learn");

        thisClass = skillToLearnData;
        skillIndex = index;
        skillSize = size;

        position.center = new Rect(Screen.width * -1 * .05f, Screen.height * -1 * .05f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        window.position = position;
        window.Show();
    }

    private void OnGUI()
    {
        LoadSkillTabList();
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

        
        #region PrimaryTab
        Rect primaryBox = new Rect(0, 0, 400, 153);
        GUILayout.BeginArea(primaryBox, windowStyle);

            #region MainTab
            Rect generalBox = new Rect(5, 7, 390, 155);
            GUILayout.BeginArea(generalBox, columnStyle);
                GUILayout.BeginVertical("Box");
                    GUILayout.Label("Skills To Learn", EditorStyles.boldLabel);
                    GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                            GUILayout.Label("Level:");
                            thisClass[skillIndex].level = EditorGUILayout.IntField(thisClass[skillIndex].level, GUILayout.Width(generalBox.width * .475f), GUILayout.Height(20));
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical();
                            GUILayout.Label("Skill:");
                            thisClass[skillIndex].selectedArrayIndex = EditorGUILayout.Popup(thisClass[skillIndex].selectedArrayIndex, skillTabDisplayName, GUILayout.Width(generalBox.width * .475f), GUILayout.Height(20));
                        GUILayout.EndVertical();            
                    GUILayout.EndHorizontal();

                    GUILayout.Label("Notes:");
                    thisClass[skillIndex].notes = GUILayout.TextArea(thisClass[skillIndex].notes, GUILayout.Width(generalBox.width - 15), GUILayout.Height(20));
                    thisClass[skillIndex].skillToLearnName = StringMaker();

                    GUILayout.Space(5);

                    GUILayout.BeginHorizontal();
                            // OK Button
                            if (GUILayout.Button("OK", GUILayout.Width(generalBox.width * .31f), GUILayout.Height(20)))
                            {
                                this.Close();
                            }
                            // OK Button
                            if (GUILayout.Button("Cancel", GUILayout.Width(generalBox.width * .31f), GUILayout.Height(20)))
                            {
                                if(firstSkillName != null && firstSkillName != "")
                                { 
                                    thisClass[skillIndex].skillToLearnName = firstSkillName;
                                    thisClass[skillIndex].selectedArrayIndex = firstSelectedArray;
                                    thisClass[skillIndex].level = firstLevel;
                                    thisClass[skillIndex].notes = firstNotes;
                                    this.Close();
                                }
                                else
                                {
                                    this.Close();
                                    for (int i = skillIndex; i < skillSize - 1; i++)
                                    {
                                        thisClass[skillIndex].skillToLearnName = thisClass[skillIndex + 1].skillToLearnName;
                                        thisClass[skillIndex].selectedArrayIndex = thisClass[skillIndex + 1].selectedArrayIndex;
                                        thisClass[skillIndex].level = thisClass[skillIndex + 1].level;
                                        thisClass[skillIndex].notes = thisClass[skillIndex + 1].notes;
                                    }
                                    skillIndex = 0;
                                    ChangeMaximum<SkillsToLearn>(--skillSize, thisClass, PathDatabase.SkillToLearnExplicitDataPath + (ClassTab.index + 1) + "/SkillToLearn_");

                                    if(skillSize <= 0)
                                    {
                                        ChangeMaximum<SkillsToLearn>(1, thisClass, PathDatabase.SkillToLearnExplicitDataPath + (ClassTab.index + 1) + "/SkillToLearn_");
                                        skillSize = 1;
                                    }

                                    ClassTab.skillToLearnSize[ClassTab.index] = skillSize;
                                }
                            }
                            if(firstSkillName != null)
                            { 
                                if (GUILayout.Button("Clear", GUILayout.Width(generalBox.width * .31f), GUILayout.Height(20)))
                                {
                                    this.Close();
                                    for (int i = skillIndex; i < skillSize - 1; i++)
                                    {
                                        thisClass[skillIndex].skillToLearnName = thisClass[skillIndex + 1].skillToLearnName;
                                        thisClass[skillIndex].selectedArrayIndex = thisClass[skillIndex + 1].selectedArrayIndex;
                                        thisClass[skillIndex].level = thisClass[skillIndex + 1].level;
                                        thisClass[skillIndex].notes = thisClass[skillIndex + 1].notes;
                                    }
                                    skillIndex = 0;
                                    ChangeMaximum<SkillsToLearn>(--skillSize, thisClass, PathDatabase.SkillToLearnExplicitDataPath + (ClassTab.index + 1) + "/SkillToLearn_");
                
                                    if(skillSize <= 0)
                                    {
                                        ChangeMaximum<SkillsToLearn>(1, thisClass, PathDatabase.SkillToLearnExplicitDataPath + (ClassTab.index + 1) + "/SkillToLearn_");
                                        skillSize = 1;
                                    }

                                    ClassTab.skillToLearnSize[ClassTab.index] = skillSize;
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("Unable To Clear", GUILayout.Width(generalBox.width * .31f), GUILayout.Height(20)))
                                {

                                }
                            }

                    GUILayout.EndHorizontal();

                    GUILayout.Space(5);

                GUILayout.EndVertical();
            GUILayout.EndArea();
            #endregion
        
        GUILayout.EndArea();
        #endregion  

    }

    private void OnLostFocus()
    {
        this.Focus();
    }

    #region Features
    private string StringMaker()
    {
        string outputString = "";
        string val = "";

        val = string.Format("{0}", skillTabDisplayName[thisClass[skillIndex].selectedArrayIndex]);
        outputString = PadString("Lv. " + thisClass[skillIndex].level.ToString(), val);
        val = string.Format("{0}", thisClass[skillIndex].notes);
        outputString = PadString(outputString, val);

        return outputString;
    }
    public string PadString(string key, string value)
    {
        int pad = 4 - (key.Length / 4);

        if (key.Length >= 12)
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
    private void LoadSkillTabList()
    {
        SkillData[] skillTabData = Resources.LoadAll<SkillData>(PathDatabase.SkillTabRelativeDataPath);
        skillTabDisplayName = new string[skillTabData.Length];
        for (int i = 0; i < skillTabDisplayName.Length; i++)
        {
            skillTabDisplayName[i] = skillTabData[i].skillName;
        }
    }
    public void BaseValue(int i)
    {
        if (i == 0)
        {
            firstSkillName = thisClass[skillIndex].skillToLearnName;
            firstSelectedArray = thisClass[skillIndex].selectedArrayIndex;
            firstLevel = thisClass[skillIndex].level;
            firstNotes = thisClass[skillIndex].notes;
        }
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
        int counter = 0;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        if (dataSize > listTabData.Count)
            while (dataSize > listTabData.Count)
            {
                listTabData.Add(ScriptableObject.CreateInstance<GameData>());
                counter = listTabData.Count;
                AssetDatabase.CreateAsset(listTabData[listTabData.Count - 1], dataPath + counter + ".asset");
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


    #endregion
}
