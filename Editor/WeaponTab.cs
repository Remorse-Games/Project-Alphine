using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class WeaponTab : BaseTab
{
    //Having list of all weapons exist in data.
    public List<WeaponData> weapon = new List<WeaponData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in weaponData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> weaponDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle weaponStyle;

    //How many weapon in ChangeMaximum Func
    public int weaponSize;
    public int weaponSizeTemp;

    public void Init(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in weaponsTab itself.
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
        weaponStyle = new GUIStyle(GUI.skin.box);
        weaponStyle.normal.background = CreateTexture(1, 1, Color.gray);
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

        #region Entry Of weaponsTab GUILayout
        //Start drawing the whole weaponTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the weaponsTab? yes, this one.
            GUILayout.Box(" ", weaponStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));


        GUILayout.EndArea(); // End of drawing the whole weaponTab
        #endregion
    }








    #region Features

    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from weaponSize</param>

    int counter = 0;
    private void ChangeMaximumPrivate(int size)
    {
        weaponSize = weaponSizeTemp;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        while (counter <= weaponSize)
        {
            weapon.Add(ScriptableObject.CreateInstance<WeaponData>());
            AssetDatabase.CreateAsset(weapon[counter], "Assets/Resources/Data/WeaponData/Weapon_" + counter + ".asset");
            AssetDatabase.SaveAssets();
            weaponDisplayName.Add(weapon[counter].weaponName);
            counter++;
        }
        if (counter > weaponSize)
        {
            weapon.RemoveRange(weaponSize, weapon.Count - weaponSize);
            weaponDisplayName.RemoveRange(weaponSize, weaponDisplayName.Count - weaponSize);
            for (int i = weaponSize; i <= counter; i++)
            {
                AssetDatabase.DeleteAsset("Assets/Resources/Data/WeaponData/Wweapon_" + i + ".asset");
            }
            AssetDatabase.SaveAssets();
            counter = weaponSize;
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
