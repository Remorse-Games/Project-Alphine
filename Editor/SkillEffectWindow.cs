using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Runtime.CompilerServices;
using System.IO;
using SFB;
using System.Linq;

public class SkillEffectWindow : EditorWindow
{
    private string[] skillDisplayName;
    private bool[] tabToggle = new bool[4] { true, false, false, false };

    //All GUIStyle variable initialization.
    GUIStyle windowStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    public string[] recoverTabToggleList =
    {
        "Recover HP",
        "Recover MP",
        "Gain TP"
    };

    public string[] stateTabToggleList =
    {
        "Add State",
        "Remove State"
    };

    public string[] paramTabToggleList =
    {
        "Add Buff",
        "Add Debuff",
        "Remove Buff",
        "Remove Debuff"
    };

    public string[] otherTabToggleList =
    {
        "Special Effect",
        "Grow",
        "Learn Skill"
    };

    public string[] tabNames =
    {
        "Recover",
        "State",
        "Param",
        "Other"
    };


    static List<SkillEffectData> effects;
    static int effectSize;
    static int effectIndex;

    //start value
    public bool set = false;
    public int firstSelectedArray;
    public int firstSelectedTab;
    public int firstSelectedToggle;
    public int[] firstValue = new int[2];
    public string firstEffectName;

