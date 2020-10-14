using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
public class StateTab : BaseTab
{

    //Having list of all states exist in data.
    public List<StateData> state = new List<StateData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in stateData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> stateDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle stateStyle;

    //How many state in ChangeMaximum Func
    public int stateSize;
    public int stateSizeTemp;

    //dataPath where the game data will be saved as a .assets
    private string dataPath = "Assets/Resources/Data/StateData/State_";
    private string _dataPath = "Data/StateData";

    //i don't know about this but i leave this to handle later.
    int index = 0;
    int indexTemp = -1;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;

    //Icon
    Texture2D stateIcon;

    public void Init()
    {
        LoadGameData<StateData>(ref stateSize, state, _dataPath);
        ListReset();
    }
    public void OnRender(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in stateTab itself.
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
        stateStyle = new GUIStyle(GUI.skin.box);
        stateStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

        #region Entry Of StateTab GUILayout
        //Start drawing the whole stateTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the stateTab? yes, this one.
            GUILayout.Box(" ", stateStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

        
            #region Tab 1/2

            //First Tab of two
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
                GUILayout.Box("States", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

                //Scroll View
                #region ScrollView
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
                index = GUILayout.SelectionGrid(index, stateDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * stateSize));
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
                stateSizeTemp = EditorGUILayout.IntField(stateSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
                {
                    stateSize = stateSizeTemp;
                    ChangeMaximum<StateData>(stateSize, state, dataPath);
                    ListReset();
                }

            GUILayout.EndArea();
            #endregion // End Of First Tab

        GUILayout.EndArea(); //End drawing the whole StateTab
        #endregion 
        EditorUtility.SetDirty(state[index]);

    }

    #region Features
    public override void ItemTabLoader(int index)
    {
        Debug.Log(index + "index");
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if (stateSize > 0)
            {
                if (state[index].icon == null)
                    stateIcon = defTex;
                else
                    stateIcon = TextureToSprite(state[index].icon);

            }
        }
    }

    ///<summary>
    ///Clears out the displayName list and add it with new value
    ///</summary>
    private void ListReset()
    {
        stateDisplayName.Clear();
        for (int i = 0; i < stateSize; i++)
        {
            stateDisplayName.Add(state[i].stateName);
        }
    }

    #endregion
}