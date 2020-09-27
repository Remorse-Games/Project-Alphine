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
    private int actorSizeTemp;

    #region Features
    /// <summary>
    /// This called when actor list on active.
    /// </summary>
    /// <param name="index">index of actor in a list.</param>
    public abstract void ActorListSelected(int index, int actorSize);

    ///<summary>
    /// Folder checker, create folder if it doesnt exist already, Might refactor into one liner if
    ///</summary>
    public void FolderChecker()
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
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Image"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Image");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/ClassesData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "ClassesData");
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


    /// <summary>
    /// Create a texture from a sprite (Used for changing actors' images)
    /// </summary>
    /// <param name="sprite">the sprite that wants to be converted into texture</param>
    /// <returns></returns>
    public static Texture2D textureFromSprite(Sprite sprite)
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

    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from actorSize</param>


    int counter = 0;
    public void ChangeMaximum(int actorSize, List<ActorData> player, List<string> actorDisplayName)
    {

        actorSize = actorSizeTemp;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        while (counter <= actorSize)
        {
            player.Add(ScriptableObject.CreateInstance<ActorData>());
            Debug.Log("Show player List size : " + player.Count);

            AssetDatabase.CreateAsset(player[counter], "Assets/Resources/Data/ActorData/Actor_" + counter + ".asset");
            AssetDatabase.SaveAssets();
            actorDisplayName.Add(player[counter].actorName);
            counter++;
        }
        if (counter > actorSize)
        {
            player.RemoveRange(actorSize, player.Count - actorSize);
            actorDisplayName.RemoveRange(actorSize, actorDisplayName.Count - actorSize);
            for (int i = actorSize; i <= counter; i++)
            {
                AssetDatabase.DeleteAsset("Assets/Resources/Data/ActorData/Actor_" + i + ".asset");
            }
            AssetDatabase.SaveAssets();
            counter = actorSize;
        }
    }

    ExtensionFilter[] extensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
                new ExtensionFilter("Sound Files", "mp3", "wav" ),
                new ExtensionFilter("All Files", "*" ),
        };

    public void changeFaceImage(int index, List<ActorData> player)
    {
        string relativepath;
        string[] path = StandaloneFileBrowser.OpenFilePanel("Choose Face", "Assets/Resources/Image", extensions, false);

        if (path.Length != 0)
        {
            relativepath = "Image/";
            relativepath += Path.GetFileNameWithoutExtension(path[0]);
            Sprite imageChosen = Resources.Load<Sprite>(relativepath);
            player[index].face = imageChosen;
            ActorListSelected(index, actorSizeTemp);
        }
    }

    public void changeCharacterImage(int index, List<ActorData> player)
    {
        string relativepath;
        string[] path = StandaloneFileBrowser.OpenFilePanel("Choose Character", "Assets/Resources/Image", extensions, false);
        if (path.Length != 0)
        {
            relativepath = "Image/";
            relativepath += Path.GetFileNameWithoutExtension(path[0]);
            Sprite imageChosen = Resources.Load<Sprite>(relativepath);
            player[index].characterWorld = imageChosen;
            ActorListSelected(index, actorSizeTemp);
        }
    }

    public void ImageChanger(int index, string panelName, string assetPath, Sprite imagePick)
    {
        string relativepath;
        Debug.Log("called");
        string[] path = StandaloneFileBrowser.OpenFilePanel(panelName, assetPath, extensions, false);
 //       string[] path = StandaloneFileBrowser.OpenFilePanel("Choose Face", "Assets/Resources/Image", extensions, false);
        if (path.Length != 0)
        {
            relativepath = "Image/";
            relativepath += Path.GetFileNameWithoutExtension(path[0]);
            Sprite imageChosen = Resources.Load<Sprite>(relativepath);
            imagePick = imageChosen;
 //           player[index].battler = imageChosen;
            ActorListSelected(index, actorSizeTemp);
        }
    }
    #endregion
}
