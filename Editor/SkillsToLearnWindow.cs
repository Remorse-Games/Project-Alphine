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

    //GUI Styles
    GUIStyle windowStyle;
    GUIStyle columnStyle;
    GUIStyle tabStyle;

    //Data(s) reference
    static SkillsToLearn thisClass;
    public static void ShowWindow(SkillsToLearn skillToLearnData)
    {
        var window = GetWindow<SkillsToLearnWindow>();
        var position = window.position;
        //Sizing
        window.maxSize = new Vector2(190, 190);
        window.minSize = new Vector2(190, 190);
        window.titleContent = new GUIContent("Skills To Learn");
        thisClass = skillToLearnData;
        position.center = new Rect(Screen.width * -1 * .05f, Screen.height * -1 * .05f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        window.position = position;
        window.Show();
    }

    private void OnGUI()
    {
        LoadSkillTabList();
        thisClass.skillToLearnName = StringMaker();

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
        Rect primaryBox = new Rect(0, 0, 190, 190);
        GUILayout.BeginArea(primaryBox, windowStyle);

            #region MainTab
            Rect generalBox = new Rect(5, 7, 180, 187);
            GUILayout.BeginArea(generalBox, columnStyle);
                GUILayout.BeginVertical("Box");
                    GUILayout.Label("Skills To Learn", EditorStyles.boldLabel);
                    GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                            GUILayout.Label("Level:");
                                            thisClass.level = EditorGUILayout.IntField(thisClass.level, GUILayout.Width(generalBox.width * .45f), GUILayout.Height(20));
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical();
                            GUILayout.Label("Skill:");
                            thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, skillTabDisplayName, GUILayout.Width(generalBox.width * .45f), GUILayout.Height(20));
                        GUILayout.EndVertical();            
                    GUILayout.EndHorizontal();

                    GUILayout.Label("Notes:");
                    thisClass.notes = GUILayout.TextArea(thisClass.notes, GUILayout.Width(generalBox.width - 15), GUILayout.Height(20));
                    thisClass.skillToLearnName = StringMaker();
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

        val = string.Format("{0}", skillTabDisplayName[thisClass.selectedArrayIndex]);
        outputString = PadString("Lv. " + thisClass.level.ToString(), val);
        val = string.Format("{0}", thisClass.notes);
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