    public static void ShowWindow(List<SkillEffectData> skillEffectData, int index, int size)
    {
        var window = GetWindow<SkillEffectWindow>();
        var position = window.position;

        //sizing and positioning
        window.maxSize = new Vector2(400, 190);
        window.minSize = new Vector2(400, 190);
        position.center = new Rect(Screen.width * -1 * .05f, Screen.height * -1 * .05f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        window.position = position;

        effects = skillEffectData;
        effectIndex = index;
        effectSize = size;

        window.titleContent = new GUIContent("Effect");
        window.Show();
    }

    private void OnGUI()
    {
        SetStartValue();
        LoadSkillList();

        //set window color
        windowStyle = new GUIStyle(GUI.skin.box);
        windowStyle.normal.background = CreateTexture(1, 1, Color.gray);

        //set column color
        columnStyle = new GUIStyle(GUI.skin.box);
        columnStyle.normal.background = CreateTexture(1, 1, new Color32(70, 70, 70, 200));

        //set tabs color
        tabStyle = new GUIStyle(GUI.skin.box);
        Color32 proSkin = new Color32(150, 150, 150, 100);
        Color32 normalSkin = new Color32(200, 200, 200, 100);
        tabStyle.normal.background = CreateTexture(1, 1, EditorGUIUtility.isProSkin ? proSkin : normalSkin);

        #region PrimaryTab
        Rect primaryBox = new Rect(0, 0, 400, 190);
        GUILayout.BeginArea(primaryBox, windowStyle);

            #region MainTab
            Rect generalBox = new Rect(5, 7, 390, 187);

            GUILayout.BeginArea(generalBox, columnStyle);
                GUILayout.BeginVertical("Box");

                    effects[effectIndex].selectedTabIndex = GUILayout.SelectionGrid
                    (
                        effects[effectIndex].selectedTabIndex, 
                        tabNames, 
                        6, 
                        GUILayout.Width(generalBox.width * .97f), 
                        GUILayout.Height(primaryBox.height * .15f)
                    );

                    GUILayout.Space(5);
                    GUILayout.BeginVertical();

                        float widthSpace = generalBox.width * .37f;
                        switch (effects[effectIndex].selectedTabIndex)
                        {
                            case 0:
                                RecoverTab(generalBox, widthSpace);
                                break;
                            case 1:
                                StateTab(generalBox, widthSpace);
                                break;
                            case 2:
                                ParamTab(generalBox, widthSpace);
                                break;
                            case 3:
                                OtherTab(generalBox, widthSpace);
                                break;
                        }

                        effects[effectIndex].effectName = StringMaker(effects[effectIndex].selectedTabIndex, effects[effectIndex].selectedTabToggle, effects[effectIndex].selectedArrayIndex, effects[effectIndex].effectValue);
                        GUILayout.Space(5);
                        GUILayout.BeginHorizontal();

                            // OK Button
                            if(GUILayout.Button("OK", GUILayout.Height(20)))
                            {
                                this.Close();
                            }

                            // Cancel Button
                            if (GUILayout.Button("Cancel", GUILayout.Height(20)))
                            {
                                if(firstEffectName != null)
                                {
                                    effects[effectIndex].selectedTabToggle = firstSelectedToggle;
                                    effects[effectIndex].selectedTabIndex = firstSelectedTab;
                                    effects[effectIndex].selectedArrayIndex = firstSelectedArray;
                                    effects[effectIndex].effectValue = firstValue;
                                    effects[effectIndex].effectName = firstEffectName;
                                    this.Close();
                                }
                                else
                                {
                                    this.Close();
                                    for (int i = effectIndex; i < effectSize - 1; i++)
                                    {
                                        effects[i].effectName = effects[i + 1].effectName;
                                        effects[i].effectValue = effects[i + 1].effectValue;
                                        effects[i].selectedArrayIndex = effects[i + 1].selectedArrayIndex;
                                        effects[i].selectedTabIndex = effects[i + 1].selectedTabIndex;
                                        effects[i].selectedTabToggle = effects[i + 1].selectedTabToggle;
                                    }
                                    effectIndex = 0;
                                    ChangeMaximum<SkillEffectData>(--effectSize, effects, PathDatabase.SkillEffectExplicitDataPath + (SkillsTab.index + 1) + "/Effect_");

                                    if(effectSize <= 0)
                                    {
                                        ChangeMaximum<SkillEffectData>(1, effects, PathDatabase.SkillEffectExplicitDataPath + (SkillsTab.index + 1) + "/Effect_");
                                        effectSize = 1;
                                    }

                                    SkillsTab.effectSize[SkillsTab.index] = effectSize;
                                }
                            }

                            if(firstEffectName != null)
                            {
                                if (GUILayout.Button("Clear", GUILayout.Height(20)))
                                {
                                    this.Close();
                                    for (int i = effectIndex; i < effectSize - 1; i++)
                                    {
                                        effects[i].effectName = effects[i + 1].effectName;
                                        effects[i].effectValue = effects[i + 1].effectValue;
                                        effects[i].selectedArrayIndex = effects[i + 1].selectedArrayIndex;
                                        effects[i].selectedTabIndex = effects[i + 1].selectedTabIndex;
                                        effects[i].selectedTabToggle = effects[i + 1].selectedTabToggle;
                                    }
                                    effectIndex = 0;
                                    ChangeMaximum<SkillEffectData>(--effectSize, effects, PathDatabase.SkillTabExplicitDataPath + (SkillsTab.index + 1) + "/Effect_");
                
                                    if(effectSize <= 0)
                                    {
                                        ChangeMaximum<SkillEffectData>(1, effects, PathDatabase.SkillEffectExplicitDataPath + (SkillsTab.index + 1) + "/Effect_");
                                        effectSize = 1;
                                    }

                                    SkillsTab.effectSize[SkillsTab.index] = effectSize;
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("Unable To Clear", GUILayout.Height(20)))
                                {

                                }
                            }

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
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

    #region RecoverTab

    void RecoverTab(Rect generalBox, float widthSpace)
    {
        float fieldWidth = generalBox.width * .2f;
        float fieldHeight = generalBox.height * .15f;

        if(effects[effectIndex].selectedTabToggle > recoverTabToggleList.Count() - 1)
        {
            MemsetArray(recoverTabToggleList.Count() - 1, tabToggle);
            effects[effectIndex].selectedTabToggle = recoverTabToggleList.Count() - 1;
        }

        GUILayout.BeginHorizontal();

            if (EditorGUILayout.Toggle(recoverTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
            {
                if(effects[effectIndex].effectValue[0] == -1)
                {
                    effects[effectIndex].effectValue[0] = 0;
                }

                if(effects[effectIndex].effectValue[1] == -1)
                {
                    effects[effectIndex].effectValue[1] = 0;
                }

                MemsetArray(0, tabToggle);

                GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();

                        effects[effectIndex].effectValue[0] = EditorGUILayout.IntField(effects[effectIndex].effectValue[0], GUILayout.Width(fieldWidth));
                        GUILayout.Label("%", GUILayout.Width(15));

                        if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[0] < 100)
                        {
                            effects[effectIndex].effectValue[0]++;
                        }

                        if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[0] > 0)
                        {
                            effects[effectIndex].effectValue[0]--;
                        }

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                        effects[effectIndex].effectValue[1] = EditorGUILayout.IntField(effects[effectIndex].effectValue[1], GUILayout.Width(fieldWidth));
                        GUILayout.Space(20);

                        if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] < 100)
                        {
                            effects[effectIndex].effectValue[1]++;
                        }

                        if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] > 0)
                        {
                            effects[effectIndex].effectValue[1]--;
                        }

