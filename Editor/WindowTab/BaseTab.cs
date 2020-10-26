using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;

/*BASE TAB v 1.0
 * This base tab used for base tabbing, so we won't use
 * repeated codes that used in many occasion.
 * This code still under development, make sure to contact
 * jodysag5@gmail.com for further question about tabbing.
 * 
 */

public abstract class BaseTab
{
    #region Features
    /// <summary>
    /// This called when actor list on active.
    /// pick item to load it.
    /// </summary>
    /// <param name="index">index of actor in a list.</param>
    public abstract void ItemTabLoader(int index);

    ///<summary>
    /// Folder checker, create folder if it doesnt exist already, Might refactor into one liner if
    ///</summary>
    public static void FolderChecker()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Data");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/ActorData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "ActorData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/ActorData/TraitData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data/TraitData", "TraitData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Image"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Image");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/ClassesData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "ClassesData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/SkillData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "SkillData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/ItemData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "ItemData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/WeaponData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "WeaponData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/ArmorData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "ArmorData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/EnemyData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "EnemyData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/TroopData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "TroopData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/StateData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "StateData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/TermData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "TermData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/SystemData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "SystemData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/TypeData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "TypeData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/TypeData/ElementData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data/TypeData", "ElementData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/TypeData/SkillData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data/TypeData", "SkillData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/TypeData/WeaponData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data/TypeData", "WeaponData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/TypeData/ArmorData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data/TypeData", "ArmorData");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/TypeData/EquipmentData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data/TypeData", "EquipmentData");
        }
    }

    public void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
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


    /// <summary>
    /// Create a texture from a sprite (Used for changing actors' images)
    /// </summary>
    /// <param name="sprite">the sprite that wants to be converted into texture</param>
    /// <returns></returns>
    public Texture2D TextureToSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

    public void LoadGameData<GameData>(ref int dataSize, List<GameData> listTabData, string dataPath) where GameData : ScriptableObject
    {
        GameData[] list = Resources.LoadAll<GameData>(dataPath);
        dataSize = list.Length;

        foreach (GameData gd in list)
        {
            listTabData.Add(gd);
        }
    }


    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from actorSize</param>
    /// <param name="listTabData">list of item that you want to display.</param>
    /// <param name="dataTabName">get size from actorSize</param>
    public void ChangeMaximum<GameData>(int dataSize, List<GameData> listTabData, string dataPath) where GameData : ScriptableObject
    {
        int counter = 0;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        if (dataSize > listTabData.Count)
            while (dataSize > listTabData.Count)
            {
                listTabData.Add(ScriptableObject.CreateInstance<GameData>());
                counter = listTabData.Count;
                AssetDatabase.CreateAsset(listTabData[listTabData.Count - 1], dataPath + counter + ".asset");
                AssetDatabase.SaveAssets();
                counter++;
            }
        else
        {
            int tempListTabData = listTabData.Count;
            listTabData.RemoveRange(dataSize, listTabData.Count - dataSize);
            for (int i = tempListTabData; i > dataSize; i--)
            {
                AssetDatabase.DeleteAsset(dataPath + i + ".asset");
            }
            AssetDatabase.SaveAssets();
        }
    }

    ExtensionFilter[] fileExtensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
                new ExtensionFilter("Sound Files", "mp3", "wav" ),
                new ExtensionFilter("All Files", "*" ),
        };

    public Sprite ImageChanger(int index, string panelName, string assetPath)
    {
        string relativepath;
        string[] path = StandaloneFileBrowser.OpenFilePanel(panelName, assetPath, fileExtensions, false);

        if (path.Length != 0)
        {
            relativepath = "Image/";
            relativepath += Path.GetFileNameWithoutExtension(path[0]);
            Sprite imageChosen = Resources.Load<Sprite>(relativepath);
            return imageChosen;
        }

        Debug.LogError("Image Changer should have path directly to this!");
        return null;
    }
    #endregion
}