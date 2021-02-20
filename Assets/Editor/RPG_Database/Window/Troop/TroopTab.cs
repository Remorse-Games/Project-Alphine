using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

using Remorse.Tools.RPGDatabase.Utility;

namespace Remorse.Tools.RPGDatabase.Window
{

    public class TroopTab : BaseTab
    {

        //Having list of all troops exist in data.
        public List<TroopData> troop = new List<TroopData>();

        // list of all battle event data
        public List<BattleEventData> battleEvents = new List<BattleEventData>();

        //List of names. Why you ask? because selectionGrid require
        //array of string, which we cannot obtain in TroopData.
        //I hope later got better solution about this to not do
        //a double List for this kind of thing.
        List<string> troopDisplayName = new List<string>();

        //Unique List
        List<string> troopAvailableList = new List<string>();

        //page index string list
        List<string> pageIndexList = new List<string>()
    {
        "1",
        "2",
        "3"
    };

        List<string> spanList = new List<string>()
    {
        "Battle",
        "Turn",
        "Moment"
    };

        List<string> eventCommandList = new List<string>()
    {
        "- "
    };

        //All GUIStyle variable initialization.
        GUIStyle tabStyle;
        GUIStyle columnStyle;
        GUIStyle troopStyle;

        //How many troop in ChangeMaximum Func
        public int troopSize;

        //How many battle event data
        public int[] battleEventSize;

        //i don't know about this but i leave this to handle later.
        int index = 0;
        int indexTemp = -1;

        int battleEventIndex = 0;
        int eventCommandIndex = -1;

        //Scroll position. Is this necessary?
        Vector2 scrollPos = Vector2.zero;
        Vector2 scrollAddedListPos = Vector2.zero;
        Vector2 scrollAvailableTroopListPos = Vector2.zero;
        Vector2 scrollPageIndex = Vector2.zero;
        Vector2 scrollPage = Vector2.zero;

        Texture2D background;
        public int troopSizeTemp;

        private BattleEventData CopyData = null;

        public void Init()
        {
            troop.Clear();
            battleEvents.Clear();

            index = 0;
            battleEventIndex = 0;

            LoadGameData<TroopData>(ref troopSize, troop, PathDatabase.TroopRelativeDataPath);

            battleEventSize = new int[troopSize];
            LoadGameData<BattleEventData>(ref battleEventSize[index], battleEvents, PathDatabase.BattleEventRelativeDataPath + (index + 1));

            FolderCreator(troopSize, "Assets/Resources/Data/TroopData", "BattleEventData");

            if (battleEventSize[index] <= 0)
            {
                battleEventIndex = 0;
                ChangeMaximum<BattleEventData>(++battleEventSize[index], battleEvents, PathDatabase.BattleEventExplicitDataPath + (index + 1) + "/BattleEvent_");
            }

            LoadEnemyList();

            ClearNullScriptableObjects();
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
                battleEventIndex = 0;

                // load battle events data
                battleEvents.Clear();
                LoadGameData<BattleEventData>(ref battleEventSize[index], battleEvents, PathDatabase.BattleEventRelativeDataPath + (index + 1));

                //check if battle event folder empty
                if (battleEventSize[index] <= 0)
                {
                    ChangeMaximum<BattleEventData>(++battleEventSize[index], battleEvents, PathDatabase.BattleEventExplicitDataPath + (index + 1) + "/BattleEvent_");
                    battleEventIndex = 0;
                }

                ClearNullScriptableObjects();

                ItemTabLoader(indexTemp);

                ListReset();
                indexTemp = -1;
            }

