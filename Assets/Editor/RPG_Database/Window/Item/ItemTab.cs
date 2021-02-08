using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Remorse.Tools.RPGDatabase.Utility;

namespace Remorse.Tools.RPGDatabase.Window
{
    public class ItemTab : BaseTab
    {
        //Having list of all items exist in data.
        public List<ItemData> item = new List<ItemData>();
        public List<EffectData> effects = new List<EffectData>();

        //List of names. Why you ask? because selectionGrid require
        //array of string, which we cannot obtain in itemData.
        //I hope later got better solution about this to not do
        //a double List for this kind of thing.
        List<string> itemDisplayName = new List<string>();
        List<string> effectDisplayName = new List<string>();

        //i don't know about this but i leave this to handle later.
        public static int effectIndex = 0;
        public static int effectIndexTemp = -1;

        //Classes
        public string[] itemTypeList =
        {
        "Regular Item",
        "Key Item",
        "Hidden Item A",
        "Hidden Item B",
    };

        public string[] itemScopeList =
        {
        "None",
        "1 Enemy",
        "All Enemies",
        "1 Random Enimies",
        "2 Random Enimies",
        "3 Random Enimies",
        "4 Random Enimies",
        "1 Ally",
        "All Allies",
        "1 Ally (Dead)",
        "The Allies (Dead)",
        "The User",
    };

        public string[] itemOccasion =
        {
        "Always",
        "Battle Screen",
        "Menu Screen",
        "Never",
    };

        public string[] itemHitType =
        {
        "Certain Hit",
        "Pyhsical Hit",
        "Magical Hit",
    };

        public string[] itemAnimation =
        {
        "Normal Attack",
        "None",
        "Hit Pyhsical",
        "Other... (Add More Manually)",
    };

        public string[] itemType =
        {
        "None",
        "HP Damage",
        "MP Damage",
        "HP Recover",
        "MP Recover",
        "HP Drain",
        "MP Drain",
    };

        public string[] itemElement =
        {
        "Normal Attack",
        "None",
        "Physical",
        "Fire",
        "Ice",
        "Thunder",
        "Water",
        "Earth",
        "Wind",
        "Light",
        "Darkness",
    };

        public string[] itemBool =
        {
        "Yes",
        "No",
    };


        //All GUIStyle variable initialization.
        GUIStyle tabStyle;
        GUIStyle columnStyle;
        GUIStyle itemStyle;

        //How many items in ChangeMaximum Func
        public int itemSize;
        public int itemSizeTemp;

        public static int[] effectSize;

        //i don't know about this but i leave this to handle later.
        public static int index = 0;
        int indexTemp = -1;

        //Scroll position. Is this necessary?
        Vector2 scrollPos = Vector2.zero;
        Vector2 equipmentScrollPos = Vector2.zero;
        Vector2 effectsScrollPos = Vector2.zero;

