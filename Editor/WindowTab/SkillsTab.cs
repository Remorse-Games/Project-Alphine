using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.Linq;

public class SkillsTab : BaseTab
{
    //Having list of all skills exist in data.
    public List<SkillData> skill = new List<SkillData>();
    public List<EffectData> effects = new List<EffectData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in SkillData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> skillDisplayName = new List<string>();
    List<string> effectDisplayName = new List<string>();

    //i don't know about this but i leave this to handle later.
    public static int effectIndex = 0;
    public static int effectIndexTemp = -1;


    public string[] skillWeaponType;

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

    public string[] skillType =
    {
        "None",
        "HP Damage",
        "MP Damage",
        "HP Recover",
        "MP Recover",
        "HP Drain",
        "MP Drain",
    };

    public string[] skillElement;

    public string[] skillBool =
    {
        "Yes",
        "No",
    };

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle skillStyle;

    //How many skill in ChangeMaximum Func
    public int skillSize;
    public static int[] effectSize;

    //i don't know about this but i leave this to handle later.
    public static int index = 0;
    int indexTemp = -1;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;
    Vector2 equipmentScrollPos = Vector2.zero;
    Vector2 effectsScrollPos = Vector2.zero;

    //Image Area.
    Texture2D skillIcon;

    public int skillSizeTemp;
    public void Init()
    {
        //Clear List
        skill.Clear();
        effects.Clear();

        //resetting each index to 0
        index = 0;
        effectIndex = 0;

        LoadGameData<SkillData>(ref skillSize, skill, PathDatabase.SkillTabRelativeDataPath);

        effectSize = new int[skillSize];
        LoadGameData<EffectData>(ref effectSize[index], effects, PathDatabase.SkillEffectRelativeDataPath + (index + 1));
        LoadWeaponTypeList();
        LoadElementTypeList();

        //create folder for effectdata
        FolderCreator(skillSize, "Assets/Resources/Data/SkillData", "EffectData");

        if(effectSize[index] <= 0)
        {
            effectIndex = 0;
            ChangeMaximum<EffectData>(++effectSize[index], effects, PathDatabase.SkillEffectExplicitDataPath + (index + 1) + "/Effect_");
        }
        ClearNullScriptableObjects();
        ListReset();
    }

    public void OnRender(Rect position)
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
        
