using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Runtime.CompilerServices;

public class InitialEquipmentWindow : EditorWindow
{
    //Base Value
    int i = 0;
    int firstSelectedArmor;

    GUIStyle windowStyle;

    //Static Values
    static ActorData thisClass;
    static string[] armorList;
    static int windowOrder;
    public static void ShowWindow(ActorData equipmentData, string windowName, string[] list, int order)
    {
        var window = GetWindow<InitialEquipmentWindow>();
        var position = window.position;
        //Sizing
        window.maxSize = new Vector2(190, 95);
        window.minSize = new Vector2(190, 95);
        window.titleContent = new GUIContent(windowName);
        position.center = new Rect(Screen.width * -1 * .05f, Screen.height * -1 * .05f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        thisClass = equipmentData;
        armorList = list;
        windowOrder = order;
        window.position = position;
        window.Show();
    }

    private void OnLostFocus()
    {
        this.Focus();
    }

    private void OnGUI()
    {
        BaseValue(i++);
        windowStyle = new GUIStyle(GUI.skin.box);
        windowStyle.normal.background = CreateTexture(1, 1, Color.gray);

        Rect primaryBox = new Rect(0, 0, 190, 95);
        GUILayout.BeginArea(primaryBox, windowStyle);
            GUILayout.BeginVertical();
                GUILayout.Label("Armor:", EditorStyles.boldLabel);
                thisClass.allArmorIndexes[windowOrder] = EditorGUILayout.Popup(thisClass.allArmorIndexes[windowOrder], armorList);

                GUILayout.Space(30);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("OK", GUILayout.Width(50), GUILayout.Height(20)))
                {
                    this.Close();
                }
                if (GUILayout.Button("Cancel", GUILayout.Width(50), GUILayout.Height(20)))
                {
                    thisClass.allArmorIndexes[windowOrder] = firstSelectedArmor;
                    this.Close();
                }

                GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        GUILayout.EndArea();
    }


    #region Features
    public void BaseValue(int i)
    {
        if (i == 0)
        {
            firstSelectedArmor = thisClass.allArmorIndexes[windowOrder];
        }
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
