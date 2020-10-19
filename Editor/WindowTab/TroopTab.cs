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
    //array of string, which we cannot obtain in TroopData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> troopDisplayName = new List<string>();

    //Unique List
    List<string> troopAvailableList = new List<string>
    (
        new string[]
        {
            "Bat",
            "Slime",
            "Orc",
            "Minotaur",
        }
    );

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
    Vector2 scrollAddedListPos = Vector2.zero;
    Vector2 scrollAvailableTroopListPos = Vector2.zero;

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
        ///Second, there is 3 tab slicing in TroopTab itself.
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
        //Start drawing the whole TroopTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the TroopTab? yes, this one.
            GUILayout.Box(" ", troopStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/2

            //First Tab of two
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

            #region Tab 2/2
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, tabWidth - firstTabWidth - 15, tabHeight - 25), columnStyle);

                Rect generalBox = new Rect(5, 5, .7f * (tabWidth - firstTabWidth - 15), position.height / 4 + 125);
                    #region GeneralSettings
                    GUILayout.BeginArea(generalBox, tabStyle);
                        GUILayout.Label("General Settings", EditorStyles.boldLabel);
                        GUILayout.BeginVertical();
                            GUILayout.Label("Name:");
                            if (troopSize > 0)
                            {
                                troop[index].troopName = GUILayout.TextField(troop[index].troopName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                troopDisplayName[index] = troop[index].troopName;
                            }
                            else
                            {
                                GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                            }
                        GUILayout.EndVertical();

                        GUILayout.BeginHorizontal();
                            GUILayout.BeginVertical();

                                GUILayout.BeginArea(new Rect(5, 45+generalBox.height/8, 25 + (firstTabWidth * .4f), generalBox.height * .725f), troopStyle);
                                #region ScrollView
                                scrollAddedListPos = GUILayout.BeginScrollView(scrollAddedListPos, false, true, GUILayout.Width(20 + (firstTabWidth * .4f)), GUILayout.Height(generalBox.height * .725f));
                                troop[index].indexAddedList = GUILayout.SelectionGrid(troop[index].indexAddedList, troop[index].troopAddedList.ToArray(), 1, GUILayout.Width(firstTabWidth * .4f), GUILayout.Height(generalBox.height * .72f / 10 * troop[index].troopAddedList.Count));
                                GUILayout.EndScrollView();
                                #endregion

                                //Happen everytime selection grid is updated
                                if (GUI.changed && troop[index].indexAddedList != troop[index].indexAddedListTemp)
                                {
                                    troop[index].indexAddedListTemp = troop[index].indexAddedList;
                                    troop[index].indexAddedListTemp = -1;
                                }
                            GUILayout.EndArea();

                            GUILayout.Space(generalBox.width * .30f);
                            GUILayout.EndVertical();
                            GUILayout.BeginVertical();
                                GUILayout.Space(generalBox.height * .20f);
                                if(GUILayout.Button("< Add",GUILayout.Width(firstTabWidth * .4f),GUILayout.Height(position.height * .3f / 8)))
                                {
                                    troop[index].troopAddedList.Add(troopAvailableList[troop[index].indexAvailableList]);
                                }
                                if (GUILayout.Button("Remove >", GUILayout.Width(firstTabWidth * .4f), GUILayout.Height(position.height * .3f / 8)))
                                {
                                    troop[index].troopAddedList.RemoveAt(troop[index].indexAddedList);
                                }
                                if (GUILayout.Button("Clear", GUILayout.Width(firstTabWidth * .4f), GUILayout.Height(position.height * .3f / 8)))
                                {
                                    troop[index].troopAddedList.Clear();
                                }
                            GUILayout.EndVertical();

                            GUILayout.BeginArea(new Rect(generalBox.width - (30 + (firstTabWidth * .4f)), 45 + generalBox.height / 8, 25 + (firstTabWidth * .4f), generalBox.height * .725f), troopStyle);
                                #region ScrollView
                                scrollAvailableTroopListPos = GUILayout.BeginScrollView(scrollAvailableTroopListPos, false, true, GUILayout.Width(20 + (firstTabWidth * .4f)), GUILayout.Height(generalBox.height * .725f));
                                troop[index].indexAvailableList = GUILayout.SelectionGrid(troop[index].indexAvailableList, troopAvailableList.ToArray(), 1, GUILayout.Width(firstTabWidth * .4f), GUILayout.Height(generalBox.height * .72f / 10 * troopAvailableList.Count));
                                GUILayout.EndScrollView();
                                #endregion

                                //Happen everytime selection grid is updated
                                if (GUI.changed && troop[index].indexAvailableList != troop[index].indexAvailableListTemp)
                                {
                                    troop[index].indexAvailableListTemp = troop[index].indexAvailableList;
                                    troop[index].indexAvailableListTemp = -1;
                                }
                            GUILayout.EndArea();
                        GUILayout.EndHorizontal();

                    GUILayout.EndArea();
                    #endregion

                Rect battleEvent = new Rect(5, generalBox.height + 10, tabWidth - firstTabWidth - 25, position.height - generalBox.height - 50);
                    #region BattleEvent
                    GUILayout.BeginArea(battleEvent, tabStyle);
                    GUILayout.Label("Battle Event", EditorStyles.boldLabel);


                    GUILayout.EndArea();
                    #endregion


        GUILayout.EndArea();
            #endregion
        GUILayout.EndArea(); //End drawing the whole TroopTab
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