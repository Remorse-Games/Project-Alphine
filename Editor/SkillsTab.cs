using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;

public class SkillsTab
{
    //All GUIStyle variable initialization.
    GUIStyle skillStyle;

    public void Init(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in SkillsTab itself.
        ///So make sure you understand that tabbing in here does not
        ///have any corelation with DatabaseMain.cs Tab system.
        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////START REGION OF VALUE INIT/////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        float tabWidth = position.width * .85f;
        float tabHeight = position.height - 10f;

        //Style area.
        skillStyle = new GUIStyle(GUI.skin.box);
        skillStyle.normal.background = CreateTexture(1, 1, Color.gray);

        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////END REGION OF VALUE INIT///////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        #region Entry Of SkillsTab GUILayout
        //Start drawing the whole SkillsTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the SkillsTab? yes, this one.
            GUILayout.Box(" ", skillStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));
            
            
        GUILayout.EndArea(); // End of drawing SkillsTab

        #endregion

    }

    #region Features
    /// <summary>
    /// Create Texture for GUI skin.
    /// </summary>
    /// <param name="width">pixel width of GUI Skin.</param>
    /// <param name="height">pixel height of GUI Skin.</param>
    /// <param name="col">Color of GUI Skin.</param>
    /// <returns></returns>
    private Texture2D CreateTexture(int width, int height, Color col)
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
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/SkillData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "SkillData");
        }

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
    #endregion
}