            GUI.changed = false;
            index = GUILayout.SelectionGrid(index, skillDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * skillSize));

            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed && index != indexTemp)
            {
                indexTemp = index;
                effectIndex = 0; 
                effectIndexTemp = -1;

                ItemTabLoader(indexTemp);

                effects.Clear();
                LoadGameData<EffectData>(ref effectSize[index], effects, PathDatabase.SkillEffectRelativeDataPath + (index + 1));

                if(effectSize[index] <= 0)
                {
                    ChangeMaximum<EffectData>(++effectSize[index], effects, PathDatabase.SkillEffectExplicitDataPath + (index + 1) + "/Effect_");
                    effectIndexTemp = 0;
                }
                ClearNullScriptableObjects();

                ListReset();
                indexTemp = -1;
            }

            // Change Maximum field and button
            skillSizeTemp = EditorGUILayout.IntField(skillSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                skillSize = skillSizeTemp;
                index = indexTemp = 0;
                FolderCreator(skillSize, "Assets/Resources/Data/SkillData", "EffectData");
                ChangeMaximum<SkillData>(skillSize, skill, PathDatabase.SkillTabExplicitDataPath);

                //new effectsize array length
                int[] tempArr = new int[effectSize.Length];
                for (int i = 0; i < effectSize.Length; i++)
                    tempArr[i] = effectSize[i];

                effectSize = new int[skillSize];

                //find the smallest between tempArr and skillSize
                int smallestValue = tempArr.Length < skillSize ? tempArr.Length : skillSize;

                for (int i = 0; i <smallestValue; i++)
                    effectSize[i] = tempArr[i];

                //Reload data
                LoadGameData<EffectData>(ref effectSize[index], effects, PathDatabase.SkillEffectRelativeDataPath + (index + 1));
                if(effectSize[index] <= 0)
                {
                    ChangeMaximum<EffectData>(++effectSize[index], effects, PathDatabase.SkillEffectExplicitDataPath + (index + 1) + "/Effect_");
                }

                ClearNullScriptableObjects();
                ListReset();
            }
            else if(skillSizeTemp <= 0){
                skillSizeTemp = skillSize;
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
                            GUILayout.BeginArea(new Rect(generalBox.width / 2 - 3, generalBox.height * .05f + 5, firstTabWidth - 220, position.height / 2)); // Icon Area
                            GUILayout.BeginHorizontal();
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Icon:"); // Icon label
                                GUILayout.EndVertical();

                                GUILayout.BeginVertical();
                                    GUILayout.Box(skillIcon, GUILayout.Width(61), GUILayout.Height(61)); // Icon Box preview
                                    if (GUILayout.Button("Edit Icon", GUILayout.Height(20), GUILayout.Width(61))) // Icon changer Button
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
                                skill[index].skillDescription = GUILayout.TextArea(skill[index].skillDescription, GUILayout.Width(firstTabWidth + 53), GUILayout.Height(generalBox.height / 5 + 5));
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
  
            Rect invocationBox = new Rect(5, generalBox.height + 10, firstTabWidth + 60, position.height / 4 - 70);
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
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Label("Animation:"); // item Animation label
                                        GUILayout.Label("**UnderWorking**", EditorStyles.boldLabel);
                                    GUILayout.EndHorizontal();
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

            Rect messageBox = new Rect(5, generalBox.height + invocationBox.height + 15, firstTabWidth + 60, position.height / 4 + 10);
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
                                skill[index].skillMessage = GUILayout.TextField(skill[index].skillMessage, GUILayout.Width(messageBox.width - 9), GUILayout.Height(messageBox.height / 4 + 40));
                            }
                            else
                            {
                                GUILayout.TextField("Null", GUILayout.Width(messageBox.width - 9), GUILayout.Height(messageBox.height / 4 + 40));
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

            Rect requiredWeapon = new Rect(5, generalBox.height + invocationBox.height + messageBox.height + 20, firstTabWidth + 60, position.height / 4 - 120);
                #region RequiredWeapon
                    GUILayout.BeginArea(requiredWeapon, tabStyle);
                    GUILayout.Label("Required Weapon", EditorStyles.boldLabel);
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

            #region Tab 3/3
            GUILayout.BeginArea(new Rect(firstTabWidth * 2 + 77, 0, firstTabWidth + 25, tabHeight - 25), columnStyle);

                Rect damageBox = new Rect(5, 5, firstTabWidth + 15, position.height / 3 - 80);
                    #region DamageBox
                    GUILayout.BeginArea(damageBox, tabStyle);
                        GUILayout.Label("Damage", EditorStyles.boldLabel);
                        GUILayout.BeginVertical();
                            #region Type Element Formula
                            GUILayout.BeginHorizontal();

                            #region Type Element
                            #region Type
                            GUILayout.BeginVertical();
                                                        GUILayout.Label("Type:");
                                                        if (skillSize > 0)
                                                        {
                                                            skill[index].selectedTypeIndex = EditorGUILayout.Popup(skill[index].selectedTypeIndex, skillType, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(damageBox.width / 2 - 5));
                                                        }
                                                        else
                                                        {
                                                            EditorGUILayout.Popup(0, skillType, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(damageBox.width / 2 - 5));
                                                        }
                                                    GUILayout.EndVertical();
                                                    #endregion
                                                    #region Element
                                                    GUILayout.BeginVertical();
                                                        GUILayout.Label("Element:");
                                                        if (skillSize > 0)
                                                        {
                                                            skill[index].selectedElementIndex = EditorGUILayout.Popup(skill[index].selectedElementIndex, skillElement, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(damageBox.width / 2 - 5));
                                                        }
                                                        else
                                                        {
                                                            EditorGUILayout.Popup(0, skillElement, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(damageBox.width / 2 - 5));
                                                        }
                                                GUILayout.EndVertical();
                                                #endregion

                                            GUILayout.EndHorizontal();
                                            #endregion

                                #region Formula
                            GUILayout.Label("Formula:");
                                if (skillSize > 0)
                                {
                                    skill[index].skillFormula = GUILayout.TextField(skill[index].skillFormula, GUILayout.Width(damageBox.width - 8), GUILayout.Height(damageBox.height / 4 - 17));
                                }
                                else
                                {
                                    GUILayout.TextField("Null", GUILayout.Width(damageBox.width - 8), GUILayout.Height(damageBox.height / 4 - 17));
                                }
            #endregion

                            #endregion
                            #region Variance CriticalHits
                            GUILayout.BeginHorizontal();

                                #region Variance
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Variance:");
                                    if (skillSize > 0)
                                    {
                                        skill[index].skillVariance = EditorGUILayout.IntField(skill[index].skillVariance, GUILayout.Width(.25f * (damageBox.width - 8)), GUILayout.Height(damageBox.height / 4 - 17));
                                    }
                                    else
                                    {
                                        EditorGUILayout.IntField(-1 , GUILayout.Width(.25f * (damageBox.width - 8)), GUILayout.Height(damageBox.height / 4 - 17));
                                    }
                                GUILayout.EndVertical();
                                #endregion

                                #region CriticalHits
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Critical Hits:");
                                    if (skillSize > 0)
                                    {
                                        skill[index].selectedCriticalHits = EditorGUILayout.Popup(skill[index].selectedCriticalHits, skillBool, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(.25f * (damageBox.width - 8)));
                                    }
                                    else
                                    {
                                        EditorGUILayout.Popup(0, skillBool, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(.25f * (damageBox.width - 8)));
                                    }
                                GUILayout.EndVertical();

                                GUILayout.Space(damageBox.width - (2 * .25f * (damageBox.width - 8)) - 20);
                                #endregion

                            GUILayout.EndHorizontal();
                            #endregion
                        GUILayout.EndVertical();

                    GUILayout.EndArea();
                    #endregion

                Rect effectsBox = new Rect(5, damageBox.height + 10, firstTabWidth + 15, position.height / 3);
                    #region Effects
                    ListReset();
                    GUILayout.BeginArea(effectsBox, tabStyle);
                        GUILayout.Label("Effects", EditorStyles.boldLabel);
                        GUILayout.Space(2);

                        #region Horizontal For Type And Content
                        GUILayout.BeginHorizontal();
                            GUILayout.Label(PadString("Type", string.Format("{0}", " Equipment Item")), GUILayout.Width(effectsBox.width));
                        GUILayout.EndHorizontal();
                        #endregion
                        
                        #region ScrollView
                        effectsScrollPos = GUILayout.BeginScrollView(
                            effectsScrollPos,
                            false,
                            true,
                            GUILayout.Width(firstTabWidth + 5),
                            GUILayout.Height(effectsBox.height * 0.725f)
                        );

                        GUI.changed = false;
                        GUI.skin.button.alignment = TextAnchor.MiddleLeft;

                        effectIndex = GUILayout.SelectionGrid(
                            effectIndex,
                            effectDisplayName.ToArray(),
                            1,
                            GUILayout.Width(firstTabWidth - 20),
                            GUILayout.Height(position.height / 24 * effectSize[index])
                        );

                        GUI.skin.button.alignment = TextAnchor.MiddleCenter;

                        GUILayout.EndScrollView();
                        #endregion

                        //Happen everytime selection grid is updated
                        if (GUI.changed)
                        {
                            if(effectIndex != effectIndexTemp)
                            {
                                EffectWindow.ShowWindow(effects, effectIndex, effectSize[index], TabType.Skill);
                                effectIndexTemp = effectIndex;
                            }
                        }

                        if((effects[effectSize[index] - 1].effectName != null && effects[effectSize[index] - 1].effectName != "") && effectSize[index] > 0)
                        {
                            effectIndex = 0;
                            ChangeMaximum<EffectData>(++effectSize[index], effects, PathDatabase.SkillEffectExplicitDataPath + (index + 1) + "/Effect_");
                        }

                        Color tempColor = GUI.backgroundColor;
                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("Delete All Data", GUILayout.Width(effectsBox.width * .3f)))
                        {
                            if (EditorUtility.DisplayDialog("Delete All Effect Data", "Are you sure want to delete all Effect Data?", "Yes", "No"))
                            {
                                effectIndex = 0;
                                effectSize[index] = 1;
                                ChangeMaximum<EffectData>(0, effects, PathDatabase.SkillEffectExplicitDataPath + (index + 1) + "/Effect_");
                                ChangeMaximum<EffectData>(1, effects, PathDatabase.SkillEffectExplicitDataPath + (index + 1) + "/Effect_");
                            }
                        }
                        GUI.backgroundColor = tempColor;
                    GUILayout.EndArea();
                    #endregion

                Rect notesBox = new Rect(5, damageBox.height + effectsBox.height + 17, firstTabWidth + 15, position.height - (damageBox.height + effectsBox.height + 17) - 40);
                    #region NoteBox
                    GUILayout.BeginArea(notesBox, tabStyle);
                        GUILayout.Space(2);
                        GUILayout.Label("Notes", EditorStyles.boldLabel);
                        GUILayout.Space(notesBox.height / 50);
                        if (skillSize > 0)
                        {
                            skill[index].notes = GUILayout.TextArea(skill[index].notes, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.9f));
                        }
                        else
                        {
                            GUILayout.TextArea("Null", GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * .85f + 8));
                        }
                    GUILayout.EndArea();
                    #endregion //End of notebox area

            GUILayout.EndArea();
            #endregion // End of Third Tab

        GUILayout.EndArea();
        #endregion // End of SkillTab

        EditorUtility.SetDirty(skill[index]);
    }

    #region Features

    ///<summary>
    ///Clears out the displayName list and add it with new value
    ///</summary>
    private void ListReset()
    {
        //Skill Reset
        skillDisplayName.Clear();
        for (int i = 0; i < skillSize; i++)
        {
            skillDisplayName.Add(skill[i].skillName);
        }

        //Effect Reset
        effectDisplayName.Clear();
        for (int i = 0; i < effectSize[index]; i++)
        {
            effectDisplayName.Add(effects[i].effectName);
        }
    }

    private void LoadElementTypeList()
    {
        TypeElementData[] typeElementData = Resources.LoadAll<TypeElementData>(PathDatabase.ElementRelativeDataPath);
        skillElement = new string[typeElementData.Length];
        for (int i = 0; i < skillElement.Length; i++)
        {
            skillElement[i] = typeElementData[i].dataName;
        }
    }
    private void LoadWeaponTypeList()
    {
        TypeWeaponData[] typeWeaponData = Resources.LoadAll<TypeWeaponData>(PathDatabase.WeaponRelativeDataPath);
        skillWeaponType = new string[typeWeaponData.Length];
        for (int i = 0; i < skillWeaponType.Length; i++)
        {
            skillWeaponType[i] = typeWeaponData[i].dataName;
        }
    }

    private void ClearNullScriptableObjects()
    {
        bool availableNull = true;
        while (availableNull)
        {
            availableNull = false;
            for (int i = 0; i < effectSize[index] - 1; i++)
            {
                if (effects[i].effectName == "" || effects[i].effectName == null)
                {
                    availableNull = true;
                    for (int j = i; j < effectSize[index] - 1; j++)
                    {
                        effects[j] = effects[j + 1];
                    }
                    effectIndex = 0;
                    ChangeMaximum<EffectData>(--effectSize[index], effects, PathDatabase.SkillEffectExplicitDataPath);
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
