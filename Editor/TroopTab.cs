using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
public class TroopTab : BaseTab
{

    //Having list of all troops exist in data.
    public List<TroopData> troop = new List<TroopData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in troopData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> troopDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle troopStyle;

    //How many troop in ChangeMaximum Func
    public int troopSize;

    //dataPath where the game data will be saved as a .assets
    private string dataPath = "Assets/Resources/Data/TroopData/Troop_";
    private string _dataPath = "Data/TroopData";

    //i don't know about this but i leave this to handle later.
    int index = 0;
    int indexTemp = -1;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;

    Texture2D background;
    public int troopSizeTemp;

    public void Init()
    {
        LoadGameData<TroopData>(ref troopSize, troop, _dataPath);
        ListReset();
    }
    public void OnRender(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in EnemyTab itself.
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
        troopStyle = new GUIStyle(GUI.skin.box);
        troopStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

        #region Entry Of TroopTab GUILayout
        //Start drawing the whole EnemyTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the troopsTab? yes, this one.
            GUILayout.Box(" ", troopStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/3

            //First Tab of three
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
            GUILayout.Box("Troops", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

            //Scroll View
            #region ScrollView
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
            index = GUILayout.SelectionGrid(index, troopDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * troopSize));
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
            troopSizeTemp = EditorGUILayout.IntField(troopSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                troopSize = troopSizeTemp;
                ChangeMaximum<TroopData>(troopSize, troop, dataPath);
                ListReset();
            }

            GUILayout.EndArea();
            #endregion // End Of First Tab

        GUILayout.EndArea(); //End drawing the whole EnemyTab
        #endregion

        EditorUtility.SetDirty(troop[index]);
    }

    #region Features


    ///<summary>
    ///Clears out the displayName list and add it with new value
    ///</summary>
    private void ListReset()
    {
        troopDisplayName.Clear();
        for (int i = 0; i < troopSize; i++)
        {
            troopDisplayName.Add(troop[i].troopName);
        }
    }

    public override void ItemTabLoader(int index)
    {
        Debug.Log(index + "index");
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if (troopSize > 0)
            {
                if (troop[index].background == null)
                    background = defTex;
                else
                    background = TextureToSprite(troop[index].background);

            }
        }
    }

    #endregion
}