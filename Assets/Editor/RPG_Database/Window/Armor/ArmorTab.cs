using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Remorse.Tools.RPGDatabase.Utility;
using Remorse.Tools.RPGDatabase.Commons;

namespace Remorse.Tools.RPGDatabase.Window
{
    public class ArmorTab : BaseTab
    {
        //Having list of all armors exist in data.
        public List<ArmorData> armor = new List<ArmorData>();
        public List<TraitsData> traits = new List<TraitsData>();

        //List of names. Why you ask? because selectionGrid require
        //array of string, which we cannot obtain in armorData.
        //I hope later got better solution about this to not do
        //a double List for this kind of thing.
        List<string> armorDisplayName = new List<string>();
        List<string> traitDisplayName = new List<string>();

        //All GUIStyle variable initialization.
        GUIStyle tabStyle;
        GUIStyle columnStyle;
        GUIStyle armorStyle;

        //How many armor in ChangeMaximum Func
        public int armorSize;
        public int armorSizeTemp;
        public static int[] traitSize;

        public string[] armorTypeList;
        public string[] armorEquipmentList;

        //i don't know about this but i leave this to handle later.
        public static int index = 0;
        public static int traitIndex = 0;
        int indexTemp = -1;
        public static int traitIndexTemp = -1;

        //Scroll position. Is this necessary?
        Vector2 scrollPos = Vector2.zero;
        Vector2 equipmentScrollPos = Vector2.zero;
        Vector2 traitsScrollPos = Vector2.zero;

        //Image Area.
        Texture2D armorIcon;
        public void Init()
        {
            //Clears List
            armor.Clear();
            traits.Clear();

            //Resetting each index to 0, so that it won't have error (Index Out Of Range)
            index = 0;
            traitIndex = 0;

            //Load Every List needed in ArmorTab
            LoadGameData<ArmorData>(ref armorSize, armor, PathDatabase.ArmorTabRelativeDataPath);

            traitSize = new int[armorSize]; //Resets Trait Sizing
            LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.ArmorTraitRelativeDataPath + (index + 1));

            //Create Folder For TraitsData and its sum is based on actorSize value
            FolderCreator(armorSize, "Assets/Resources/Data/ArmorData", "TraitData");

            LoadArmorData();
            LoadEquipmentData();

            //Check if TraitsData_(index) is empty, if it is empty then create a SO named Trait_1
            if (traitSize[index] <= 0)
            {
                traitIndex = 0;
                ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ArmorTraitExplicitDataPath + (index + 1) + "/Trait_");
            }

