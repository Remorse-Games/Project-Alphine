using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
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
        /// <summary>
        /// The Function That Makes Effect Like Tab Button In The Keyboard for Two Seperated Words
        /// </summary>
        /// <param name="key">First String</param>
        /// <param name="value">Second String (After Tab)</param>
        /// <returns></returns>
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

        /// <summary>
        /// Create GameObject that can be played
        /// </summary>
        /// <param name="mainSprite">Sprite Thumbnail For The GameObject</param>
        /// <param name="gameObjectName">Game Object Name in Hierarchy</param>
        /// <param name="controllerPath">The Loaded Controller Path</param>
        public void GameObjectForAnimationCreator(Sprite mainSprite, string gameObjectName, string controllerPath)
        {
            AnimatorController animatorController = Resources.Load<AnimatorController>(controllerPath);

            GameObject objToSpawn;
            objToSpawn = new GameObject(gameObjectName);
            //Add Components
            objToSpawn.AddComponent<SpriteRenderer>();
            objToSpawn.GetComponent<SpriteRenderer>().sprite = mainSprite;
            objToSpawn.AddComponent<Animator>();
            objToSpawn.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        }

        /// <summary>
        /// Create Folder For RPG Scriptable Objects' Data
        /// </summary>
        /// <param name="dataSize">Current Folder Amount</param>
        /// <param name="dataPath">Folder Path</param>
        /// <param name="dataName">Folder Name</param>
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
        /// Creates Line In The EditorWindow
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
        /// <param name="width">Pixel Width of GUI Skin.</param>
        /// <param name="height">Pixel Height of GUI Skin.</param>
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
        /// Create a Texture From a Sprite (To Change Actors' Images)
        /// </summary>
        /// <param name="sprite">The Sprite Which Will Be Converted To Texture</param>
        /// <returns></returns>
        public Texture2D SpriteToTexture(Sprite sprite)
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

        /// <summary>
        /// Load Scriptable Objects' Data From Folder
        /// </summary>
        /// <typeparam name="GameData"></typeparam>
        /// <param name="dataSize">Current Data Amount</param>
        /// <param name="listTabData">The List that Will Be Updated Later</param>
        /// <param name="dataPath">Folder Path</param>
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
        /// Remove Name Duplicates Using Function From List
        /// </summary>
        /// <param name="lastIndex">Last Index To Be Checked</param>
        /// <param name="currentIndex">Current Data Index</param>
        /// /// <param name="currentName">Current Data Name</param>
        /// /// <param name="names">Name List</param>
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
        /// <param name="size">Get Size from dataSize</param>
        /// <param name="listTabData">List of Item that You Want to Display.</param>
        /// <param name="dataTabName">Get Size From dataSize</param>
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

        /// <summary>
        /// File Filter
        /// </summary>
        ExtensionFilter[] fileExtensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
                new ExtensionFilter("Sound Files", "mp3", "wav" ),
                new ExtensionFilter("All Files", "*" ),
        };

        /// <summary>
        /// Sprite Slicer into NxN Size
        /// </summary>
        /// <param name="assetPath">Current Sprite Path ["Assets/Resources/..."]</param>
        /// <param name="sliceWidth">Slice Width</param>
        /// <param name="sliceHeight">Slice Height</param>
        public void SliceSprite(string[] assetPath, int sliceWidth, int sliceHeight)
        {
            int findResourcesPath = assetPath[0].IndexOf("Assets", 0, assetPath[0].Length);
            string relativePath = assetPath[0].Remove(0, findResourcesPath);
            // Remove Path Until The First Index Of Assets

            Texture2D myTexture = (Texture2D)AssetDatabase.LoadAssetAtPath<Texture2D>(relativePath);

            string path = AssetDatabase.GetAssetPath(myTexture);
            TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;
            List<SpriteMetaData> newData = new List<SpriteMetaData>();

            for (int i = 0; i < myTexture.width; i += sliceWidth)
            {
                for (int j = myTexture.height; j > 0; j -= sliceHeight)
                {
                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0.5f);
                    smd.alignment = 9;
                    smd.name = myTexture.name + "_" + (i / sliceWidth);
                    smd.rect = new Rect(i, j - sliceHeight, sliceWidth, sliceHeight);
                    
                    newData.Add(smd);
                }
            }
            ti.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            if (ti.spriteImportMode == SpriteImportMode.Single)
            {
                ti.spriteImportMode = SpriteImportMode.Multiple;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }

        /// <summary>
        /// Create Animation File
        /// </summary>
        /// <param name="spritePath">Path of The Sprite To Be Added In The Animation [Takes only "sprite" from (Assets/Resources/sprite)]</param>
        /// <param name="fps">FPS in Animation</param>
        /// <param name="animCreatePath">Location To Create The Animator ["Assets/Resources/..."]</param>
        /// <param name="spriteName">Sprite Property Name</param>
        public void AnimationCreator(string spritePath, int fps, string animCreatePath, string spriteName)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);
            if (sprites == null)
            {
                Debug.LogError("File Not Found!\nCheck Path / File Extension (sprite)");
            }
            AnimationClip animClip = new AnimationClip();
            animClip.frameRate = fps;
            EditorCurveBinding spriteBinding = new EditorCurveBinding();
            spriteBinding.type = typeof(SpriteRenderer);
            spriteBinding.path = "";
            spriteBinding.propertyName = spriteName;
            ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprites.Length];
            for (int i = 0; i < (sprites.Length); i++)
            {
                spriteKeyFrames[i] = new ObjectReferenceKeyframe();
                spriteKeyFrames[i].time = i / (11.99999976f);
                spriteKeyFrames[i].value = sprites[i];

                AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);
                AssetDatabase.CreateAsset(animClip, animCreatePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Create Controller
        /// </summary>
        /// <param name="animationPath">Path of The Animation To Be Added In The Controller [Takes only "sprite" from (Assets/Resources/sprite)]</param>
        /// <param name="controllerCreatePath">Location To Create The Animator</param>
        public void ControllerCreator(string animationPath, string controllerCreatePath)
        {
            AnimationClip animClip = Resources.Load<AnimationClip>(animationPath);
            if (animClip == null)
            {
                Debug.LogError("File Not Found!\nCheck Path / File Extension (anim)");
            }

            // Creates the controller
            var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(controllerCreatePath);

            AnimatorState emptyState = new AnimatorState();

            emptyState.name = "Main State";
            var stateComponent = controller.layers[0].stateMachine.AddState(emptyState.name);
            stateComponent.motion = animClip;
        }

        /// <summary>
        /// Image Importer
        /// </summary>
        /// <param name="panelName">Up Left Corner Window Name</param>
        /// <param name="assetPath">Data Path ["Assets/Resources/..."]</param>
        /// <returns></returns>
        public Sprite ImageChanger(string panelName, string assetPath)
        {
            string[] rawPath = StandaloneFileBrowser.OpenFilePanel(panelName, assetPath, fileExtensions, false);

            if (rawPath.Length != 0)
            {
                int findResourcesPath = rawPath[0].IndexOf("Resources", 0, rawPath[0].Length);
                // relative path to the Resources folder.
                // I added +10 because of the process to get the end of "Resources\"
                // to get relative path directly even we had subfolder.
                string relativePath = rawPath[0].Remove(0, findResourcesPath + 10);
                // remove the file extension.
                string finalPath = relativePath.Remove(relativePath.Length - 4, 4);

                ActorTab.sliceSpritePath = rawPath; // Give string to ActorTab to run SliceSprite()

                Sprite imageChosen = Resources.Load<Sprite>(finalPath);
                // rawPath[0] = "D:Documents\Assets\ResourcesData\pict.png";

                if (imageChosen == null)
                {
                    Debug.LogError("File Not Found!\nCheck Path / File Extension (sprite)");
                }
                return imageChosen;
            }

            Debug.LogError("Image Changer should have path directly to this!");
            return null;
        }
        #endregion
    }
}