                    GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                effects[effectIndex].selectedTabToggle = 0;
            }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

            if (EditorGUILayout.Toggle(recoverTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
            {
                if(effects[effectIndex].effectValue[0] == -1)
                {
                    effects[effectIndex].effectValue[0] = 0;
                }

                if(effects[effectIndex].effectValue[1] == -1)
                {
                    effects[effectIndex].effectValue[1] = 0;
                }

                MemsetArray(1, tabToggle);

                GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();

                        effects[effectIndex].effectValue[0] = EditorGUILayout.IntField(effects[effectIndex].effectValue[0], GUILayout.Width(fieldWidth));
                        GUILayout.Label("%", GUILayout.Width(15));

                        if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[0] < 100)
                        {
                            effects[effectIndex].effectValue[0]++;
                        }

                        if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[0] > 0)
                        {
                            effects[effectIndex].effectValue[0]--;
                        }

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                        effects[effectIndex].effectValue[1] = EditorGUILayout.IntField(effects[effectIndex].effectValue[1], GUILayout.Width(fieldWidth));
                        GUILayout.Space(20);

                        if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] < 100)
                        {
                            effects[effectIndex].effectValue[1]++;
                        }

                        if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] > 0)
                        {
                            effects[effectIndex].effectValue[1]--;
                        }

                    GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                effects[effectIndex].selectedTabToggle = 1;
            }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

            if (EditorGUILayout.Toggle(recoverTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
            {
                if(effects[effectIndex].effectValue[0] == -1)
                {
                    effects[effectIndex].effectValue[0] = 0;
                }

                if(effects[effectIndex].effectValue[1] == -1)
                {
                    effects[effectIndex].effectValue[1] = 0;
                }

                MemsetArray(2, tabToggle);

                GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();

                        effects[effectIndex].effectValue[1] = EditorGUILayout.IntField(effects[effectIndex].effectValue[1], GUILayout.Width(fieldWidth));
                        GUILayout.Space(20);

                        if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] < 100)
                        {
                            effects[effectIndex].effectValue[1]++;
                        }

                        if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] > 0)
                        {
                            effects[effectIndex].effectValue[1]--;
                        }

                    GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                effects[effectIndex].selectedTabToggle = 2;
            }

        GUILayout.EndHorizontal();
    }
    #endregion

    #region StateTab

    void StateTab(Rect generalBox, float widthSpace)
    {
        float fieldWidth = generalBox.width * .2f;
        float fieldHeight = generalBox.height * .12f;

        if (effects[effectIndex].selectedTabToggle > stateTabToggleList.Count() - 1)
        {
            MemsetArray(stateTabToggleList.Count() - 1, tabToggle);
            effects[effectIndex].selectedTabToggle = stateTabToggleList.Count() - 1;
        }

        GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

                if(EditorGUILayout.Toggle(stateTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
                {
                    if(effects[effectIndex].effectValue[0] == -1)
                    {
                        effects[effectIndex].effectValue[0] = 0;
                    }

                    if(effects[effectIndex].effectValue[1] == -1)
                    {
                        effects[effectIndex].effectValue[1] = 0;
                    }

                    MemsetArray(0, tabToggle);

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].selectedArrayIndex = EditorGUILayout.Popup(effects[effectIndex].selectedArrayIndex, CharacterDevelopmentData.stateNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].effectValue[0] = EditorGUILayout.IntField(effects[effectIndex].effectValue[0], GUILayout.Width(fieldWidth));
                            GUILayout.Label("%", GUILayout.Width(15));

                            if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[0] < 100)
                            {
                                effects[effectIndex].effectValue[0]++;
                            }

                            if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[0] > 0)
                            {
                                effects[effectIndex].effectValue[0]--;
                            }

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    effects[effectIndex].selectedTabToggle = 0;
                }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

                if(EditorGUILayout.Toggle(stateTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
                {
                    if(effects[effectIndex].effectValue[0] == -1)
                    {
                        effects[effectIndex].effectValue[0] = 0;
                    }

                    if(effects[effectIndex].effectValue[1] == -1)
                    {
                        effects[effectIndex].effectValue[1] = 0;
                    }

                    MemsetArray(1, tabToggle);

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].selectedArrayIndex = EditorGUILayout.Popup(effects[effectIndex].selectedArrayIndex, CharacterDevelopmentData.stateNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].effectValue[0] = EditorGUILayout.IntField(effects[effectIndex].effectValue[0], GUILayout.Width(fieldWidth));
                            GUILayout.Label("%", GUILayout.Width(15));

                            if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[0] < 100)
                            {
                                effects[effectIndex].effectValue[0]++;
                            }

                            if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[0] > 0)
                            {
                                effects[effectIndex].effectValue[0]--;
                            }

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    effects[effectIndex].selectedTabToggle = 1;
                }

            GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
    #endregion

    #region ParamTab
    void ParamTab(Rect generalBox, float widthSpace)
    {
        float fieldWidth = generalBox.width * .2f;
        float fieldHeight = generalBox.height * .12f;

        if (effects[effectIndex].selectedTabToggle > paramTabToggleList.Count() - 1)
        {
            MemsetArray(paramTabToggleList.Count() - 1, tabToggle);
            effects[effectIndex].selectedTabToggle = paramTabToggleList.Count() - 1;
        }

        GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

                if (EditorGUILayout.Toggle(paramTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
                {
                    if(effects[effectIndex].effectValue[0] == -1)
                    {
                        effects[effectIndex].effectValue[0] = 0;
                    }

                    if(effects[effectIndex].effectValue[1] == -1)
                    {
                        effects[effectIndex].effectValue[1] = 0;
                    }

                    MemsetArray(0, tabToggle);

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].selectedArrayIndex = EditorGUILayout.Popup(effects[effectIndex].selectedArrayIndex, CharacterDevelopmentData.debuffNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].effectValue[1] = EditorGUILayout.IntField(effects[effectIndex].effectValue[1], GUILayout.Width(fieldWidth));

                            if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] < 100)
                            {
                                effects[effectIndex].effectValue[1]++;
                            }

                            if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] > 0)
                            {
                                effects[effectIndex].effectValue[1]--;
                            }

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    effects[effectIndex].selectedTabToggle = 0;
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                if (EditorGUILayout.Toggle(paramTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
                {
                    if(effects[effectIndex].effectValue[0] == -1)
                    {
                        effects[effectIndex].effectValue[0] = 0;
                    }

                    if(effects[effectIndex].effectValue[1] == -1)
                    {
                        effects[effectIndex].effectValue[1] = 0;
                    }

                    MemsetArray(1, tabToggle);

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].selectedArrayIndex = EditorGUILayout.Popup(effects[effectIndex].selectedArrayIndex, CharacterDevelopmentData.debuffNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].effectValue[1] = EditorGUILayout.IntField(effects[effectIndex].effectValue[1], GUILayout.Width(fieldWidth));

                            if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] < 100)
                            {
                                effects[effectIndex].effectValue[1]++;
                            }

                            if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] > 0)
                            {
                                effects[effectIndex].effectValue[1]--;
                            }

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    effects[effectIndex].selectedTabToggle = 1;
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                if (EditorGUILayout.Toggle(paramTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
                {
                    if(effects[effectIndex].effectValue[0] == -1)
                    {
                        effects[effectIndex].effectValue[0] = 0;
                    }

                    if(effects[effectIndex].effectValue[1] == -1)
                    {
                        effects[effectIndex].effectValue[1] = 0;
                    }

                    MemsetArray(2, tabToggle);

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].selectedArrayIndex = EditorGUILayout.Popup(effects[effectIndex].selectedArrayIndex, CharacterDevelopmentData.debuffNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    effects[effectIndex].selectedTabToggle = 2;
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                if (EditorGUILayout.Toggle(paramTabToggleList[3], tabToggle[3], EditorStyles.radioButton))
                {
                    if(effects[effectIndex].effectValue[0] == -1)
                    {
                        effects[effectIndex].effectValue[0] = 0;
                    }

                    if(effects[effectIndex].effectValue[1] == -1)
                    {
                        effects[effectIndex].effectValue[1] = 0;
                    }

                    MemsetArray(3, tabToggle);

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].selectedArrayIndex = EditorGUILayout.Popup(effects[effectIndex].selectedArrayIndex, CharacterDevelopmentData.debuffNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    effects[effectIndex].selectedTabToggle = 3;
                }

            GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
    #endregion

    #region OtherTab
    void OtherTab(Rect generalBox, float widthSpace)
    {
        float fieldWidth = generalBox.width * .2f;
        float fieldHeight = generalBox.height * .12f;

        if (effects[effectIndex].selectedTabToggle > otherTabToggleList.Count() - 1)
        {
            MemsetArray(otherTabToggleList.Count() - 1, tabToggle);
            effects[effectIndex].selectedTabToggle = otherTabToggleList.Count() - 1;
        }

        GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

                if (EditorGUILayout.Toggle(otherTabToggleList[0], tabToggle[0], EditorStyles.radioButton))
                {
                    if(effects[effectIndex].effectValue[0] == -1)
                    {
                        effects[effectIndex].effectValue[0] = 0;
                    }

                    if(effects[effectIndex].effectValue[1] == -1)
                    {
                        effects[effectIndex].effectValue[1] = 0;
                    }

                    MemsetArray(0, tabToggle);

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].selectedArrayIndex = EditorGUILayout.Popup(effects[effectIndex].selectedArrayIndex, CharacterDevelopmentData.specialEffect, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    effects[effectIndex].selectedTabToggle = 0;
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                if (EditorGUILayout.Toggle(otherTabToggleList[1], tabToggle[1], EditorStyles.radioButton))
                {
                    if(effects[effectIndex].effectValue[0] == -1)
                    {
                        effects[effectIndex].effectValue[0] = 0;
                    }

                    if(effects[effectIndex].effectValue[1] == -1)
                    {
                        effects[effectIndex].effectValue[1] = 0;
                    }

                    MemsetArray(1, tabToggle);

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].selectedArrayIndex = EditorGUILayout.Popup(effects[effectIndex].selectedArrayIndex, CharacterDevelopmentData.debuffNames, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].effectValue[1] = EditorGUILayout.IntField(effects[effectIndex].effectValue[1], GUILayout.Width(fieldWidth));

                            if (GUILayout.Button("+", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] < 100)
                            {
                                effects[effectIndex].effectValue[1]++;
                            }

                            if (GUILayout.Button("-", GUILayout.Width(25)) && effects[effectIndex].effectValue[1] > 0)
                            {
                                effects[effectIndex].effectValue[1]--;
                            }

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    effects[effectIndex].selectedTabToggle = 1;
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                if (EditorGUILayout.Toggle(otherTabToggleList[2], tabToggle[2], EditorStyles.radioButton))
                {
                    if(effects[effectIndex].effectValue[0] == -1)
                    {
                        effects[effectIndex].effectValue[0] = 0;
                    }

                    if(effects[effectIndex].effectValue[1] == -1)
                    {
                        effects[effectIndex].effectValue[1] = 0;
                    }

                    MemsetArray(2, tabToggle);

                    GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();

                            effects[effectIndex].selectedArrayIndex = EditorGUILayout.Popup(effects[effectIndex].selectedArrayIndex, skillDisplayName, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    effects[effectIndex].selectedTabToggle = 2;
                }

                GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
    #endregion

    #region Features
    public void MemsetArray(int checkedTrue, bool[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = false;
        }
        arr[(checkedTrue)] = true;
    }

    public string StringMaker(int selectedTabIndex, int selectedToggleIndex, int selectedArrayIndex, int[] value)
    {
        string outputString = "";
        string val = "";

        switch (selectedTabIndex)
        {
            case 0:
                switch (selectedToggleIndex)
                {
                    case 2:
                        outputString = PadString(recoverTabToggleList[selectedToggleIndex], value[1].ToString());
                        break;
                    default:
                        val = string.Format("{0}% + {1}", value[0], value[1]);
                        outputString = PadString(recoverTabToggleList[selectedToggleIndex], val);
                        break;
                }
                break;
            case 1:
                val = string.Format("{0} {1}%", CharacterDevelopmentData.stateNames[selectedArrayIndex], value[0]);
                outputString = PadString(stateTabToggleList[selectedToggleIndex], val);
                break;
            case 2:
                switch (selectedToggleIndex)
                {
                    case 0:
                    case 1:
                        val = string.Format("{0} {1} turns", CharacterDevelopmentData.debuffNames[selectedArrayIndex], value[1]);
                        outputString = PadString(paramTabToggleList[selectedToggleIndex], val);
                        break;
                    default:
                        val = CharacterDevelopmentData.debuffNames[selectedArrayIndex];
                        outputString = PadString(paramTabToggleList[selectedToggleIndex], val);
                        break;
                }
                break;
            case 3:
                switch (selectedToggleIndex)
                {
                    case 0:
                        val = string.Format("{0}", CharacterDevelopmentData.specialEffect[selectedArrayIndex]);
                        outputString = PadString(otherTabToggleList[selectedToggleIndex]+"  ", val);
                        break;
                    case 1:
                        val = string.Format("{0} {1}", CharacterDevelopmentData.debuffNames[selectedArrayIndex], value[1]);
                        outputString = PadString(otherTabToggleList[selectedToggleIndex], val);
                        break;
                    case 2:
                        val = string.Format("{0}", skillDisplayName[selectedArrayIndex]);
                        outputString = PadString(otherTabToggleList[selectedToggleIndex], val);
                        break;
                }
                break;
        }

        return outputString;
    }

    public string PadString(string key, string value)
    {
        int pad = 4 - (key.Length / 4);

        if (key.Length >= 12)
        {
            pad++;
        }
        if (key.Length >= 14)
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
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from effectSize</param>
    /// <param name="listTabData">list of item that you want to display.</param>
    /// <param name="dataTabName">get size from effectSize</param>
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

    public void SetStartValue()
    {
        if (set) return;

        firstSelectedArray = effects[effectIndex].selectedArrayIndex;
        firstSelectedTab = effects[effectIndex].selectedTabIndex;
        firstSelectedToggle = effects[effectIndex].selectedTabToggle;
        firstValue = effects[effectIndex].effectValue;
        firstEffectName = effects[effectIndex].effectName;
    }

    private void LoadSkillList()
    {
        if (set) return;

        SkillData[] skillData = Resources.LoadAll<SkillData>(PathDatabase.SkillTabRelativeDataPath);
        skillDisplayName = new string[skillData.Length];

        for (int i = 0; i < skillDisplayName.Length; i++)
        {
            skillDisplayName[i] = skillData[i].skillName;
        }

        set = true;
    }

    #endregion
}