            ClearNullScriptableObjects(); //Clear Trait SO without a value
            ListReset();
        }
        public void OnRender(Rect position)
        {
            #region A Bit Explanation About Local Tab
            ///So there is 2 types of Tab,
            ///One is in Database that not included here.
            ///Second, there is 3 tab slicing in armorsTab itself.
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
            armorStyle = new GUIStyle(GUI.skin.box);
            armorStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

            #region Entry Of armorsTab GUILayout
            //Start drawing the whole armorTab.
            GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the armorsTab? yes, this one.
            GUILayout.Box(" ", armorStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));


            #region Tab 1/3
            //First Tab of three
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));

            GUILayout.Box("Armors", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

            //Scroll View
            #region ScrollView
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
            index = GUILayout.SelectionGrid(index, armorDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * armorSize));
            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed && index != indexTemp)
            {
                indexTemp = index;
                traitIndex = 0;
                traitIndexTemp = -1;

                ItemTabLoader(indexTemp);

                //Load TraitsData
                traits.Clear();
                LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.ArmorTraitRelativeDataPath + (index + 1));
                //Check if TraitsData folder is empty
                if (traitSize[index] <= 0)
                {
                    ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ArmorTraitExplicitDataPath + (index + 1) + "/Trait_");
                    traitIndexTemp = 0;
                }
                ClearNullScriptableObjects();
                ListReset();
                indexTemp = -1;
            }

            // Change Maximum field and button
            armorSizeTemp = EditorGUILayout.IntField(armorSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                armorSize = armorSizeTemp;
                index = indexTemp = 0;
                FolderCreator(armorSize, "Assets/Resources/Data/ArmorData", "TraitData");
                ChangeMaximum<ArmorData>(armorSize, armor, PathDatabase.ArmorTabExplicitDataPath);

                //New TraitSize array length
                int[] tempArr = new int[traitSize.Length];
                for (int i = 0; i < traitSize.Length; i++)
                    tempArr[i] = traitSize[i];

                traitSize = new int[armorSize];

                #region FindSmallestBetween
                int smallestValue;
                if (tempArr.Length < armorSize) smallestValue = tempArr.Length;
                else smallestValue = armorSize;
                #endregion

                for (int i = 0; i < smallestValue; i++)
                    traitSize[i] = tempArr[i];

                //Reload Data and Check SO
                LoadGameData<TraitsData>(ref traitSize[index], traits, PathDatabase.ArmorTraitRelativeDataPath + (index + 1));
                if (traitSize[index] <= 0)
                {
                    ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ArmorTraitExplicitDataPath + (index + 1) + "/Trait_");
                }

                ClearNullScriptableObjects();
                ListReset();
            }
            else if (armorSizeTemp <= 0)
            {
                armorSizeTemp = armorSize;
            }
            GUILayout.EndArea();
            #endregion // End Of First Tab


            #region Tab 2/3
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 25), columnStyle);

            Rect generalBox = new Rect(5, 5, firstTabWidth + 60, position.height / 4 + 120);
            #region GeneralSettings
            GUILayout.BeginArea(generalBox, tabStyle); // Start of General Settings tab
            GUILayout.Label("General Settings", EditorStyles.boldLabel); // General Settings label
            #region Vertical
            GUILayout.BeginVertical();
            #region Horizontal
            GUILayout.BeginHorizontal();
            #region Name
            GUILayout.BeginVertical();
            GUILayout.Label("Name:"); // Name label
            if (armorSize > 0)
            {
                armor[index].armorName = GUILayout.TextField(armor[index].armorName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                armorDisplayName[index] = armor[index].armorName;
            }
            else
            {
                GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
            }
            GUILayout.EndVertical();
            #endregion
            #region Icon
            GUILayout.BeginArea(new Rect(generalBox.width / 2 - 3, generalBox.height * .05f + 5, firstTabWidth - 220, position.height / 2)); // Icon Area
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Icon:"); // Icon label
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Box(armorIcon, GUILayout.Width(61), GUILayout.Height(61)); // Icon Box preview
            Color tempColorIcon = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Edit Icon", GUILayout.Height(20), GUILayout.Width(61))) // Icon changer Button
            {
                armor[index].Icon = ImageChanger(
                index,
                "Choose Icon",
                "Assets/Resources/Image"
                );
                ItemTabLoader(index);
            }
            GUI.backgroundColor = tempColorIcon;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            #endregion
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(30);

            #region Description
            GUILayout.Label("Description:"); // Description label
            if (armorSize > 0)
            {
                armor[index].armorDescription = GUILayout.TextArea(armor[index].armorDescription, GUILayout.Width(firstTabWidth + 53), GUILayout.Height(generalBox.height / 5 + 5));
            }
            else
            {
                GUILayout.TextArea("Null", GUILayout.Width(firstTabWidth + 53), GUILayout.Height(generalBox.height / 5 + 5));
            }
            #endregion
            GUILayout.Space(5);

            #region Price ArmorType 
            GUILayout.BeginHorizontal();
            #region armorType
            GUILayout.BeginVertical();
            GUILayout.Label("Armor Type:"); // Armor Type class label
            if (armorSize > 0)
            {
                armor[index].selectedArmorTypeIndex = EditorGUILayout.Popup(armor[index].selectedArmorTypeIndex, armorTypeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
            }
            else
            {
                EditorGUILayout.Popup(0, armorTypeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
            }
            GUILayout.EndVertical();
            #endregion
            #region Price
            GUILayout.BeginVertical();
            GUILayout.Label("Price:"); // Price label
            if (armorSize > 0)
            {
                armor[index].armorPrice = EditorGUILayout.IntField(armor[index].armorPrice, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9));
            }
            else
            {
                EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9));
            }
            GUILayout.EndVertical();

            GUILayout.Space(generalBox.width / 4 - 2);
            GUILayout.EndHorizontal();
            #endregion
            #endregion

            #region EquipmentType
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Equipment Type:"); // Animation class label
            if (armorSize > 0)
            {
                armor[index].selectedArmorEquipmentIndex = EditorGUILayout.Popup(armor[index].selectedArmorEquipmentIndex, armorEquipmentList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
            }
            else
            {
                EditorGUILayout.Popup(0, armorEquipmentList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.EndVertical();
            #endregion
            GUILayout.EndArea(); // End of GeneralSettings Tab
            #endregion

            Rect parameterChangesBox = new Rect(5, generalBox.height + 10, firstTabWidth + 60, position.height / 4 - 65);
            #region ParameterChangesBox
            GUILayout.BeginArea(parameterChangesBox, tabStyle);
            #region Vertical
            GUILayout.BeginVertical();
            GUILayout.Label("Parameter Changes", EditorStyles.boldLabel);

            #region InitialLevel Success Repeat TPGain
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Attack:");
            if (armorSize > 0)
            { armor[index].armorAttack = EditorGUILayout.IntField(armor[index].armorAttack, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Defense:");
            if (armorSize > 0)
            { armor[index].armorDefense = EditorGUILayout.IntField(armor[index].armorDefense, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("M.Attack:");
            if (armorSize > 0)
            { armor[index].armorMAttack = EditorGUILayout.IntField(armor[index].armorMAttack, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("M.Defense:");
            if (armorSize > 0)
            { armor[index].armorMDefense = EditorGUILayout.IntField(armor[index].armorMDefense, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            #endregion

            #region Agility Luck MaxHP MaxMP
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Agility:");
            if (armorSize > 0)
            { armor[index].armorAgility = EditorGUILayout.IntField(armor[index].armorAgility, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Luck:");
            if (armorSize > 0)
            { armor[index].armorLuck = EditorGUILayout.IntField(armor[index].armorLuck, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Max HP:");
            if (armorSize > 0)
            { armor[index].armorMaxHP = EditorGUILayout.IntField(armor[index].armorMaxHP, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Max MP:");
            if (armorSize > 0)
            { armor[index].armorMaxMP = EditorGUILayout.IntField(armor[index].armorMaxMP, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(parameterChangesBox.width / 4 - 5), GUILayout.Height(parameterChangesBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            #endregion

            GUILayout.EndVertical();
            #endregion

            GUILayout.EndArea();
            #endregion // End Of ParameterChanges Settings

            GUILayout.EndArea();
            #endregion // End of Second Tab


            #region Tab 3/3
            //Third Column
            GUILayout.BeginArea(new Rect(position.width - (position.width - firstTabWidth * 2) + 77, 0, firstTabWidth + 25, tabHeight - 15), columnStyle);

            //Traits
            Rect traitsBox = new Rect(5, 5, firstTabWidth + 15, position.height * 5 / 8);
            #region Traits
            ListReset();
            GUILayout.BeginArea(traitsBox, tabStyle);
            GUILayout.Space(2);
            GUILayout.Label("Traits", EditorStyles.boldLabel);
            GUILayout.Space(5);
            #region Horizontal For Type And Content
            GUILayout.BeginHorizontal();
            GUILayout.Label(PadString("Type", string.Format("{0}", "  Content")), GUILayout.Width(traitsBox.width));
            GUILayout.EndHorizontal();
            #endregion
            #region ScrollView
            traitsScrollPos = GUILayout.BeginScrollView(
                traitsScrollPos,
                false,
                true,
                GUILayout.Width(firstTabWidth + 5),
                GUILayout.Height(traitsBox.height * 0.83f)
                );

            GUI.changed = false;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
            traitIndex = GUILayout.SelectionGrid(
                traitIndex,
                traitDisplayName.ToArray(),
                1,
                GUILayout.Width(firstTabWidth - 20),
                GUILayout.Height(position.height / 24 * traitSize[index]
                ));
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed)
            {
                if (traitIndex != traitIndexTemp)
                {
                    TraitWindow.ShowWindow(traits, traitIndex, traitSize[index], TabType.Armor);

                    traitIndexTemp = traitIndex;
                }
            }

            //Create Empty SO if there isn't any null SO left
            if ((traits[traitSize[index] - 1].traitName != null && traits[traitSize[index] - 1].traitName != "") && traitSize[index] > 0)
            {
                traitIndex = 0;
                ChangeMaximum<TraitsData>(++traitSize[index], traits, PathDatabase.ArmorTraitExplicitDataPath + (index + 1) + "/Trait_");
            }

            Color tempColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            //Delete All Data Button
            if (GUILayout.Button("Delete All Data", GUILayout.Width(traitsBox.width * .3f), GUILayout.Height(traitsBox.height * .055f)))
            {
                if (EditorUtility.DisplayDialog("Delete All Trait Data", "Are you sure want to delete all Trait Data?", "Yes", "No"))
                {
                    traitIndex = 0;
                    traitSize[index] = 1;
                    ChangeMaximum<TraitsData>(0, traits, PathDatabase.ArmorTraitExplicitDataPath + (index + 1) + "/Trait_");
                    ChangeMaximum<TraitsData>(1, traits, PathDatabase.ArmorTraitExplicitDataPath + (index + 1) + "/Trait_");
                }
            }
            GUI.backgroundColor = tempColor;
            GUILayout.EndArea();
            #endregion //End of TraitboxArea


            //Notes
            Rect notesBox = new Rect(5, traitsBox.height + 15, firstTabWidth + 15, position.height * 2.5f / 8);
            #region NoteBox
            GUILayout.BeginArea(notesBox, tabStyle);
            GUILayout.Space(2);
            GUILayout.Label("Notes", EditorStyles.boldLabel);
            GUILayout.Space(notesBox.height / 50);
            if (armorSize > 0)
            {
                armor[index].notes = GUILayout.TextArea(armor[index].notes, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.9f));
            }
            else
            {
                GUILayout.TextArea("Null", GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.85f));
            }
            GUILayout.EndArea();
            #endregion //End of notebox area

            GUILayout.EndArea();
            #endregion // End of third column

            GUILayout.EndArea(); // End drawing the whole ArmorTab
            #endregion
            EditorUtility.SetDirty(armor[index]);
        }

        #region Features
        ///<summary>
        ///Clears out the displayName list and add it with new value
        ///</summary>
        private void ListReset()
        {
            armorDisplayName.Clear();
            for (int i = 0; i < armorSize; i++)
            {
                armorDisplayName.Add(armor[i].armorName);
            }
            //Trait Reset
            traitDisplayName.Clear();
            for (int i = 0; i < traitSize[index]; i++)
            {
                traitDisplayName.Add(traits[i].traitName);
            }
        }

        private void LoadArmorData()
        {
            TypeArmorData[] armorData = Resources.LoadAll<TypeArmorData>(PathDatabase.ArmorRelativeDataPath);
            armorTypeList = new string[armorData.Length];
            for (int i = 0; i < armorTypeList.Length; i++)
            {
                armorTypeList[i] = armorData[i].dataName;
            }
        }
        private void LoadEquipmentData()
        {
            TypeEquipmentData[] equipmentData = Resources.LoadAll<TypeEquipmentData>(PathDatabase.EquipmentRelativeDataPath);
            armorEquipmentList = new string[equipmentData.Length];
            for (int i = 0; i < armorEquipmentList.Length; i++)
            {
                armorEquipmentList[i] = equipmentData[i].dataName;
            }
        }
        public override void ItemTabLoader(int index)
        {
            Texture2D defTex = new Texture2D(256, 256);
            if (index != -1)
            {
                if (armorSize > 0)
                {
                    if (armor[index].Icon == null)
                        armorIcon = defTex;
                    else
                        armorIcon = TextureToSprite(armor[index].Icon);
                }
            }
        }
        private void ClearNullScriptableObjects()
        {
            bool availableNull = true;
            while (availableNull)
            {
                availableNull = false;
                for (int i = 0; i < traitSize[index] - 1; i++)
                {
                    if (string.IsNullOrEmpty(traits[i].traitName))
                    {
                        availableNull = true;
                        for (int j = i; j < traitSize[index] - 1; j++)
                        {
                            traits[j].traitName = traits[j + 1].traitName;
                            traits[j].selectedTabToggle = traits[j + 1].selectedTabToggle;
                            traits[j].selectedTabIndex = traits[j + 1].selectedTabIndex;
                            traits[j].selectedArrayIndex = traits[j + 1].selectedArrayIndex;
                            traits[j].traitValue = traits[j + 1].traitValue;
                        }
                        ChangeMaximum<TraitsData>(--traitSize[index], traits, PathDatabase.ArmorTraitExplicitDataPath + (index + 1) + "/Trait_");
                        i--;
                    }
                }
            }
        }


        #endregion
    }
}