        Texture2D itemIcon;
        public void Init()
        {
            item.Clear();
            effects.Clear();
            effectIndex = 0;

            LoadGameData<ItemData>(ref itemSize, item, PathDatabase.ItemRelativeDataPath);

            effectSize = new int[itemSize];
            LoadGameData<EffectData>(ref effectSize[index], effects, PathDatabase.ItemEffectRelativeDataPath + (index + 1));
            LoadElementTypeList();

            //create folder for effectdata
            FolderCreator(itemSize, "Assets/Resources/Data/ItemData", "EffectData");

            if (effectSize[index] <= 0)
            {
                effectIndex = 0;
                ChangeMaximum<EffectData>(++effectSize[index], effects, PathDatabase.ItemEffectExplicitDataPath + (index + 1) + "/Effect_");
            }

            ClearNullScriptableObjects();
            ListReset();
        }
        public void OnRender(Rect position)
        {
            #region A Bit Explanation About Local Tab
            ///So there is 2 types of Tab,
            ///One is in Database that not included here.
            ///Second, there is 3 tab slicing in ItemTab itself.
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
            itemStyle = new GUIStyle(GUI.skin.box);
            itemStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

            #region Entry Of itemsTab GUILayout
            //Start drawing the whole ItemTab.
            GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the ItemTab? yes, this one.
            GUILayout.Box(" ", itemStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/3
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
            GUILayout.Box("Items", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

            //Scroll View
            #region ScrollView
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
            index = GUILayout.SelectionGrid(index, itemDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * itemSize));
            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed && index != indexTemp)
            {
                indexTemp = index;
                effectIndex = 0;
                effectIndexTemp = -1;

                ItemTabLoader(indexTemp);

                effects.Clear();
                LoadGameData<EffectData>(ref effectSize[index], effects, PathDatabase.ItemEffectRelativeDataPath + (index + 1));

                if (effectSize[index] <= 0)
                {
                    ChangeMaximum<EffectData>(++effectSize[index], effects, PathDatabase.ItemEffectExplicitDataPath + (index + 1) + "/Effect_");
                    effectIndexTemp = 0;
                }
                ClearNullScriptableObjects();

                ListReset();
                indexTemp = -1;
            }

            // Change Maximum field and button
            itemSizeTemp = EditorGUILayout.IntField(itemSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                itemSize = itemSizeTemp;
                index = indexTemp = 0;
                FolderCreator(itemSize, "Assets/Resources/Data/ItemData", "EffectData");
                ChangeMaximum<ItemData>(itemSize, item, PathDatabase.ItemExplicitDataPath);

                //new effectsize array length
                int[] tempArr = new int[effectSize.Length];
                for (int i = 0; i < effectSize.Length; i++)
                    tempArr[i] = effectSize[i];

                effectSize = new int[itemSize];

                //find the smallest between tempArr and itemSize
                int smallestValue = tempArr.Length < itemSize ? tempArr.Length : itemSize;

                for (int i = 0; i < smallestValue; i++)
                    effectSize[i] = tempArr[i];

                //Reload data
                LoadGameData<EffectData>(ref effectSize[index], effects, PathDatabase.ItemEffectRelativeDataPath + (index + 1));
                if (effectSize[index] <= 0)
                {
                    ChangeMaximum<EffectData>(++effectSize[index], effects, PathDatabase.ItemEffectExplicitDataPath + (index + 1) + "/Effect_");
                }

                ClearNullScriptableObjects();
                ListReset();
            }
            else if (itemSizeTemp <= 0)
            {
                itemSizeTemp = itemSize;
            }
            GUILayout.EndArea();
            #endregion

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
            if (itemSize > 0)
            {
                item[index].itemName = GUILayout.TextField(item[index].itemName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                itemDisplayName[index] = item[index].itemName;
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
            GUILayout.Box(itemIcon, GUILayout.Width(61), GUILayout.Height(61)); // Icon Box preview
            Color tempColorIcon = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Edit Icon", GUILayout.Height(20), GUILayout.Width(61))) // Icon changer Button
            {
                item[index].Icon = ImageChanger(
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
            if (itemSize > 0)
            {
                item[index].itemDescription = GUILayout.TextArea(item[index].itemDescription, GUILayout.Width(firstTabWidth + 53), GUILayout.Height(generalBox.height / 5 + 5));
            }
            else
            {
                GUILayout.TextArea("Null", GUILayout.Width(firstTabWidth + 53), GUILayout.Height(generalBox.height / 5 + 5));
            }
            #endregion
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            #region itemType 
            GUILayout.BeginVertical();
            GUILayout.Label("Item Type:"); // item Type class label
            if (itemSize > 0)
            {
                item[index].selecteditemTypeIndex = EditorGUILayout.Popup(item[index].selecteditemTypeIndex, itemTypeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
            }
            else
            {
                EditorGUILayout.Popup(0, itemTypeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
            }
            GUILayout.EndVertical();
            #endregion

            #region Price Consumable
            GUILayout.BeginVertical();
            GUILayout.Label("Price:"); // Price label
            if (itemSize > 0)
            { item[index].itemPrice = EditorGUILayout.IntField(item[index].itemPrice, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 2), GUILayout.Height(generalBox.height / 8 - 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Consumable:"); // Consumable class label
            if (itemSize > 0)
            {
                item[index].selectedConsumableIndex = EditorGUILayout.Popup(item[index].selectedConsumableIndex, itemBool, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 4 - 2));

            }
            else
            {
                EditorGUILayout.Popup(0, itemBool, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 4 - 2));
            }
            GUILayout.EndVertical();
            #endregion
            GUILayout.EndHorizontal();

            #region Scope Occasion

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Scope:"); // Scope class label
            if (itemSize > 0)
            {
                item[index].selecteditemScopeIndex = EditorGUILayout.Popup(item[index].selecteditemScopeIndex, itemScopeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
            }
            else
            {
                EditorGUILayout.Popup(0, itemScopeList, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2 - 15));
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Occasion:"); // Occasion class label
            if (itemSize > 0)
            {
                item[index].selecteditemOccasionIndex = EditorGUILayout.Popup(item[index].selecteditemOccasionIndex, itemOccasion, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2));
            }
            else
            {
                EditorGUILayout.Popup(0, itemOccasion, GUILayout.Height(generalBox.height / 8 - 15), GUILayout.Width(generalBox.width / 2));
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.EndVertical();
            #endregion
            GUILayout.EndArea();
            #endregion

            Rect invocationBox = new Rect(5, generalBox.height + 10, firstTabWidth + 60, position.height / 4 - 70);
            #region InvocationSettings
            GUILayout.BeginArea(invocationBox, tabStyle);
            #region Vertical
            GUILayout.BeginVertical();

            GUILayout.Label("Invocation", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            #region InitialLevel Success Repeat TPGain
            GUILayout.BeginVertical();
            GUILayout.Label("Initial Level:");
            if (itemSize > 0)
            { item[index].itemSpeed = EditorGUILayout.IntField(item[index].itemSpeed, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Success:");
            if (itemSize > 0)
            { item[index].itemSuccessLevel = EditorGUILayout.IntField(item[index].itemSuccessLevel, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Repeat:");
            if (itemSize > 0)
            { item[index].itemRepeat = EditorGUILayout.IntField(item[index].itemRepeat, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("TP Gain:");
            if (itemSize > 0)
            { item[index].itemTPGain = EditorGUILayout.IntField(item[index].itemTPGain, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
            else
            { EditorGUILayout.IntField(-1, GUILayout.Width(invocationBox.width / 4 - 5), GUILayout.Height(invocationBox.height / 8 + 9)); }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            #endregion

            #region HitType Animation
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label("Hit Type:"); // item Hit Type class label
            if (itemSize > 0)
            {
                item[index].selecteditemHitTypeIndex = EditorGUILayout.Popup(item[index].selecteditemHitTypeIndex, itemHitType, GUILayout.Height(invocationBox.height / 8 + 3), GUILayout.Width(invocationBox.width / 2 - 5));
            }
            else
            {
                EditorGUILayout.Popup(0, itemHitType, GUILayout.Height(invocationBox.height / 8 + 3), GUILayout.Width(invocationBox.width / 2 - 5));
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Animation:"); // item Animation label
            GUILayout.Label("**UnderWorking**", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();
            if (itemSize > 0)
            {
                item[index].selecteditemAnimationIndex = EditorGUILayout.Popup(item[index].selecteditemAnimationIndex, itemAnimation, GUILayout.Height(invocationBox.height / 8 + 3), GUILayout.Width(invocationBox.width / 2 - 5));
            }
            else
            {
                EditorGUILayout.Popup(0, itemAnimation, GUILayout.Height(invocationBox.height / 8 + 3), GUILayout.Width(invocationBox.width / 2 - 5));
            }
            GUILayout.EndVertical();


            GUILayout.EndHorizontal();
            #endregion
            GUILayout.EndVertical();
            #endregion

            GUILayout.EndArea();
            #endregion // End Of Invocation Settings


            GUILayout.EndArea();
            #endregion

            #region Tab 3/3
            GUILayout.BeginArea(new Rect(firstTabWidth * 2 + 77, 0, firstTabWidth + 25, tabHeight - 25), columnStyle);

            Rect damageBox = new Rect(5, 5, firstTabWidth + 15, position.height / 3 - 80);
            #region DamageBox
            GUILayout.BeginArea(damageBox, tabStyle);
            GUILayout.Label("Damage", EditorStyles.boldLabel);
            GUILayout.BeginVertical();
            #region Type Element Formula
            GUILayout.BeginHorizontal();

            #region Type Element
            #region Type
            GUILayout.BeginVertical();
            GUILayout.Label("Type:");
            if (itemSize > 0)
            {
                item[index].selectedTypeIndex = EditorGUILayout.Popup(item[index].selectedTypeIndex, itemType, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(damageBox.width / 2 - 5));
            }
            else
            {
                EditorGUILayout.Popup(0, itemType, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(damageBox.width / 2 - 5));
            }
            GUILayout.EndVertical();
            #endregion
            #region Element
            GUILayout.BeginVertical();
            GUILayout.Label("Element:");
            if (itemSize > 0)
            {
                item[index].selectedElementIndex = EditorGUILayout.Popup(item[index].selectedElementIndex, itemElement, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(damageBox.width / 2 - 5));
            }
            else
            {
                EditorGUILayout.Popup(0, itemElement, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(damageBox.width / 2 - 5));
            }
            GUILayout.EndVertical();
            #endregion

            GUILayout.EndHorizontal();
            #endregion

            #region Formula
            GUILayout.Label("Formula:");
            if (itemSize > 0)
            {
                item[index].itemFormula = GUILayout.TextField(item[index].itemFormula, GUILayout.Width(damageBox.width - 8), GUILayout.Height(damageBox.height / 4 - 17));
            }
            else
            {
                GUILayout.TextField("Null", GUILayout.Width(damageBox.width - 8), GUILayout.Height(damageBox.height / 4 - 17));
            }
            #endregion

            #endregion
            #region Variance CriticalHits
            GUILayout.BeginHorizontal();

            #region Variance
            GUILayout.BeginVertical();
            GUILayout.Label("Variance:");
            if (itemSize > 0)
            {
                item[index].itemVariance = EditorGUILayout.IntField(item[index].itemVariance, GUILayout.Width(.25f * (damageBox.width - 8)), GUILayout.Height(damageBox.height / 4 - 17));
            }
            else
            {
                EditorGUILayout.IntField(-1, GUILayout.Width(.25f * (damageBox.width - 8)), GUILayout.Height(damageBox.height / 4 - 17));
            }
            GUILayout.EndVertical();
            #endregion

            #region CriticalHits
            GUILayout.BeginVertical();
            GUILayout.Label("Critical Hits:");
            if (itemSize > 0)
            {
                item[index].selectedCriticalHits = EditorGUILayout.Popup(item[index].selectedCriticalHits, itemBool, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(.25f * (damageBox.width - 8)));
            }
            else
            {
                EditorGUILayout.Popup(0, itemBool, GUILayout.Height(damageBox.height / 3 - 35), GUILayout.Width(.25f * (damageBox.width - 8)));
            }
            GUILayout.EndVertical();

            GUILayout.Space(damageBox.width - (2 * .25f * (damageBox.width - 8)) - 20);
            #endregion

            GUILayout.EndHorizontal();
            #endregion
            GUILayout.EndVertical();

            GUILayout.EndArea();
            #endregion

            Rect effectsBox = new Rect(5, damageBox.height + 10, firstTabWidth + 15, position.height / 3);
            #region Effects
            ListReset();
            GUILayout.BeginArea(effectsBox, tabStyle);
            GUILayout.Label("Effects", EditorStyles.boldLabel);
            GUILayout.Space(2);

            #region Horizontal For Type And Content
            GUILayout.BeginHorizontal();
            GUILayout.Label(PadString("Type", string.Format("{0}", " Content")), GUILayout.Width(effectsBox.width));
            GUILayout.EndHorizontal();
            #endregion

            #region ScrollView
            effectsScrollPos = GUILayout.BeginScrollView(
                effectsScrollPos,
                false,
                true,
                GUILayout.Width(firstTabWidth + 5),
                GUILayout.Height(effectsBox.height * 0.725f)
                );

            GUI.changed = false;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;

            effectIndex = GUILayout.SelectionGrid(
                effectIndex,
                effectDisplayName.ToArray(),
                1,
                GUILayout.Width(firstTabWidth - 20),
                GUILayout.Height(position.height / 24 * effectSize[index])
            );

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            GUILayout.EndScrollView();
            #endregion

            if (GUI.changed)
            {
                if (effectIndex != effectIndexTemp)
                {
                    EffectWindow.ShowWindow(effects, effectIndex, effectSize[index], TabType.Item);
                    effectIndexTemp = effectIndex;
                }
            }

            if ((effects[effectSize[index] - 1].effectName != null && effects[effectSize[index] - 1].effectName != "") && effectSize[index] > 0)
            {
                effectIndex = 0;
                ChangeMaximum<EffectData>(++effectSize[index], effects, PathDatabase.ItemEffectExplicitDataPath + (index + 1) + "/Effect_");
            }

            Color tempColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Delete All Data", GUILayout.Width(effectsBox.width * .3f)))
            {
                if (EditorUtility.DisplayDialog("Delete All Effect Data", "Are you sure want to delete all Effect Data?", "Yes", "No"))
                {
                    effectIndex = 0;
                    effectSize[index] = 1;
                    ChangeMaximum<EffectData>(0, effects, PathDatabase.ItemEffectExplicitDataPath + (index + 1) + "/Effect_");
                    ChangeMaximum<EffectData>(1, effects, PathDatabase.ItemEffectExplicitDataPath + (index + 1) + "/Effect_");
                }
            }
            GUI.backgroundColor = tempColor;

            GUILayout.EndArea();
            #endregion

            Rect notesBox = new Rect(5, damageBox.height + effectsBox.height + 17, firstTabWidth + 15, position.height - (damageBox.height + effectsBox.height + 17) - 40);
            #region NoteBox
            GUILayout.BeginArea(notesBox, tabStyle);
            GUILayout.Space(2);
            GUILayout.Label("Notes", EditorStyles.boldLabel);
            GUILayout.Space(notesBox.height / 50);
            if (itemSize > 0)
            {
                item[index].notes = GUILayout.TextArea(item[index].notes, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.9f));
            }
            else
            {
                GUILayout.TextArea("Null", GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * .85f + 8));
            }
            GUILayout.EndArea();
            #endregion //End of notebox area

            GUILayout.EndArea();
            #endregion // End of Third Tab


            GUILayout.EndArea(); //End drawing the ItemTab
            #endregion

            EditorUtility.SetDirty(item[index]);
        }



        #region Features
        private void LoadElementTypeList()
        {
            TypeElementData[] typeElementData = Resources.LoadAll<TypeElementData>(PathDatabase.ElementRelativeDataPath);
            itemElement = new string[typeElementData.Length];
            for (int i = 0; i < itemElement.Length; i++)
            {
                itemElement[i] = typeElementData[i].dataName;
            }
        }
        public override void ItemTabLoader(int index)
        {
            Texture2D defTex = new Texture2D(256, 256);
            if (index != -1)
            {
                if (itemSize > 0)
                {
                    if (item[index].Icon == null)
                        itemIcon = defTex;
                    else
                        itemIcon = TextureToSprite(item[index].Icon);
                }
            }
        }

        private void ClearNullScriptableObjects()
        {
            bool availableNull = true;
            while (availableNull)
            {
                availableNull = false;
                for (int i = 0; i < effectSize[index] - 1; i++)
                {
                    if (effects[i].effectName == "" || effects[i].effectName == null)
                    {
                        availableNull = true;
                        for (int j = i; j < effectSize[index] - 1; j++)
                        {
                            effects[j] = effects[j + 1];
                        }
                        effectIndex = 0;
                        ChangeMaximum<EffectData>(--effectSize[index], effects, PathDatabase.ItemEffectExplicitDataPath);
                        i--;
                    }
                }
            }
        }

        ///<summary>
        ///Clears out the displayName list and add it with new value
        ///</summary>
        private void ListReset()
        {
            itemDisplayName.Clear();
            for (int i = 0; i < itemSize; i++)
            {
                itemDisplayName.Add(item[i].itemName);
            }

            //Effect Reset
            effectDisplayName.Clear();
            for (int i = 0; i < effectSize[index]; i++)
            {
                effectDisplayName.Add(effects[i].effectName);
            }
        }

        #endregion
    }
}