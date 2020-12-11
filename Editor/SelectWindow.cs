using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

// will remove this when find better solution
public enum SelectType
{
    Skill,
    Actor
}

public class SelectWindow : EditorWindow
{
    public static SystemData data;
    public static SelectType type;

    public static int index;

    //All GUIStyle variable initialization.
    GUIStyle windowStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    private List<string> DataList = new List<string>();

    public static List<string> list = new List<string>();

    private int SelectedActorIndex = 0;

    private Vector2 scrollPos;

    bool set = false;

    public static void ShowWindow(SystemData _data, int _index, SelectType _type)
    {
        var window = GetWindow<SelectWindow>();
        var position = window.position;

        //sizing and positioning
        window.maxSize = new Vector2(200, 190);
        window.minSize = new Vector2(200, 190);
        position.center = new Rect(Screen.width * -1 * .05f, Screen.height * -1 * .05f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        window.position = position;

        data = _data;
        index = _index;
        type = _type;

        switch (type)
        {
            case SelectType.Actor:
                list = data.startingParty;
                break;

            case SelectType.Skill:
                list = data.magicSkills;
                break;
        }

        window.titleContent = new GUIContent("Effect");
        window.Show();
    }

    private void OnGUI()
    {
        LoadActorList();

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

        Rect primaryBox = new Rect(0, 0, 200, 190);
        GUILayout.BeginArea(primaryBox, windowStyle);

            #region Main Tab

            Rect generalBox = new Rect(5, 7, 190, 187);

            GUILayout.BeginArea(generalBox, columnStyle);

                #region ScrollPos

                scrollPos = GUILayout.BeginScrollView(
                    scrollPos,
                    false,
                    true,
                    GUILayout.Height(position.height - 40)
                );

                    SelectedActorIndex = GUILayout.SelectionGrid
                    (
                        SelectedActorIndex,
                        DataList.ToArray(),
                        1
                    );

                GUILayout.EndScrollView();

                #endregion

                GUILayout.BeginHorizontal();

                    if (GUILayout.Button("ok"))
                    {
                        // save and close
                        list[index] = DataList[SelectedActorIndex];
                
                        if(index == list.Count - 1)
                        {
                            list.Add("");
                        }


                        this.Close();
                    }

                    if (GUILayout.Button("cancel"))
                    {
                        // close
                        this.Close();
                    }

                    Color tempColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;

                    EditorGUI.BeginDisabledGroup(list[index] == "");

                    if (GUILayout.Button("delete"))
                    {
                        list.RemoveAt(index);
                        this.Close();
                    }

                    EditorGUI.EndDisabledGroup();

                    GUI.backgroundColor = tempColor;

                GUILayout.EndHorizontal();

            GUILayout.EndArea();

            #endregion

        GUILayout.EndArea();

        #endregion
    }

    private void OnDestroy()
    {
        SystemTab.selectedStartingPartyIndex = -1;
        SystemTab.selectedMagicSkillIndex = -1;
    }

    #region Features

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

    private void LoadActorList()
    {
        if(!set)
        {
            switch (type)
            {
                case SelectType.Actor:
                    ActorData[] actor = Resources.LoadAll<ActorData>(PathDatabase.ActorRelativeDataPath);
                    DataList = actor.Select(x => x.actorName).ToList();
                    break;

                case SelectType.Skill:
                    TypeSkillData[] skill = Resources.LoadAll<TypeSkillData>(PathDatabase.SkillRelativeDataPath);
                    DataList = skill.Select(x => x.dataName).ToList();
                    break;
            }

            set = true;
        }
    }

    #endregion

}
