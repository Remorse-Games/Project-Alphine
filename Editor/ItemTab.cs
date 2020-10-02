using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class ItemTab : BaseTab
{

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle itemStyle;

    //How many skill in ChangeMaximum Func
    public int itemSize;
    public int itemSizeTemp;

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


    #endregion
}