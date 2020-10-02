using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class ItemTab : BaseTab
{
    //Having list of all skills exist in data.
    public List<ItemData> item = new List<ItemData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in SkillData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> itemDisplayName = new List<string>();

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

        #region Entry Of SkillsTab GUILayout
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
            GUILayout.EndArea();
            //End drawing the ItemTab

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
                
            }
        }
    }

    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from skillSize</param>

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