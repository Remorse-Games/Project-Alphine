using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Remorse.Editor.RPGDatabase.Utility;

namespace Remorse.Editor.RPGDatabase.Window
{
    public class TermTab : BaseTab
    {
        public TermData term;

        public int termSize;
        public int index = 0;

        //All GUIStyle variable initialization.
        GUIStyle tabStyle;
        GUIStyle columnStyle;
        GUIStyle enemyStyle;

        //Scroll position. Is this necessary
        Vector2 traitsScrollPos = Vector2.zero;

        public void Init()
        {
            term = Resources.Load<TermData>("Data/TermData/TermData");
            if (term == null)
            {
                ScriptableObject newTermData = ScriptableObject.CreateInstance<TermData>();
                AssetDatabase.CreateAsset(newTermData, "Assets/Resources/Data/TermData/TermData.asset");
                AssetDatabase.SaveAssets();
                term = Resources.Load<TermData>("Data/TermData/TermData");

            }
        }

        public void OnRender(Rect position)
        {
            #region A Bit Explanation About Local Tab
            ///So there is 2 types of Tab,
            ///One is in Database that not included here.
            ///Second, there is 3 tab slicing in enemysTab itself.
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
            enemyStyle = new GUIStyle(GUI.skin.box);
            enemyStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

            #region Entry Of enemysTab GUILayout
            //Start drawing the whole enemyTab.
            GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the enemysTab? yes, this one.
            GUILayout.Box(" ", enemyStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/2
            Rect firstTab = new Rect(0, 0, tabWidth * .75f, tabHeight - 18);
            GUILayout.BeginArea(firstTab, columnStyle);
            #region BasicStatus
            Rect basicStatuses = new Rect(5, 5, firstTab.width * .49f, firstTab.height * .43f);
            GUILayout.BeginArea(basicStatuses, tabStyle);
            GUILayout.Label("Basic Statuses", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            float fieldWidth = basicStatuses.width * .48f;
            float fieldHeight = basicStatuses.height * .11f;
            GUILayout.BeginVertical();
            GUILayout.Label("Level:");
            term.termLevel = GUILayout.TextField(term.termLevel, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("HP:");
            term.termHP = GUILayout.TextField(term.termHP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("MP:");
            term.termMP = GUILayout.TextField(term.termMP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("TP:");
            term.termTP = GUILayout.TextField(term.termTP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("EXP:");
            term.termEXP = GUILayout.TextField(term.termEXP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Level (abbr.):");
            term.termLevelabbr = GUILayout.TextField(term.termLevelabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("HP (abbr.):");
            term.termHPabbr = GUILayout.TextField(term.termHPabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("MP (abbr.):");
            term.termMPabbr = GUILayout.TextField(term.termMPabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("TP (abbr.):");
            term.termTPabbr = GUILayout.TextField(term.termTPabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("EXP (abbr.):");
            term.termEXPabbr = GUILayout.TextField(term.termEXPabbr, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
            #endregion

            #region Parameters
            Rect parameterBox = new Rect(basicStatuses.width + 10, 5, firstTab.width * .49f, basicStatuses.height);
            GUILayout.BeginArea(parameterBox, tabStyle);
            GUILayout.Label("Parameter", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Max. HP:");
            term.termMaxHP = GUILayout.TextField(term.termMaxHP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("Attack:");
            term.termAttack = GUILayout.TextField(term.termAttack, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("M. Attack:");
            term.termMAttack = GUILayout.TextField(term.termMAttack, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("Agility:");
            term.termAgility = GUILayout.TextField(term.termAgility, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("Hit Rate:");
            term.termHitRate = GUILayout.TextField(term.termHitRate, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Max. MP:");
            term.termMaxMP = GUILayout.TextField(term.termMaxMP, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("Defense:");
            term.termDefense = GUILayout.TextField(term.termDefense, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("M. Defense:");
            term.termMDefense = GUILayout.TextField(term.termMDefense, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("Luck:");
            term.termLuck = GUILayout.TextField(term.termLuck, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));

            GUILayout.Label("Evasion Rate:");
            term.termEvasionRate = GUILayout.TextField(term.termEvasionRate, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            #endregion

            #region Commands
            Rect commandBox = new Rect(5, parameterBox.height + 10, firstTab.width - 14, firstTab.height * .55f);
            GUILayout.BeginArea(commandBox, columnStyle);
            float secondFieldWidth = commandBox.width * .24f;
            float secondFieldHeight = commandBox.height * .09f;
            GUILayout.Label("Commands", EditorStyles.boldLabel);

            #region TopArea
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Fight:");
            term.commandFight = GUILayout.TextField(term.commandFight, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Item:");
            term.commandItem = GUILayout.TextField(term.commandItem, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Formation:");
            term.commandFormation = GUILayout.TextField(term.commandFormation, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Escape:");
            term.commandEscape = GUILayout.TextField(term.commandEscape, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Skill:");
            term.commandSkill = GUILayout.TextField(term.commandSkill, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Options:");
            term.commandOption = GUILayout.TextField(term.commandOption, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Attack:");
            term.commandAttack = GUILayout.TextField(term.commandAttack, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Equip:");
            term.commandEquip = GUILayout.TextField(term.commandEquip, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Save:");
            term.commandSave = GUILayout.TextField(term.commandSave, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Guard:");
            term.commandGuard = GUILayout.TextField(term.commandGuard, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Status:");
            term.commandStatus = GUILayout.TextField(term.commandStatus, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Game End:");
            term.commandGameEnd = GUILayout.TextField(term.commandGameEnd, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            #endregion

            DrawUILine(Color.black, 2, 10);

            #region BottomArea
            GUILayout.BeginHorizontal();


            GUILayout.BeginVertical();
            GUILayout.Label("Weapon:");
            term.commandWeapon = GUILayout.TextField(term.commandWeapon, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Optimize:");
            term.commandOptimize = GUILayout.TextField(term.commandOptimize, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("New Game:");
            term.commandNewGame = GUILayout.TextField(term.commandNewGame, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Armor:");
            term.commandArmor = GUILayout.TextField(term.commandArmor, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Clear:");
            term.commandClear = GUILayout.TextField(term.commandClear, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Continue:");
            term.commandContinue = GUILayout.TextField(term.commandContinue, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Key Item:");
            term.commandKeyItem = GUILayout.TextField(term.commandKeyItem, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Buy:");
            term.commandBuy = GUILayout.TextField(term.commandBuy, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("To Title:");
            term.commandToTitle = GUILayout.TextField(term.commandToTitle, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Equip:");
            term.commandEquip2 = GUILayout.TextField(term.commandEquip2, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Sell:");
            term.commandSell = GUILayout.TextField(term.commandSell, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));

            GUILayout.Label("Cancel:");
            term.commandCancel = GUILayout.TextField(term.commandCancel, GUILayout.Width(secondFieldWidth), GUILayout.Height(secondFieldHeight));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            #endregion

            GUILayout.EndArea();
            #endregion

            GUILayout.EndArea();
            #endregion

            #region Tab 2/2
            Rect messageBox = new Rect(firstTab.width + 5, 5, tabWidth * .24f, tabHeight - 23.5f);
            #region Traits
            GUILayout.BeginArea(messageBox, tabStyle);
            GUILayout.Space(2);
            GUILayout.Label("Messages", EditorStyles.boldLabel);
            GUILayout.Space(messageBox.height / 30);
            #region Horizontal For Type And Content
            GUILayout.BeginHorizontal();
            GUILayout.Label("Type", GUILayout.Width(messageBox.width * 3 / 8));
            GUILayout.Label("Text", GUILayout.Width(messageBox.width * 5 / 8));
            GUILayout.EndHorizontal();
            #endregion
            #region ScrollView
            traitsScrollPos = GUILayout.BeginScrollView(
                traitsScrollPos,
                false,
                true,
                GUILayout.Width(messageBox.width - 7),
                GUILayout.Height(messageBox.height * .9f)
                );
            GUILayout.EndScrollView();
            #endregion
            GUILayout.EndArea();
            #endregion //End of TraitboxArea


            #endregion

            GUILayout.EndArea();
            #endregion
            EditorUtility.SetDirty(term);
        }

        #region Features
        public override void ItemTabLoader(int index)
        {
            Debug.Log(index + "index");
            Texture2D defTex = new Texture2D(256, 256);
            if (index != -1)
            {

            }
        }

        #endregion
    }
}