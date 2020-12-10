using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class StartingPartyWindow : EditorWindow
{
    public static SystemData data;

    public static int index;

    //All GUIStyle variable initialization.
    GUIStyle windowStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    private List<string> ActorList = new List<string>();

    private int SelectedActorIndex = 0;

    private Vector2 scrollPos;

    bool set = false;

    public static void ShowWindow(SystemData _data, int _index)
    {
        var window = GetWindow<StartingPartyWindow>();
        var position = window.position;

        //sizing and positioning
        window.maxSize = new Vector2(200, 190);
        window.minSize = new Vector2(200, 190);
        position.center = new Rect(Screen.width * -1 * .05f, Screen.height * -1 * .05f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        window.position = position;

        data = _data;
        index = _index;

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
                        ActorList.ToArray(),
                        1
                    );

                GUILayout.EndScrollView();

                #endregion

                GUILayout.BeginHorizontal();

                    if (GUILayout.Button("ok"))
                    {
                        // save and close
                        data.startingParty[index] = ActorList[SelectedActorIndex];
                
                        if(index == data.startingParty.Count - 1)
                        {
                            data.startingParty.Add("");
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

                    EditorGUI.BeginDisabledGroup(data.startingParty[index] == "");

                    if (GUILayout.Button("delete"))
                    {
                        data.startingParty.RemoveAt(index);
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
            ActorData[] data = Resources.LoadAll<ActorData>(PathDatabase.ActorRelativeDataPath);
            ActorList = data.Select(x => x.actorName).ToList();

            set = true;
        }
    }

    #endregion

}
