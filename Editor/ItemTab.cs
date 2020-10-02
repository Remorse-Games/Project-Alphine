using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class ItemTab : BaseTab
{
    //Having list of all items exist in data.
    public List<ItemData> item = new List<ItemData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in itemData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> itemDisplayName = new List<string>();


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

    //i don't know about this but i leave this to handle later.
    int index = 0;
    int indexTemp = -1;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;
    Vector2 equipmentScrollPos = Vector2.zero;
    Vector2 effectsScrollPos = Vector2.zero;

    Texture2D itemIcon;

    public void Init(Rect position)
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
                        ItemTabLoader(indexTemp);
                        indexTemp = -1;
                    }

                    // Change Maximum field and button
                    itemSizeTemp = EditorGUILayout.IntField(itemSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                    if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
                    {
                        ChangeMaximumPrivate(itemSize);
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
                                                if (GUILayout.Button("Edit Icon", GUILayout.Height(20), GUILayout.Width(61))) // Icon changer Button
                                                {
                                                    item[index].Icon = ImageChanger(
                                                    index,
                                                    "Choose Icon",
                                                    "Assets/Resources/Image"
                                                    );
                                                    ItemTabLoader(index);
                                                }
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
                                            GUILayout.Label("Animation:"); // item Animation label
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



        GUILayout.EndArea(); //End drawing the ItemTab

        #endregion
    }



    #region Features
    public override void ItemTabLoader(int index)
    {
        Debug.Log(index + "index");
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

    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from itemSize</param>

    int counter = 0;
    private void ChangeMaximumPrivate(int size)
    {
        itemSize = itemSizeTemp;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        while (counter <= itemSize)
        {
            item.Add(ScriptableObject.CreateInstance<ItemData>());
            AssetDatabase.CreateAsset(item[counter], "Assets/Resources/Data/ItemData/Item_" + counter + ".asset");
            AssetDatabase.SaveAssets();
            itemDisplayName.Add(item[counter].itemName);
            counter++;
        }
        if (counter > itemSize)
        {
            item.RemoveRange(itemSize, item.Count - itemSize);
            itemDisplayName.RemoveRange(itemSize, itemDisplayName.Count - itemSize);
            for (int i = itemSize; i <= counter; i++)
            {
                AssetDatabase.DeleteAsset("Assets/Resources/Data/ItemData/Item_" + i + ".asset");
            }
            AssetDatabase.SaveAssets();
            counter = itemSize;
        }
    }

    #endregion
}