            // Change Maximum field and button
            troopSizeTemp = EditorGUILayoutExt.IntField(0, 999, troopSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                if (troopSizeTemp < troopSize)
                {
                    index = troopSizeTemp - 1;
                    GUI.changed = true;
                }

                troopSize = troopSizeTemp;
                battleEventIndex = 0;

                FolderCreator(troopSize, "Assets/Resources/Data/TroopData", "BattleEventData");

                ChangeMaximum<TroopData>(troopSize, troop, PathDatabase.TroopExplicitDataPath);

                int[] tempArr = new int[battleEventSize.Length];
                for (int i = 0; i < battleEventSize.Length; i++)
                    tempArr[i] = battleEventSize[i];

                battleEventSize = new int[troopSize];

                int smallestValue = tempArr.Length < troopSize ? tempArr.Length : troopSize;

                for (int i = 0; i < smallestValue; i++)
                    battleEventSize[i] = tempArr[i];

                // Reload data and check SO
                LoadGameData<BattleEventData>(ref battleEventSize[index], battleEvents, PathDatabase.BattleEventRelativeDataPath + (index + 1));
                if (battleEventSize[index] <= 0)
                {
                    ChangeMaximum<BattleEventData>(++battleEventSize[index], battleEvents, PathDatabase.BattleEventExplicitDataPath + (index + 1) + "/BattleEvent_");
                }

                ClearNullScriptableObjects();
                ListReset();
            }
            else if (troopSizeTemp <= 0)
            {
                troopSizeTemp = troopSize;
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
                troop[index].troopName = GUILayout.TextField(troop[index].troopName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 12));
                troopDisplayName[index] = troop[index].troopName;
            }
            else
            {
                GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 12));
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            GUILayout.BeginArea(new Rect(5, 45 + generalBox.height / 9, 25 + (firstTabWidth * .4f), generalBox.height * .725f), troopStyle);
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

            EditorGUI.BeginDisabledGroup(troopAvailableList.Count <= 0);

            if (GUILayout.Button("< Add", GUILayout.Width(firstTabWidth * .4f), GUILayout.Height(position.height * .3f / 8)))
            {
                troop[index].troopAddedList.Add(troopAvailableList[troop[index].indexAvailableList]);
            }

            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(troop[index].troopAddedList.Count <= 0);

            Color tempColorRemove = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Remove >", GUILayout.Width(firstTabWidth * .4f), GUILayout.Height(position.height * .3f / 8)))
            {
                troop[index].troopAddedList.RemoveAt(troop[index].indexAddedList);
            }
            GUI.backgroundColor = tempColorRemove;

            Color tempColorClear = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Clear", GUILayout.Width(firstTabWidth * .4f), GUILayout.Height(position.height * .3f / 8)))
            {
                troop[index].troopAddedList.Clear();
            }
            GUI.backgroundColor = tempColorClear;
            EditorGUI.EndDisabledGroup();

            GUILayout.EndVertical();

            GUILayout.BeginArea(new Rect(generalBox.width - (30 + (firstTabWidth * .4f)), 45 + generalBox.height / 9, 25 + (firstTabWidth * .4f), generalBox.height * .725f), troopStyle);
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

            Rect battleEventRect = new Rect(5, generalBox.height + 10, tabWidth - firstTabWidth - 25, position.height - generalBox.height - 50);
            #region BattleEvent
            GUILayout.BeginArea(battleEventRect, tabStyle);
            GUILayout.Label("Battle Event", EditorStyles.boldLabel);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            #region Buttons

            GUILayout.BeginVertical(GUILayout.Width(100));
            GUILayout.Space(40);

            GUIStyle button = new GUIStyle(GUI.skin.button);
            button.margin = new RectOffset(20, 20, 5, 5);

            if (GUILayout.Button("New Event Page", button, GUILayout.Height(50)))
            {
                ChangeMaximum<BattleEventData>(++battleEventSize[index], battleEvents, PathDatabase.BattleEventExplicitDataPath + (index + 1) + "/BattleEvent_");
                battleEvents.Clear();
                LoadGameData<BattleEventData>(ref battleEventSize[index], battleEvents, PathDatabase.BattleEventRelativeDataPath + (index + 1));

                ListReset();
            }

            Color tempColorCopy = GUI.backgroundColor;
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Copy Event Page", button, GUILayout.Height(50)))
            {
                CopyData = battleEvents[battleEventIndex];
            }
            GUI.backgroundColor = tempColorCopy;
            EditorGUI.BeginDisabledGroup(CopyData == null);

            if (GUILayout.Button("Paste Event Page", button, GUILayout.Height(50)))
            {
                PasteData();
                battleEvents.Clear();
                LoadGameData<BattleEventData>(ref battleEventSize[index], battleEvents, PathDatabase.BattleEventRelativeDataPath + (index + 1));

                ListReset();
            }

            EditorGUI.EndDisabledGroup();

            Color tempColor2 = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;

            EditorGUI.BeginDisabledGroup(battleEventSize[index] <= 1);

            if (GUILayout.Button("Delete Event Page", button, GUILayout.Height(50)))
            {
                DeleteEventData();
                battleEvents.Clear();
                LoadGameData<BattleEventData>(ref battleEventSize[index], battleEvents, PathDatabase.BattleEventRelativeDataPath + (index + 1));

                ListReset();
            }
            EditorGUI.EndDisabledGroup();

            GUI.backgroundColor = tempColor2;

            Color tempColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Clear Event Page", button, GUILayout.Height(50)))
            {
                battleEvents[battleEventIndex].condition = "Don't Run";
                battleEvents[battleEventIndex].span = 0;
                battleEvents[battleEventIndex].eventCommand.Clear();

                ClearNullScriptableObjects();
                ListReset();
            }
            GUI.backgroundColor = tempColor;

            GUILayout.EndVertical();

            #endregion

            #region Event Page

            #region Front-end

            GUILayout.BeginVertical();

            #region page index

            scrollPageIndex = GUILayout.BeginScrollView(scrollPageIndex, false, false, GUILayout.Height(40));
            battleEventIndex = GUILayout.SelectionGrid(
                battleEventIndex,
                pageIndexList.ToArray(),
                pageIndexList.Count,
                GUILayout.Width(position.width / 30 * pageIndexList.Count)
            );

            GUILayout.EndScrollView();

            #endregion

            #region pages

            #region Header

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.margin = new RectOffset(0, 20, 2, 2);

            GUI.skin.button.alignment = TextAnchor.MiddleLeft;

            GUILayout.Label("Condition: ", labelStyle, GUILayout.Width(70));
            if (GUILayout.Button(battleEvents[battleEventIndex].condition))
            {
                // TODO: Open Condition Window
            }

            GUILayout.Label("Span: ", labelStyle, GUILayout.Width(70));
            battleEvents[battleEventIndex].span = EditorGUILayout.Popup(battleEvents[battleEventIndex].span, spanList.ToArray(), GUILayout.Width(100));

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;


            GUILayout.EndHorizontal();

            #endregion

            #region Main Pages

            // init style and position
            GUIStyle pageStyle = new GUIStyle(GUI.skin.box);
            pageStyle.normal.background = CreateTexture(1, 1, Color.gray);

            Rect pageRect = new Rect(160, 100, tabWidth - firstTabWidth - 190, position.height - generalBox.height - 155);

            // pages
            GUILayout.BeginArea(pageRect, pageStyle);

            #region ScrollView

            scrollPage = GUILayout.BeginScrollView(scrollPage, false, true);

            GUI.skin.button.alignment = TextAnchor.MiddleLeft;

            eventCommandIndex = GUILayout.SelectionGrid(
                eventCommandIndex,
                eventCommandList.ToArray(),
                1,
                GUILayout.Height(position.height / 30 * eventCommandList.Count)
            );

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            GUILayout.EndScrollView();

            #endregion

            GUILayout.EndArea();

            #endregion

            #endregion

            GUILayout.EndVertical();

            #endregion

            #region Back-end



            #endregion

            #endregion

            GUILayout.EndHorizontal();

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

            pageIndexList.Clear();
            for (int i = 0; i < battleEventSize[index]; i++)
            {
                pageIndexList.Add((i + 1).ToString());
            }
        }

        private void ClearNullScriptableObjects()
        {
            bool availableNull = true;
            while (availableNull)
            {
                availableNull = false;
                for (int i = 0; i < battleEventSize[index] - 1; i++)
                {
                    if (battleEvents[i].condition == "Don't Run")
                    {
                        availableNull = true;
                        for (int j = i; j < battleEventSize[index] - 1; j++)
                        {
                            battleEvents[j].condition = battleEvents[j + 1].condition;
                            battleEvents[j].span = battleEvents[j + 1].span;
                            battleEvents[j].eventCommand = battleEvents[j + 1].eventCommand;
                        }
                        ChangeMaximum<BattleEventData>(--battleEventSize[index], battleEvents, PathDatabase.BattleEventExplicitDataPath + (index + 1) + "/BattleEvent_");
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
                if (troopSize > 0)
                {
                    if (troop[index].background == null)
                        background = defTex;
                    else
                        background = TextureToSprite(troop[index].background);

                }
            }
        }

        private void LoadEnemyList()
        {
            troopAvailableList.Clear();

            EnemyData[] enemyData = Resources.LoadAll<EnemyData>(PathDatabase.EnemyRelativeDataPath);
            troopAvailableList = enemyData.Select(x => x.enemyName).ToList();
        }

        private void DeleteEventData()
        {
            for (int i = battleEventIndex; i < battleEventSize[index] - 1; i++)
            {
                battleEvents[i].condition = battleEvents[i + 1].condition;
                battleEvents[i].span = battleEvents[i + 1].span;
                battleEvents[i].eventCommand = battleEvents[i + 1].eventCommand;
            }

            if (battleEventIndex > 0) battleEventIndex--;

            ChangeMaximum<BattleEventData>(--battleEventSize[index], battleEvents, PathDatabase.BattleEventExplicitDataPath + (index + 1) + "/BattleEvent_");
            if (battleEventSize[index] <= 0)
            {
                ChangeMaximum<BattleEventData>(1, battleEvents, PathDatabase.BattleEventExplicitDataPath + (index + 1) + "/BattleEvent_");
                battleEventSize[index] = 1;
            }
        }

        private void PasteData()
        {
            ChangeMaximum<BattleEventData>(++battleEventSize[index], battleEvents, PathDatabase.BattleEventExplicitDataPath + (index + 1) + "/BattleEvent_");

            for (int i = battleEventIndex + 1; i < battleEventSize[index] - 1; i++)
            {
                battleEvents[i + 1].condition = battleEvents[i].condition;
                battleEvents[i + 1].span = battleEvents[i].span;
                battleEvents[i + 1].eventCommand = battleEvents[i].eventCommand;
            }

            battleEvents[battleEventIndex + 1].condition = CopyData.condition;
            battleEvents[battleEventIndex + 1].span = CopyData.span;
            battleEvents[battleEventIndex + 1].eventCommand = CopyData.eventCommand;
            CopyData = null;

            battleEventIndex++;
        }

        #endregion
    }
}