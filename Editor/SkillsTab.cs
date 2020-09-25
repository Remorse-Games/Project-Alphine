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
    GUIStyle skillStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    public string[] skillTypeList =
    {
        "None",
        "Magic",
        "Special",
    };


    public string[] skillScopeList =
    {
        "None",
        "1 Enemy",
        "All Enemies",
        "1 Random Enimies",
        "2 Random Enimies",
        "3 Random Enimies",
        "4 Random Enimies",
        "1 Ally",
        "All Allies",
        "1 Ally (Dead)",
        "The Allies (Dead)",
        "The User",
    };

    public string[] skillOccasion =
    {
        "Always",
        "Battle Screen",
        "Menu Screen",
        "Never",
    };

    public string[] skillHitType =
    {
        "Certain Hit",
        "Pyhsical Hit",
        "Magical Hit",
    };

    public string[] skillAnimation =
    {
        "Normal Attack",
        "None",
        "Hit Pyhsical",
        "...",
    };

    public string[] skillWeaponType =
    {
        "None",
        "Dagger",
        "Sword",
        "...",
    };
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
                SkillListSelected(indexTemp);
                indexTemp = -1;
            }

            skillSizeTemp = EditorGUILayout.IntField(skillSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                ChangeMaximum(skillSize);
            }
            GUILayout.EndArea();
            #endregion
            #endregion

            #region Tab 2/3
            //second Column
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 15), columnStyle);

                //GeneralSettings tab
                Rect generalBox = new Rect(5, 5, firstTabWidth + 60, position.height / 3 - 50);

                #region GeneralSettings
                GUILayout.BeginArea(generalBox, tabStyle); //Start of general settings tab
                    GUILayout.Label("General Settings", EditorStyles.boldLabel); //general settings label
                        GUILayout.BeginVertical();
                            GUILayout.BeginHorizontal();
                                GUILayout.BeginVertical(); //Name label, name field, class label, and class popup
                                GUILayout.Label("Name:");
                                if (skillSize > 0)
                                {
                                    skill[index].skillName = GUILayout.TextField(skill[index].skillName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                    skillDisplayName[index] = skill[index].skillName;
                                }
                                else
                                { GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8)); }
                                GUILayout.EndVertical(); //Name label, name field, class label, and class popup (ending)
            
                                /*GUILayout.BeginVertical(); //Nickname label, nickname field, initial level and max level label and field
                                                           //put icon here
                                GUILayout.EndVertical();*/
                            GUILayout.EndHorizontal();
                        GUILayout.EndVertical();

                    GUILayout.EndArea(); //End of general settings tab
                #endregion
                GUILayout.EndArea(); //End of second column
            #endregion
            GUILayout.EndArea(); // End of drawing SkillsTab

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
            skillDisplayName.RemoveRange(skillSize, skill.Count - skillSize);
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
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/ActorData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "ActorData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Image"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Image");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/ClassesData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "ClassesData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/SkillData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "SkillData");
        }

    }

    ExtensionFilter[] extensions = new[] {
        new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
        new ExtensionFilter("Sound Files", "mp3", "wav" ),
        new ExtensionFilter("All Files", "*" ),
    };

    private void changeIconImage()
    {
        string relativepath;
        string[] path = StandaloneFileBrowser.OpenFilePanel("Choose Icon", "Assets/Resources/Image", extensions, false);

        if (path.Length != 0)
        {
            relativepath = "Image/";
            relativepath += Path.GetFileNameWithoutExtension(path[0]);
            Sprite imageChosen = Resources.Load<Sprite>(relativepath);
            skill[index].skillIcon = imageChosen;
            SkillListSelected(index);
        }
    }

    /// <summary>
    /// This called when actor list on active.
    /// </summary>
    /// <param name="index">index of actor in a list.</param>
    public void SkillListSelected(int index)
    {
            
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if (skillSize > 0)
            {
                if (skill[index].skillIcon == null)
                    Icon = defTex;
                else
                    Icon = textureFromSprite(skill[index].skillIcon);
            }
        }
    }

    /// <summary>
    /// Create a texture from a sprite (Used for changing actors' images)
    /// </summary>
    /// <param name="sprite">the sprite that wants to be converted into texture</param>
    /// <returns></returns>
    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
    #endregion
}
