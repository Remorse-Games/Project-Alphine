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

public enum TabType
{
    Actor,
    Classes,
    Skill,
    Item,
    Armor,
    Enemy,
    Weapon,
    State,
};

namespace Remorse.Tools.RPGDatabase.Window
{
    public abstract class BaseTab
    {
        #region Features
        public string PadString(string key, string value)
        {
            int pad = 4 - (key.Length / 4);

            if (key.Length >= 12)
            {
                pad++;
            }
            string format = key;

            for (int i = 0; i < pad; i++)
            {
                format += '\t';
            }
            return string.Format(format + "{0}", value);
        }
        public void FolderCreator(int dataSize, string dataPath, string dataName)
        {
            // dataPath = "Assets/Resources/Data/ActorData"

            int counter = 0;
            while (AssetDatabase.IsValidFolder(dataPath + "/" + dataName + (counter + 1)))
            {
                counter++;
            }

            if (dataSize > counter)
            {
                while (dataSize > counter)
                {
                    AssetDatabase.CreateFolder(dataPath, dataName + (counter + 1));
                    counter++;
                }
            }
            else
            {
                for (int i = counter; i > dataSize; i--)
                {
                    AssetDatabase.DeleteAsset(dataPath + "/" + dataName + i);
                }
            }
        }
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
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/ActorData/TraitData1"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/Data/ActorData", "TraitData1");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">Line Color (ex. Color.black)</param>
        /// <param name="thickness">Line Thickness (int)</param>
        /// <param name="padding">Line Padding (int)</param>
        /// <returns></returns>
        public void DrawUILine(Color color, int thickness, int padding)
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
                Texture2D newTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                             (int)sprite.textureRect.y,
                                                             (int)sprite.textureRect.width,
                                                             (int)sprite.textureRect.height);
                newTexture.SetPixels(newColors);
                newTexture.Apply();
                return newTexture;
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
        /// Remove name duplicates using function from a list
        /// </summary>
        /// <param name="lastIndex">last item index to be checked using for loop</param>
        /// <param name="currentIndex">current item index in the list.</param>
        /// /// <param name="currentName">the last name changed by user.</param>
        /// /// <param name="names">list of item that you want to check.</param>
        public string RemoveDuplicates(int lastIndex, int currentIndex, string currentName, List<string> names)
        {
            names[currentIndex] = currentName;

            string originalName = currentName;
            bool sameNameFound = true;
            int nameIncrement = 0;
            while (sameNameFound)
            {
                sameNameFound = false;
                for (int j = 0; j < lastIndex; j++)
                {
                    if (names[j] == names[currentIndex] && currentIndex != j)
                    {
                        sameNameFound = true;
                        break;
                    }
                }

                if (sameNameFound)
                    names[currentIndex] = originalName + ' ' + ++nameIncrement;
            }

            return names[currentIndex];
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
            int counter = listTabData.Count;

            //This count only useful when we doesn't have a name yet.
            //you can remove this when decide a new format later.
            if (dataSize > listTabData.Count)
                while (dataSize > listTabData.Count)
                {
                    listTabData.Add(ScriptableObject.CreateInstance<GameData>());
                    AssetDatabase.CreateAsset(listTabData[listTabData.Count - 1], dataPath + (counter + 1) + ".asset");
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
            string[] rawPath = StandaloneFileBrowser.OpenFilePanel(panelName, assetPath, fileExtensions, false);
            string fileName = Path.GetFileNameWithoutExtension(rawPath[0]);

            if (rawPath.Length != 0)
            {
                int findResourcesPath = rawPath[0].IndexOf("Resources", 0, rawPath[0].Length);
                // relative path to the Resources folder.
                // I add + 10 because to get end of Resouces words + backslash
                // to get relative path directly even we had subfolder.
                string relativePath = rawPath[0].Remove(0, findResourcesPath + 10);
                // remove the file extension.
                string finalPath = relativePath.Remove(relativePath.Length - 4, 4);

                Sprite imageChosen = Resources.Load<Sprite>(finalPath);

                if (imageChosen == null)
                {
                    Debug.LogError("File did not found! Try to check path / file extension.");
                }

                return imageChosen;
            }

            Debug.LogError("Image Changer should have path directly to this!");
            return null;
        }
        #endregion
    }
}