using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Runtime.CompilerServices;
using System.IO;
using SFB;
using System.Linq;

public class ActorTraitWindow : EditorWindow
{
    public string[] elementDisplayName;
    public string[] skillDisplayName;
    public string[] weaponDisplayName;
    public string[] armorDisplayName;
    public string[] rateTabToggleList =
    {
        "Element Rate",
        "Debuff Rate",
        "State Rate",
        "State Resist",
    };
    public string[] paramTabToggleList =
    {
        "Parameter",
        "Ex-Parameter",
        "Sp-Parameter",
    };
    public string[] attackTabToggleList =
    {
        "Attack Element",
        "Attack State",
        "Attack Speed",
        "Attack Times",
    };
    public string[] skillTabToggleList =
    {
        "Add Skill Type",
        "Seal Skill Type",
        "Add Skill",
        "Seal Skill",
    };
    public string[] equipTabToggleList =
    {
        "Equip Weapon",
        "Equip Armor",
        "Lock Equip",
        "Seal Equip",
        "Slot Type",
    };
    public string[] otherTabToggleList =
    {
        "Action Times",
        "Special Flag",
        "Collapse Effect",
        "Party Ability",
    };

    //Arrays
    bool[] tabToggle = new bool[5] { true, false, false, false , false};

    //All GUIStyle variable initialization.
    GUIStyle windowStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    public float fieldWidth;
    public float fieldHeight;
    public string[] tabNames =
    {
        "Rate",
        "Param",
        "Attack",
        "Skill",
        "Equip",
        "Other",
    };

    //Base Value
    public int i = 0;
    public int firstSelectedArray;
    public int firstSelectedTab;
    public int firstSelectedToggle;
    public int firstValue;
    public string firstTraitName;
    static int traitSize;
    static int traitIndex;

    //Data(s) reference
    static List<ActorTraitsData> traits;
    public static void ShowWindow(List<ActorTraitsData> actorTraitData, int index, int size)
    {
        var window = GetWindow<ActorTraitWindow>();
        var position = window.position;
        //Sizing
        window.maxSize = new Vector2(500, 190);
        window.minSize = new Vector2(500, 190);
        window.titleContent = new GUIContent("Traits");
        traits = actorTraitData;
        traitIndex = index;
        traitSize = size;
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

        // Getting Each Array List
        LoadArmorList();
        LoadElementList();
        LoadSkillList();
        LoadWeaponList();

        #region PrimaryTab
        Rect primaryBox = new Rect(0, 0, 500, 190);
        GUILayout.BeginArea(primaryBox, windowStyle);

            #region MainTab
            Rect generalBox = new Rect(5, 7, 490, 187);
            GUILayout.BeginArea(generalBox, columnStyle);
                GUILayout.BeginVertical("Box");
                    traits[traitIndex].selectedTabIndex = GUILayout.SelectionGrid(traits[traitIndex].selectedTabIndex, tabNames, 6, GUILayout.Width(generalBox.width * .97f), GUILayout.Height(primaryBox.height * .1f));
                    GUILayout.BeginVertical();
                        float widthSpace = generalBox.width * .37f;
                        switch (traits[traitIndex].selectedTabIndex)
                        {
                            case 5:
                                Other(generalBox, widthSpace);
                                break;
                            case 4:
                                EquipTab(generalBox, widthSpace);
                                break;
                            case 3:
                                SkillTab(generalBox, widthSpace);
                                break;
                            case 2:
                                AttackTab(generalBox, widthSpace);
                                break;
                            case 1:
                                ParamTab(generalBox, widthSpace);
                                break;
                            default:
                                RateTab(generalBox, widthSpace);
                                break;
                        }
                        traits[traitIndex].traitName = StringMaker(traits[traitIndex].selectedTabIndex, traits[traitIndex].selectedTabToggle, traits[traitIndex].selectedArrayIndex, traits[traitIndex].traitValue);
                        GUILayout.Space(5);
                        GUILayout.BeginHorizontal();
                            GUILayout.Space(generalBox.width * .155f);
                            // OK Button
                            if (GUILayout.Button("OK", GUILayout.Width(generalBox.width * .23f), GUILayout.Height(20)))
                            {
                                this.Close();
                            }
                            // OK Button
                            if (GUILayout.Button("Cancel", GUILayout.Width(generalBox.width * .23f), GUILayout.Height(20)))
                            {
                                traits[traitIndex].selectedTabToggle = firstSelectedToggle;
                                traits[traitIndex].selectedTabIndex = firstSelectedTab;
                                traits[traitIndex].selectedArrayIndex = firstSelectedArray;
                                traits[traitIndex].traitValue = firstValue;
                                traits[traitIndex].traitName = firstTraitName;
                                this.Close();
                            }
                            if(firstTraitName != null)
                            { 
                                if (GUILayout.Button("Clear", GUILayout.Width(generalBox.width * .23f), GUILayout.Height(20)))
                                {
                                    this.Close();
                                    for (int i = traitIndex; i < traitSize - 1; i++)
                                    {
                                        traits[i] = traits[i + 1];
                                    }
                                    traitIndex = 0;
                                    ChangeMaximum<ActorTraitsData>(--traitSize, traits, PathDatabase.ActorTraitExplicitDataPath);
                                    ActorTab.traitSize = traitSize;
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("Unable To Clear", GUILayout.Width(generalBox.width * .23f), GUILayout.Height(20)))
                                {

                                }
                            }
                        GUILayout.EndHorizontal();
                        GUILayout.Space(5);
                    GUILayout.EndVertical();
                GUILayout.EndVertical();
            GUILayout.EndArea();
            #endregion
        
        GUILayout.EndArea();
        #endregion
    }

