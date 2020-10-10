using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyDropWindow : EditorWindow
{
    //All GUIStyle variable initialization.
    GUIStyle windowStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;


    //Arrays
    bool[] itemsToggle = new bool[4] { true, false, false, false };

    

    //The i-th Window
    static int windowOrder;

    static EnemyData thisClass;
    public static void ShowWindow(EnemyData enemyData, int size)
    {
        var window = GetWindow<EnemyDropWindow>();
        //Sizing
        window.maxSize = new Vector2(190, 189);
        window.minSize = new Vector2(190, 189);
        window.titleContent = new GUIContent("Drop Item");
        thisClass = enemyData;
        windowOrder = size;
        window.Show();
    }

    private void OnGUI()
    {
        MemsetArray(thisClass.selectedToggle[windowOrder]);
        windowStyle = new GUIStyle(GUI.skin.box);
        windowStyle.normal.background = CreateTexture(1, 1, Color.gray);
        columnStyle = new GUIStyle(GUI.skin.box);
        columnStyle.normal.background = CreateTexture(1, 1, new Color32(70, 70, 70, 200));
        tabStyle = new GUIStyle(GUI.skin.box);
        if (EditorGUIUtility.isProSkin)
            tabStyle.normal.background = CreateTexture(1, 1, new Color32(150, 150, 150, 100));
        else
            tabStyle.normal.background = CreateTexture(1, 1, new Color32(200, 200, 200, 100));

        #region PrimaryTab
        Rect primaryBox = new Rect(0, 0, 190, 193);
                GUILayout.BeginArea(primaryBox, windowStyle);

                    #region DropItem
                    Rect dropItem = new Rect(5, 7, 180, 123);
                        GUILayout.BeginArea(dropItem, tabStyle);
                            GUILayout.Label("Drop Item", EditorStyles.boldLabel);
                            if (EditorGUILayout.Toggle("None", itemsToggle[0], EditorStyles.radioButton))
                            {
                                MemsetArray(0);
                                thisClass.selectedToggle[windowOrder] = 0;
                            }
                            if (EditorGUILayout.Toggle("Items", itemsToggle[1], EditorStyles.radioButton))
                            {
                                MemsetArray(1);
                                thisClass.selectedIndex[windowOrder] = EditorGUILayout.Popup(thisClass.selectedIndex[windowOrder], thisClass.enemyItem, GUILayout.Height(15), GUILayout.Width(170));
                                thisClass.selectedToggle[windowOrder] = 1;
                                GUILayout.Space(5);
                            }
                            if (EditorGUILayout.Toggle("Weapon", itemsToggle[2], EditorStyles.radioButton))
                            {
                                MemsetArray(2);
                                thisClass.selectedIndex[windowOrder] = EditorGUILayout.Popup(thisClass.selectedIndex[windowOrder], thisClass.enemyWeapon, GUILayout.Height(15), GUILayout.Width(170));
                                thisClass.selectedToggle[windowOrder] = 2;
                                GUILayout.Space(5);
                            }
                            if (EditorGUILayout.Toggle("Armor", itemsToggle[3], EditorStyles.radioButton))
                            {
                                MemsetArray(3);
                                thisClass.selectedIndex[windowOrder] = EditorGUILayout.Popup(thisClass.selectedIndex[windowOrder], thisClass.enemyArmor, GUILayout.Height(15), GUILayout.Width(170));
                                thisClass.selectedToggle[windowOrder] = 3;
                                GUILayout.Space(5);
                            }

                    GUILayout.EndArea();
                    #endregion

                    #region Probability
                    Rect probabilityBox = new Rect(5, 13 + dropItem.height, .5f * dropItem.width, 50);
                        GUILayout.BeginArea(probabilityBox, tabStyle);
                            GUILayout.BeginVertical();
                                GUILayout.Label("Probability", EditorStyles.boldLabel);
                                GUILayout.Space(5);
                                GUILayout.BeginHorizontal();
                                    GUILayout.Label("  1 / ");
                                    thisClass.enemyProbability[windowOrder] = EditorGUILayout.IntField(
                                                                                                    thisClass.enemyProbability[windowOrder], 
                                                                                                    GUILayout.Width(probabilityBox.width - 40), 
                                                                                                    GUILayout.Height(20)
                                                                                                    ); 
                                GUILayout.EndHorizontal();
                            GUILayout.EndVertical();
                        GUILayout.EndArea();
                        #endregion

        GUILayout.EndArea();
        #endregion
    }




    #region Features
    /// <summary>
    /// Create Texture for GUI skin.
    /// </summary>
    /// <param name="checkedTrue">bool index that will be checked as true</param>
    /// <returns></returns>
    public void MemsetArray(int checkedTrue)
    {
        for (int i = 0; i < 4; i++)
        {
            itemsToggle[i] = false;
        }
        itemsToggle[(checkedTrue)] = true;
    }
    /// <summary>
    /// Create Texture for GUI skin.
    /// </summary>
    /// <param name="width">pixel width of GUI Skin.</param>
    /// <param name="height">pixel height of GUI Skin.</param>
    /// <param name="col">Color of GUI Skin.</param>
    /// <returns></returns>
    public Texture2D CreateTexture(int width, int height, Color col)
    {
        //Create array of color.
        Color[] colPixel = new Color[width * height];

        for (int i = 0; i < colPixel.Length; ++i)
        {
            colPixel[i] = col;
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(colPixel);
        result.Apply();
        return result;
    }
    #endregion
}
