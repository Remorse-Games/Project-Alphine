using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Runtime.CompilerServices;

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

    //Data(s) reference
    static ActorTraitsData thisClass;
    public static void ShowWindow(ActorTraitsData actorTraitData, int size)
    {
        var window = GetWindow<ActorTraitWindow>();
        var position = window.position;
        //Sizing
        window.maxSize = new Vector2(500, 190);
        window.minSize = new Vector2(500, 190);
        window.titleContent = new GUIContent("Traits");
        thisClass = actorTraitData;
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
                    thisClass.selectedTabIndex = GUILayout.SelectionGrid(thisClass.selectedTabIndex, tabNames, 6, GUILayout.Width(generalBox.width * .97f), GUILayout.Height(primaryBox.height * .1f));
                    GUILayout.BeginVertical();
                        float widthSpace = generalBox.width * .37f;
                        switch (thisClass.selectedTabIndex)
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
                        thisClass.traitName = StringMaker(thisClass.selectedTabIndex, thisClass.selectedTabToggle, thisClass.selectedArrayIndex, thisClass.traitValue);
                        GUILayout.Space(5);
                        GUILayout.BeginHorizontal();
                            GUILayout.Space(generalBox.width * .245f);
                            if (GUILayout.Button("OK", GUILayout.Width(generalBox.width * .17f), GUILayout.Height(20)))
                            {
                                this.Close();
                            }
                            if (GUILayout.Button("Cancel", GUILayout.Width(generalBox.width * .17f), GUILayout.Height(20)))
                            {
                                thisClass.selectedTabToggle = firstSelectedToggle;
                                thisClass.selectedTabIndex = firstSelectedTab;
                                thisClass.selectedArrayIndex = firstSelectedArray;
                                thisClass.traitValue = firstValue;
                                thisClass.traitName = firstTraitName;
                                this.Close();
                            }
                            if (GUILayout.Button("Clear", GUILayout.Width(generalBox.width * .17f), GUILayout.Height(20)))
                            {
                                thisClass.selectedTabToggle = 0;
                                thisClass.selectedTabIndex = 0;
                                thisClass.selectedArrayIndex = 0;
                                thisClass.traitValue = 0;
                                thisClass.traitName = null;
                                this.Close();
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

    private void RateTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(thisClass.selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(rateTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            if (thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, elementDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(rateTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            if (thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, CharacterDevelopmentData.debuffNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(rateTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            if (thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, CharacterDevelopmentData.stateNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(rateTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, CharacterDevelopmentData.stateNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
    }

    private void ParamTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(thisClass.selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(paramTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            if (thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(0, tabToggle);
             GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, CharacterDevelopmentData.debuffNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(paramTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            if (thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, CharacterDevelopmentData.exParameterNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("+");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(paramTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            if (thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, CharacterDevelopmentData.spParameterNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("*");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
    }
    private void AttackTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(thisClass.selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(attackTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  elementDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(attackTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            if (thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, CharacterDevelopmentData.stateNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("+");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(attackTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            if (thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(attackTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            if (thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("+");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
    }
    private void SkillTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(thisClass.selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(skillTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  skillDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(skillTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex, skillDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(skillTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  CharacterDevelopmentData.skillTypes, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(skillTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  CharacterDevelopmentData.skillTypes, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
    }
    private void EquipTab(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(thisClass.selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  weaponDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  armorDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  CharacterDevelopmentData.typeNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  CharacterDevelopmentData.typeNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(equipTabToggleList[4], tabToggle[4], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  CharacterDevelopmentData.slotType, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 4;
        }
        GUILayout.EndHorizontal();
    }
    private void Other(Rect generalBox, float widthSpace)
    {
        fieldWidth = generalBox.width * .2f;
        fieldHeight = generalBox.height * .12f;
        MemsetArray(thisClass.selectedTabToggle, tabToggle);
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(otherTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
        {
            if(thisClass.traitValue == -1)
            {
                thisClass.traitValue = 0;
            }
            MemsetArray(0, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("+");
                    thisClass.traitValue = EditorGUILayout.IntField(thisClass.traitValue, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(otherTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
        {
            MemsetArray(1, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  CharacterDevelopmentData.specialFlag, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(otherTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
        {
            MemsetArray(2, tabToggle);
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  CharacterDevelopmentData.collapseEffect, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 2;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.Toggle(otherTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
        {
            MemsetArray(3, tabToggle);
           GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    GUILayout.Label(" ");
                    thisClass.traitValue = -1;
                    thisClass.selectedArrayIndex = EditorGUILayout.Popup(thisClass.selectedArrayIndex,  CharacterDevelopmentData.partyAbility, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                    GUILayout.Space(widthSpace);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            GUILayout.EndVertical();
            thisClass.selectedTabToggle = 3;
        }
        GUILayout.EndHorizontal();
    }
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
            firstTraitName = thisClass.traitName;
            firstSelectedArray = thisClass.selectedArrayIndex;
            firstSelectedTab = thisClass.selectedTabIndex;
            firstSelectedToggle = thisClass.selectedTabToggle;
            firstValue = thisClass.traitValue;
        }
    }
    public string StringMaker(int selectedTabIndex, int selectedToggleIndex, int selectedArrayIndex, int value)
    {
        string outputString = "";
        string tabSpace = "  ";
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
            }
            else if(selectedTabIndex == 2)
            {
                if(selectedToggleIndex != 2)
                {
                    outputString += " +";
                }
                if (selectedToggleIndex > 0)
                {
                    outputString += " " + value.ToString();
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
            }
        }
        #endregion

        outputString += "%";
        return outputString;
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
