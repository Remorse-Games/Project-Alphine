using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class SkillsTab : BaseTab
{
    //Having list of all skills exist in data.
    public List<SkillData> skill = new List<SkillData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in SkillData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> skillDisplayName = new List<string>();

    //Classes
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
        "Other... (Add More Manually)",
    };

    public string[] skillWeaponType =
    {
        "None",
        "Dagger",
        "Sword",
        "Other... (Add More Manually)",
    };

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle skillStyle;

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
    Texture2D skillIcon;

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
                ItemTabLoader(indexTemp);
                indexTemp = -1;
            }

            // Change Maximum field and button
            skillSizeTemp = EditorGUILayout.IntField(skillSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                ChangeMaximumPrivate(skillSize);
            }

            GUILayout.EndArea();
            #endregion // End Of First Tab

            #region Tab 2/3
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 25), columnStyle);

            Rect generalBox = new Rect(5, 5, firstTabWidth + 60, position.height / 2 - 68);
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
                                if (skillSize > 0)
                                {
                                    skill[index].skillName = GUILayout.TextField(skill[index].skillName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                    skillDisplayName[index] = skill[index].skillName;
                                }
                                else
                                {
                                    GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                }
                            GUILayout.EndVertical();
                            #endregion
                            #region Icon
                            GUILayout.BeginArea(new Rect(190, 21, firstTabWidth - 220, position.height / 2)); // Icon Area
                            GUILayout.BeginHorizontal();
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Icon:"); // Icon label
                                GUILayout.EndVertical();

                                GUILayout.BeginVertical();
                                    GUILayout.Box(skillIcon, GUILayout.Width(61), GUILayout.Height(61)); // Icon Box preview
                                    if (GUILayout.Button("Edit Icon", GUILayout.Height(20), GUILayout.Width(64))) // Icon changer Button
                                    {
                                        skill[index].Icon = ImageChanger(
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
                            if (skillSize > 0)
                            {
                                skill[index].skillDescription = GUILayout.TextArea(skill[index].skillDescription, GUILayout.Width(firstTabWidth + 53), GUILayout.Height(generalBox.height / 5 - 13));
                            }
                            else
                            {
                                GUILayout.TextArea("Null", GUILayout.Width(firstTabWidth + 53), GUILayout.Height(generalBox.height / 5 + 5));
                            }
                            #endregion
                        GUILayout.Space(5);
                        #region Horizontal
                        GUILayout.BeginHorizontal();   
                            #region SkillType Scope
                            GUILayout.BeginVertical();
                                GUILayout.Label("Skill Type:"); // Skill Type class label
                                if (skillSize > 0)
                                {
                                    skill[index].selectedSkillTypeIndex = EditorGUILayout.Popup(skill[index].selectedSkillTypeIndex, skillTypeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                }
                                else
                                {
                                    EditorGUILayout.Popup(0, skillTypeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                }
                            GUILayout.EndVertical();
                            #endregion
                            #region MP_Cost TP_Cost Occasion
                            GUILayout.BeginVertical();
                                GUILayout.Label("MP Cost:"); // MP Cost class label
                                if (skillSize > 0)
                                { skill[index].skillMPCost = EditorGUILayout.IntField(skill[index].skillMPCost, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9)); }
                                else
                                { EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9)); }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("TP Cost:"); // TP Cost class label
                                if (skillSize > 0)
                                { skill[index].skillTPCost = EditorGUILayout.IntField(skill[index].skillTPCost, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9)); }
                                else
                                { EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9)); }
                            GUILayout.EndVertical();

                           
                            #endregion
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                            #region Scope Occasion
                            GUILayout.BeginVertical();
                                GUILayout.Label("Scope:"); // Scope class label
                                if (skillSize > 0)
                                {
                                    skill[index].selectedSkillScopeIndex = EditorGUILayout.Popup(skill[index].selectedSkillScopeIndex, skillScopeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                }
                                else
                                {
                                    EditorGUILayout.Popup(0, skillScopeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
                                }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("Occasion:"); // Occasion class label
                                if (skillSize > 0)
                                {
                                    skill[index].selectedSkillOccasionIndex = EditorGUILayout.Popup(skill[index].selectedSkillOccasionIndex, skillOccasion, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2));
                                }
                                else
                                {
                                    EditorGUILayout.Popup(0, skillOccasion, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2));
                                }
                            GUILayout.EndVertical();
                            #endregion
                        GUILayout.EndHorizontal();
                        #endregion

                    GUILayout.EndVertical();
                    #endregion
                GUILayout.EndArea();
        #endregion
  
            Rect invocationBox = new Rect(5, 310, firstTabWidth + 60, position.height / 4 - 70);
                #region InvocationSettings
                GUILayout.BeginArea(invocationBox, tabStyle);
                #region Vertical
                GUILayout.BeginVertical();

                    GUILayout.Label("Invocation", EditorStyles.boldLabel);

                        GUILayout.BeginHorizontal();

                        #region InitialLevel Success Repeat TPGain
                        GUILayout.BeginVertical();
                            GUILayout.Label("Initial Level:");
                                if (skillSize > 0)
                                { skill[index].skillSpeed = EditorGUILayout.IntField(skill[index].skillSpeed, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
                                else
                                { EditorGUILayout.IntField(-1, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("Success:");
                                if (skillSize > 0)
                                { skill[index].skillSuccessLevel = EditorGUILayout.IntField(skill[index].skillSuccessLevel  , GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
                                else
                                { EditorGUILayout.IntField(-1, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("Repeat:");
                                if (skillSize > 0)
                                { skill[index].skillRepeat = EditorGUILayout.IntField(skill[index].skillRepeat, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
                                else
                                { EditorGUILayout.IntField(-1, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("TP Gain:");
                                if (skillSize > 0)
                                { skill[index].skillTPGain = EditorGUILayout.IntField(skill[index].skillTPGain, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
                                else
                                { EditorGUILayout.IntField(-1, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
                            GUILayout.EndVertical();

                        GUILayout.EndHorizontal();
                        #endregion

                        #region HitType Animation
                        GUILayout.BeginHorizontal();

                            GUILayout.BeginVertical();
                                GUILayout.Label("Hit Type:"); // Skill Hit Type class label
                                if (skillSize > 0)
                                {
                                    skill[index].selectedSkillHitTypeIndex = EditorGUILayout.Popup(skill[index].selectedSkillHitTypeIndex, skillHitType, GUILayout.Height(invocationBox.height / 8 + 3), GUILayout.Width(invocationBox.width / 2 - 5));
                                }
                                else
                                {
                                    EditorGUILayout.Popup(0, skillHitType, GUILayout.Height(invocationBox.height / 8 + 3), GUILayout.Width(invocationBox.width / 2 - 5));
                                }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                            GUILayout.Label("Animation:"); // Skill Type class label
                                if (skillSize > 0)
                                {
                                    skill[index].selectedSkillAnimationIndex = EditorGUILayout.Popup(skill[index].selectedSkillAnimationIndex, skillAnimation, GUILayout.Height(invocationBox.height / 8 + 3), GUILayout.Width(invocationBox.width / 2 - 5));
                                }
                                else
                                {
                                    EditorGUILayout.Popup(0, skillAnimation, GUILayout.Height(invocationBox.height / 8 + 3), GUILayout.Width(invocationBox.width / 2 - 5));
                                }
                            GUILayout.EndVertical();
        

                        GUILayout.EndHorizontal();
                        #endregion
                GUILayout.EndVertical();
                #endregion
                
                GUILayout.EndArea();
        #endregion // End Of Invocation Settings

            Rect messageBox = new Rect(5, 430, firstTabWidth + 60, position.height / 4 - 20);
                #region messageBox
                GUILayout.BeginArea(messageBox, tabStyle);
                    GUILayout.Label("Message", EditorStyles.boldLabel);
                    #region Vertical
                    GUILayout.BeginVertical();
                        #region Fields
                        GUILayout.BeginHorizontal();
                            GUILayout.Label("(User Name)");
                            if (skillSize > 0)
                            {
                                skill[index].skillUserNameMessage = GUILayout.TextField(skill[index].skillUserNameMessage, GUILayout.Width(messageBox.width - 90), GUILayout.Height(messageBox.height / 4 - 19));
                            }
                            else
                            {
                                GUILayout.TextField("Null", GUILayout.Width(messageBox.width - 90), GUILayout.Height(messageBox.height / 4 - 19));
                            }
                        GUILayout.EndHorizontal();

                            GUILayout.Space(3);
                            if (skillSize > 0)
                            {
                                skill[index].skillMessage = GUILayout.TextField(skill[index].skillMessage, GUILayout.Width(messageBox.width - 9), GUILayout.Height(messageBox.height / 4 + 25));
                            }
                            else
                            {
                                GUILayout.TextField("Null", GUILayout.Width(messageBox.width - 9), GUILayout.Height(messageBox.height / 4 + 25));
                            }

                            GUILayout.Space(9);
                        #region Buttons
                        GUILayout.BeginHorizontal();
                            GUILayout.Space(65);
        
                            if (GUILayout.Button("\"casts *!\"", GUILayout.Height(30), GUILayout.Width(63)))
                            {
                                skill[index].skillUserNameMessage = "\"casts *!\"";
                            }
                            GUILayout.Space(25);
                            if (GUILayout.Button("\"does *!\"", GUILayout.Height(30), GUILayout.Width(63)))
                            {
                                skill[index].skillUserNameMessage = "\"does *!\"";
                            }
                            GUILayout.Space(25);
                            if (GUILayout.Button("\"uses *!\"", GUILayout.Height(30), GUILayout.Width(63)))
                            {
                                skill[index].skillUserNameMessage = "\"uses *!\"";
                            }
                        GUILayout.EndHorizontal();
                        #endregion
                    #endregion
                    GUILayout.EndVertical();
                GUILayout.EndArea();
        #endregion
                #endregion

            Rect requiredWeapon = new Rect(5, 600, firstTabWidth + 60, position.height / 10 - 25);
                #region RequiredWeapon
                    GUILayout.BeginArea(requiredWeapon, tabStyle);
                        #region Horizontal
                        GUILayout.BeginHorizontal();
                            #region WeaponTypes
                            GUILayout.BeginVertical();
                                GUILayout.Label("Weapon Type 1:");
                                if (skillSize > 0)
                                {
                                    skill[index].selectedSkillWeaponOneIndex = EditorGUILayout.Popup(skill[index].selectedSkillWeaponOneIndex, skillWeaponType, GUILayout.Height(requiredWeapon.height / 3 + 3), GUILayout.Width(requiredWeapon.width / 2 - 5));
                                }
                                else
                                {
                                    EditorGUILayout.Popup(0, skillWeaponType, GUILayout.Height(requiredWeapon.height / 3 + 3), GUILayout.Width(requiredWeapon.width / 2 - 5));
                                }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("Weapon Type 2:");
                                if (skillSize > 0)
                                {
                                    skill[index].selectedSkillWeaponTwoIndex = EditorGUILayout.Popup(skill[index].selectedSkillWeaponTwoIndex, skillWeaponType, GUILayout.Height(requiredWeapon.height / 3 + 3), GUILayout.Width(requiredWeapon.width / 2 - 5));
                                }
                                else
                                {
                                    EditorGUILayout.Popup(0, skillWeaponType, GUILayout.Height(requiredWeapon.height / 3 + 3), GUILayout.Width(requiredWeapon.width / 2 -5));
                                }
                            GUILayout.EndVertical();
                            #endregion
                        GUILayout.EndHorizontal();
                        #endregion

                    GUILayout.EndArea();
                    #endregion

            GUILayout.EndArea();
            #endregion // End of Second Tab

        GUILayout.EndArea();
        #endregion // End of SkillTab
    }

    #region Features
    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from skillSize</param>

    int counter = 0;
    private void ChangeMaximumPrivate(int size)
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

    public override void ItemTabLoader(int index)
    {
        Debug.Log(index + "index");
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if (skillSize > 0)
            {
                if (skill[index].Icon == null)
                    skillIcon= defTex;
                else
                    skillIcon = TextureToSprite(skill[index].Icon);

            }
        }
    }

    #endregion
}
