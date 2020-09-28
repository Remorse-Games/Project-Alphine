using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;

public class SkillsTab
{
    //Having list of all skills exist in data.
    public List<SkillData> skill = new List<SkillData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in SkillData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> skillDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle skillStyle;
  
    //Index for selected Class.
    public int selectedClassIndex;

    //How many skill in ChangeMaximum Func
    public int skillSize;

    //i don't know about this but i leave this to handle later.
    int index = 0;
    int indexTemp = -1;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;
    Vector2 equipmentScrollPos = Vector2.zero;
    Vector2 traitsScrollPos = Vector2.zero;

    //Image Area.
    Texture2D Icon;

    public int skillSizeTemp;

    public void Init(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in SkillsTab itself.
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
        skillStyle = new GUIStyle(GUI.skin.box);
        skillStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

        #region Entry Of SkillsTab GUILayout
        //Start drawing the whole SkillsTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the SkillsTab? yes, this one.
            GUILayout.Box(" ", skillStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/3
            //First Tab of three
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
            GUILayout.Box("Skills", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

            //Scroll View
            #region ScrollView
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
            index = GUILayout.SelectionGrid(index, skillDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * skillSize));
            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed && index != indexTemp)
            {
                indexTemp = index;
                //SkillListSelected(indexTemp);
                indexTemp = -1;
            }

            skillSizeTemp = EditorGUILayout.IntField(skillSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                ChangeMaximum(skillSize);
            }
            GUILayout.EndArea();
            #endregion

        GUILayout.EndArea(); // End of drawing SkillsTab

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
    /// <param name="size">get size from skillSize</param>

    int counter = 0;
    private void ChangeMaximum(int size)
    {
        skillSize = skillSizeTemp;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        while (counter <= skillSize)
        {
            skill.Add(ScriptableObject.CreateInstance<SkillData>());
            AssetDatabase.CreateAsset(skill[counter], "Assets/Resources/Data/SkillData/Skill_" + counter + ".asset");
            AssetDatabase.SaveAssets();
            skillDisplayName.Add(skill[counter].skillName);
            counter++;
        }
        if (counter > skillSize)
        {
            skill.RemoveRange(skillSize, skill.Count - skillSize);
            skillDisplayName.RemoveRange(skillSize, skillDisplayName.Count - skillSize);
            for (int i = skillSize; i <= counter; i++)
            {
                AssetDatabase.DeleteAsset("Assets/Resources/Data/SkillData/Skill_" + i + ".asset");
            }
            AssetDatabase.SaveAssets();
            counter = skillSize;
        }
    }

    ///<summary>
    /// Folder checker, create folder if it doesnt exist already, Might refactor into one liner if
    ///</summary>
    public void FolderChecker()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Data");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Image"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Image");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/SkillData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "SkillData");
        }

    }

    #endregion
        //The black box behind the SkillsTab? yes, this one.
        GUILayout.Box(" ", skillStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));


        GUILayout.EndArea(); // End of drawing SkillsTab
        #endregion
    }
}