    #region RateTab
    private void RateTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(traits[traitIndex].selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(rateTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            if (traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex, elementDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(rateTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            if (traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex, CharacterDevelopmentData.debuffNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(rateTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            if (traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex, CharacterDevelopmentData.stateNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(rateTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex, CharacterDevelopmentData.stateNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region ParamTab
    private void ParamTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(traits[traitIndex].selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(paramTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            if (traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(0, tabToggle);
             GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex, CharacterDevelopmentData.debuffNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(paramTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            if (traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex, CharacterDevelopmentData.exParameterNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("+");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(paramTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            if (traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex, CharacterDevelopmentData.spParameterNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region AttackTab
    private void AttackTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(traits[traitIndex].selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(attackTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  elementDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(attackTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            if (traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex, CharacterDevelopmentData.stateNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("+");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(attackTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            if (traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(attackTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            if (traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("+");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region SkillTab
    private void SkillTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(traits[traitIndex].selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(skillTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  skillDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(skillTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex, skillDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(skillTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  CharacterDevelopmentData.skillTypes, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(skillTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  CharacterDevelopmentData.skillTypes, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
    }
    #endregion
    
    #region EquipTab
    private void EquipTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(traits[traitIndex].selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  weaponDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  armorDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  CharacterDevelopmentData.typeNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  CharacterDevelopmentData.typeNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[4], tabToggle[4], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  CharacterDevelopmentData.slotType, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 4;
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region OtherTab
    private void Other(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(traits[traitIndex].selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(otherTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            if(traits[traitIndex].traitValue == -1)
            {
                traits[traitIndex].traitValue = 0;
            }
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("+");
                    traits[traitIndex].traitValue = EditorGUILayout.IntField(traits[traitIndex].traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(otherTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  CharacterDevelopmentData.specialFlag, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(otherTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  CharacterDevelopmentData.collapseEffect, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(otherTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
           GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    traits[traitIndex].traitValue = -1;
                    traits[traitIndex].selectedArrayIndex = EditorGUILayout.Popup(traits[traitIndex].selectedArrayIndex,  CharacterDevelopmentData.partyAbility, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            traits[traitIndex].selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region Features
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
    private void LoadElementList()
    {
        TypeElementData[] elementData = Resources.LoadAll<TypeElementData>(PathDatabase.ElementRelativeDataPath);
        elementDisplayName = new string[elementData.Length];
        for (int i = 0; i < elementDisplayName.Length; i++)
        {
            elementDisplayName[i] = elementData[i].dataName;
        }
    }
    
    private void LoadSkillList()
    {
        TypeSkillData[] skillData = Resources.LoadAll<TypeSkillData>(PathDatabase.SkillRelativeDataPath);
        skillDisplayName = new string[skillData.Length];
        for (int i = 0; i < skillDisplayName.Length; i++)
        {
            skillDisplayName[i] = skillData[i].dataName;
        }
    }
    private void LoadWeaponList()
    {
        TypeWeaponData[] weaponData = Resources.LoadAll<TypeWeaponData>(PathDatabase.WeaponRelativeDataPath);
        weaponDisplayName = new string[weaponData.Length];
        for (int i = 0; i < weaponDisplayName.Length; i++)
        {
            weaponDisplayName[i] = weaponData[i].dataName;
        }
    }
    private void LoadArmorList()
    {
        TypeArmorData[] armorData = Resources.LoadAll<TypeArmorData>(PathDatabase.ArmorRelativeDataPath);
        armorDisplayName = new string[armorData.Length];
        for (int i = 0; i < armorDisplayName.Length; i++)
        {
            armorDisplayName[i] = armorData[i].dataName;
        }
    }
    public void BaseValue(int i)
    {
        if (i == 0)
        {
            firstTraitName = traits[traitIndex].traitName;
            firstSelectedArray = traits[traitIndex].selectedArrayIndex;
            firstSelectedTab = traits[traitIndex].selectedTabIndex;
            firstSelectedToggle = traits[traitIndex].selectedTabToggle;
            firstValue = traits[traitIndex].traitValue;
        }
    }
    public string StringMaker(int selectedTabIndex, int selectedToggleIndex, int selectedArrayIndex, int value)
    {
        string outputString = "";
        string tabSpace = "  ";
        #region Tab Index Selecting
        switch (selectedTabIndex)
        {
            case 5:
                outputString = otherTabToggleList[selectedToggleIndex] + tabSpace;
                break;
            case 4:
                outputString = equipTabToggleList[selectedToggleIndex] + tabSpace;
                break;
            case 3:
                outputString = skillTabToggleList[selectedToggleIndex] + tabSpace;
                break;
            case 2:
                outputString = attackTabToggleList[selectedToggleIndex] + tabSpace;
                break;
            case 1:
                outputString = paramTabToggleList[selectedToggleIndex] + tabSpace;
                break;
            case 0:
                outputString = rateTabToggleList[selectedToggleIndex] + tabSpace;
                break;
        }
        #endregion
        #region Tab With Array List Available
        if (selectedTabIndex == 0)
        {
            if(selectedToggleIndex == 0)
            {
                outputString += elementDisplayName[selectedArrayIndex];
            }
            else if(selectedToggleIndex == 1)
            {
                outputString += CharacterDevelopmentData.debuffNames[selectedArrayIndex];
            }
            else if(selectedToggleIndex >= 2)
            {
                outputString += CharacterDevelopmentData.stateNames[selectedArrayIndex];
            }
        }
        else if(selectedTabIndex == 1)
        {
            if (selectedToggleIndex == 0)
            {
                outputString += CharacterDevelopmentData.debuffNames[selectedArrayIndex];
            }
            else if (selectedToggleIndex == 1)
            {
                outputString += CharacterDevelopmentData.exParameterNames[selectedArrayIndex];
            }
            else if (selectedToggleIndex == 2)
            {
                outputString += CharacterDevelopmentData.spParameterNames[selectedArrayIndex];
            }
        }
        else if (selectedTabIndex == 2)
        {
            if (selectedToggleIndex == 0)
            {
                outputString += elementDisplayName[selectedArrayIndex];
            }
            else if (selectedToggleIndex == 1)
            {
                outputString += CharacterDevelopmentData.stateNames[selectedArrayIndex];
            }
        }
        else if (selectedTabIndex == 3)
        {
            if (selectedToggleIndex == 0)
            {
                outputString += skillDisplayName[selectedArrayIndex];
            }
            else if (selectedToggleIndex == 1)
            {
                outputString += skillDisplayName[selectedArrayIndex];
            }
            else if (selectedToggleIndex == 2)
            {
                outputString += CharacterDevelopmentData.skillTypes[selectedArrayIndex];
            }
            else
            {
                outputString += CharacterDevelopmentData.skillTypes[selectedArrayIndex];
            }
        }
        else if(selectedTabIndex == 4)
        {
            if (selectedToggleIndex == 0)
            {
                outputString += weaponDisplayName[selectedArrayIndex];
            }
            else if (selectedToggleIndex == 1)
            {
                outputString += armorDisplayName[selectedArrayIndex];
            }
            else if (selectedToggleIndex == 2 || selectedTabIndex == 3)
            {
                outputString += CharacterDevelopmentData.typeNames[selectedArrayIndex];
            }
            else
            {
                outputString += CharacterDevelopmentData.slotType[selectedArrayIndex];
            }
        }
        #endregion
        #region Value Naming
        if (value != -1)
        {
            if(selectedTabIndex == 0)
            {
                if (selectedToggleIndex < 3)
                {
                    outputString += " * " + value.ToString();
                    outputString += "%";
                }
            }
            else if(selectedTabIndex == 1)
            {
                if(selectedToggleIndex != 1)
                {
                    outputString += " *";
                }
                else
                {
                    outputString += " +";
                }
                outputString += " " + value.ToString();
                outputString += "%";
            }
            else if(selectedTabIndex == 2)
            {
                if(selectedToggleIndex != 2)
                {
                    outputString += " +";
                    outputString += "%";
                }
                if (selectedToggleIndex > 0)
                {
                    outputString += " " + value.ToString();
                    outputString += "%";
                }
            }
            else if(selectedTabIndex == 5)
            {
                if(selectedToggleIndex == 0)
                {
                    outputString += " + " + value.ToString();
                }
                else
                {
                    outputString += " " + value.ToString();
                }
                outputString += "%";
            }
        }
        #endregion

        return outputString;
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
