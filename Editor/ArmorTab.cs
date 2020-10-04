using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class ArmorTab : BaseTab
{
    //Having list of all armors exist in data.
    public List<ArmorData> armor = new List<ArmorData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in armorData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> armorDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle armorStyle;

    //How many armor in ChangeMaximum Func
    public int armorSize;
    public int armorSizeTemp;

    public void Init(Rect position)
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

        GUILayout.EndArea(); // End drawing the whole ArmorTab
        #endregion

    }

        #region Features

        /// <summary>
        /// Change Maximum function , when we change the size
        /// and click Change Maximum button in Editor, it will update
        /// and change the size while creating new data.
        /// </summary>
        /// <param name="size">get size from armorSize</param>

        int counter = 0;
    private void ChangeMaximumPrivate(int size)
    {
        armorSize = armorSizeTemp;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        while (counter <= armorSize)
        {
            armor.Add(ScriptableObject.CreateInstance<ArmorData>());
            AssetDatabase.CreateAsset(armor[counter], "Assets/Resources/Data/ArmorData/Armor_" + counter + ".asset");
            AssetDatabase.SaveAssets();
            armorDisplayName.Add(armor[counter].armorName);
            counter++;
        }
        if (counter > armorSize)
        {
            armor.RemoveRange(armorSize, armor.Count - armorSize);
            armorDisplayName.RemoveRange(armorSize, armorDisplayName.Count - armorSize);
            for (int i = armorSize; i <= counter; i++)
            {
                AssetDatabase.DeleteAsset("Assets/Resources/Data/ArmorData/Armor_" + i + ".asset");
            }
            AssetDatabase.SaveAssets();
            counter = armorSize;
        }
    }

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
