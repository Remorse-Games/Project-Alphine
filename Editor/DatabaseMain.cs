using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using SFB;

[CustomEditor(typeof(ActorData))]
[CanEditMultipleObjects]
public class DatabaseMain : EditorWindow
{
    #region Init Values

    private int selectedTab = 0;
    private string[] tabNames = {
        "Actors",
        "Classes" ,
        "Skills",
        "Item",
        "Weapons",
        "Armors",
        "Enemies",
        "Troops",
        "States",
        "Animations",
        "Tilesets",
        "Common Events",
        "System",
        "Types",
        "Terms"
    };

    private float tabAreaWidth;
    private float tabAreaHeight;

    public List<ActorData> player = new List<ActorData>();
    List<string> actorDisplayName = new List<string>();

        #region  DeleteLater
            public string[] actorClassesList = 
            {
                "Mage",
                "Cleric",
                "Healer",
                "Warrior",
            };
            public int selectedClassIndex;
        #endregion

    #endregion

    #region Init Function
    [MenuItem("Database/Database")]
    static void Init()
    {
        DatabaseMain dbMain = (DatabaseMain)EditorWindow.GetWindow(typeof(DatabaseMain));
        dbMain.minSize = new Vector2(1280f, 720f);
        dbMain.Show();
    }

    public void OnEnable()
    {
        ValueInit();
        FolderChecker();
    }

    //////////////////////////////////////////////////

    /// <summary>
    /// OnGUI Method. Initialize all variables and function in here.
    /// </summary>
    private void OnGUI()
    {
        DBTab();
        TabSelection(selectedTab);
    }

    //////////////////////////////////////////////////

    /// <summary>
    /// We Initialize all value that doesn't static in here.
    /// </summary>
    private void ValueInit()
    {

    }

    //////////////////////////////////////////////////
    ///<summary>
    /// Folder checker, create folder if it doesnt exist already, Might refactor into one liner if
    ///</summary>
    private void FolderChecker()
    {
        if(!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        if(!AssetDatabase.IsValidFolder("Assets/Resources/Data"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Data");
        }
        if(!AssetDatabase.IsValidFolder("Assets/Resources/Data/ActorData"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "ActorData");
        }
        if(!AssetDatabase.IsValidFolder("Assets/Resources/Image"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Image");
        }
        
    }

    /// <summary>
    /// Database Tab. Create selection grid so we can choose which tab is
    /// active currently.
    /// </summary>
    private void DBTab()
    {
        tabAreaWidth = position.width / 8;
        tabAreaHeight = position.height * .75f;
        
        #region  ActorBoxUI
        GUILayout.BeginVertical("Box");
        selectedTab = GUILayout.SelectionGrid(selectedTab, tabNames, 1, GUILayout.Width(tabAreaWidth), GUILayout.Height(tabAreaHeight));
        GUILayout.EndVertical();
        #endregion
    }

    //////////////////////////////////////////////////

    /// <summary>
    /// Switch statement for tab selection.
    /// Maybe will refactor later.
    /// </summary>
    /// <param name="selectedTab"></param>
    private void TabSelection(int selectedTab)
    {
        switch (selectedTab)
        {
            case 1:
                ClassesTab();
                break;
            case 0:
                ActorsTab();
                break;
        }
    }

    #endregion

    #region Functionality Tab  
    //////////////////////////////////////////////////

    //How many actor in ChangeMaximum Func
    public int actorSize;
    public int actorSizeTemp;

    int index = 0;
    int indexTemp = -1;
    Vector2 scrollPos = Vector2.zero;
    Vector2 equipmentScrollPos = Vector2.zero;
    Vector2 traitsScrollPos = Vector2.zero;
    
    Texture2D faceImage;
    Texture2D characterImage;
    Texture2D battlerImage;
    
    #region TempValues
    string actorNameTemp;
    string actorNicknameTemp;
    int initlevelTemp;
    int maxlevelTemp;
    string profileTemp;

    #endregion


    private void ActorsTab()
    {
        //Initialize value that actor tab needed.
        GUIStyle actorStyle;
        GUIStyle tabStyle;
        GUIStyle columnStyle;

        float tabWidth = position.width * .85f;
        float tabHeight = position.height - 10f;


        //Actor page
        float firstTabWidth = tabWidth * 3 / 10;

        //Size array of Actor
        //actorSize = 4;

        actorStyle = new GUIStyle(GUI.skin.box);
        actorStyle.normal.background = CreateTexture(1, 1, Color.gray);
        columnStyle = new GUIStyle(GUI.skin.box);
        columnStyle.normal.background = CreateTexture(1, 1, new Color32(99, 100, 100, 200));
        tabStyle = new GUIStyle(GUI.skin.box);
        tabStyle.normal.background = CreateTexture(1, 1, new Color32(76, 76, 76, 200));

        //Button Creation does here.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

        //The whole Actor Page
        GUILayout.Box(" ", actorStyle, GUILayout.Width(position.width - tabAreaWidth), GUILayout.Height(position.height - 25f));

        //First Tab
        GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
        GUILayout.Box("Actors", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

        //Scroll View
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
            index = GUILayout.SelectionGrid(index, actorDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height/24 * actorSize));
        GUILayout.EndScrollView();

        //Happen everytime selection grid is updated
        if (GUI.changed && index != indexTemp)
        {
            indexTemp = index;
            ActorListSelected(indexTemp);
            indexTemp = -1;
        }

        actorSizeTemp = EditorGUILayout.IntField(actorSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
        if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
        {
            ChangeMaximum(actorSize);
        }


        //second Column
        GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth+70, tabHeight-15), columnStyle);

        //GeneralSettings tab
        Rect generalBox = new Rect(5, 5, firstTabWidth + 60, position.height/3 - 50);

            GUILayout.BeginArea(generalBox, tabStyle); //Start of general settings tab
            GUILayout.Label("General Settings", EditorStyles.boldLabel); //general settings label
            GUILayout.BeginVertical(); 
                GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(); //Name label, name field, class label, and class popup
                        GUILayout.Label("Name:");
                        if(actorSize > 0)
                        {player[index].actorName = GUILayout.TextField(player[index].actorName, GUILayout.Width(generalBox.width/2 - 15), GUILayout.Height(generalBox.height/8));
                         actorDisplayName[index] = player[index].actorName;}
                        else
                        {actorNameTemp = GUILayout.TextField(actorNameTemp, GUILayout.Width(generalBox.width/2 - 15), GUILayout.Height(generalBox.height/8));}
                        GUILayout.Space(generalBox.height/20);
                        GUILayout.Label("Class:");
                        selectedClassIndex = EditorGUILayout.Popup(selectedClassIndex, actorClassesList, GUILayout.Height(generalBox.height/8), GUILayout.Width(generalBox.width/2 -15));
                    GUILayout.EndVertical(); //Name label, name field, class label, and class popup (ending)
                    GUILayout.BeginVertical(); //Nickname label, nickname field, initial level and max level label and field
                        GUILayout.Label("Nickname:");
                        if(actorSize>0)
                        {player[index].actorNickname = GUILayout.TextField(player[index].actorNickname, GUILayout.Width(generalBox.width/2 - 15), GUILayout.Height(generalBox.height/8));}
                        else
                        {actorNicknameTemp = GUILayout.TextField(actorNicknameTemp, GUILayout.Width(generalBox.width/2 - 15), GUILayout.Height(generalBox.height/8));}
                        GUILayout.Space(generalBox.height/20);
                        GUILayout.BeginHorizontal();
                            GUILayout.BeginVertical();
                                GUILayout.Label("Initial Level:");
                                if(actorSize>0)
                                player[index].initLevel = EditorGUILayout.IntField(player[index].initLevel, GUILayout.Width(generalBox.width/4-20), GUILayout.Height(generalBox.height/8));
                                else
                                initlevelTemp = EditorGUILayout.IntField(initlevelTemp, GUILayout.Width(generalBox.width/4-20), GUILayout.Height(generalBox.height/8));
                            GUILayout.EndVertical();
                            GUILayout.BeginVertical();
                                GUILayout.Label("Max Level:");
                                if(actorSize>0)
                                player[index].maxLevel = EditorGUILayout.IntField(player[index].maxLevel, GUILayout.Width(generalBox.width/4-20), GUILayout.Height(generalBox.height/8));
                                else
                                maxlevelTemp = EditorGUILayout.IntField(maxlevelTemp, GUILayout.Width(generalBox.width/4-20), GUILayout.Height(generalBox.height/8));
                            GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    GUILayout.EndVertical(); //Nickname label, nickname field, initial level and max level label and field (ending)
                GUILayout.EndHorizontal();
                GUILayout.Label("Profile:");
                if(actorSize>0)
                player[index].notes = GUILayout.TextArea(player[index].notes, GUILayout.Width(firstTabWidth+50), GUILayout.Height(generalBox.height/5));
                else
                profileTemp = GUILayout.TextArea(profileTemp, GUILayout.Width(firstTabWidth+50), GUILayout.Height(generalBox.height/5));
            GUILayout.EndVertical();
            GUILayout.EndArea();


        //Images tab
        Rect imageBox = new Rect(5, generalBox.height + 10, firstTabWidth + 60, position.height / 3 - 30); //Second Row
            GUILayout.BeginArea(imageBox ,tabStyle); //Image Tab
                GUILayout.Space(2);
                GUILayout.Label("Images", EditorStyles.boldLabel); //Image Label
                GUILayout.Space(imageBox.height/15);
                //Three image parts
                GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                        GUILayout.Label("Face:");
                        GUILayout.Box(faceImage, GUILayout.Width(imageBox.width / 3 - 10), GUILayout.Height(imageBox.width / 3 - 10));
                        if(GUILayout.Button("Edit Face", GUILayout.Height(imageBox.height/10), GUILayout.Width(imageBox.width/3 - 10))) {changeFaceImage();}
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                        GUILayout.Label("Character:");
                        GUILayout.Box(characterImage, GUILayout.Width(imageBox.width / 3 - 10), GUILayout.Height(imageBox.width / 3 - 10));
                        if(GUILayout.Button("Edit Character", GUILayout.Height(imageBox.height/10), GUILayout.Width(imageBox.width/3 - 10))) {changeCharacterImage();}                        
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                        GUILayout.Label("[SV] Battler: ");
                        GUILayout.Box(battlerImage, GUILayout.Width(imageBox.width/3 - 10), GUILayout.Height(imageBox.width / 3 - 10));
                        if(GUILayout.Button("Edit Battler", GUILayout.Height(imageBox.height/10), GUILayout.Width(imageBox.width/3 - 10))) {changeBattlerImage();}
                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();
        GUILayout.EndArea();

        //Initial Equipment
        Rect equipmentBox = new Rect(5, generalBox.height + imageBox.height + 20, firstTabWidth + 60, position.height/3+20);
            GUILayout.BeginArea(equipmentBox, tabStyle);
                GUILayout.Space(2);
                GUILayout.Label("Initial Equipment", EditorStyles.boldLabel);
                GUILayout.BeginVertical();
                GUILayout.Space(equipmentBox.height/10);
                    GUILayout.BeginHorizontal();
                        GUILayout.Label("Type", GUILayout.Width(equipmentBox.width*3/8));
                        GUILayout.Label("Equipment Item", GUILayout.Width(equipmentBox.width*5/8));
                    GUILayout.EndHorizontal();
                    equipmentScrollPos = GUILayout.BeginScrollView(equipmentScrollPos, false, true, GUILayout.Width(firstTabWidth+50), GUILayout.Height(equipmentBox.height*0.7f));

                    GUILayout.EndScrollView();
                GUILayout.EndVertical();
            GUILayout.EndArea();
        GUILayout.EndArea();

        
        //Third Column
        GUILayout.BeginArea(new Rect(position.width - (position.width - firstTabWidth*2) + 77, 0, firstTabWidth+25, tabHeight-15), columnStyle);
            
            //Traits
            Rect traitsBox = new Rect(5, 5, firstTabWidth + 15, position.height*5/8); 
            GUILayout.BeginArea(traitsBox, tabStyle);
                GUILayout.Space(2);
                GUILayout.Label("Traits", EditorStyles.boldLabel);
                GUILayout.Space(traitsBox.height/30);
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Type", GUILayout.Width(traitsBox.width*3/8));
                    GUILayout.Label("Content", GUILayout.Width(traitsBox.width*5/8));
                GUILayout.EndHorizontal();
                traitsScrollPos = GUILayout.BeginScrollView(traitsScrollPos, false, true, GUILayout.Width(firstTabWidth+5), GUILayout.Height(traitsBox.height*0.87f));

                GUILayout.EndScrollView();
            GUILayout.EndArea();

            //Notes
            Rect notesBox = new Rect(5, traitsBox.height+15, firstTabWidth + 15, position.height * 2.5f/8);
            GUILayout.BeginArea(notesBox, tabStyle);
                GUILayout.Space(2);
                GUILayout.Label("Notes", EditorStyles.boldLabel);
                GUILayout.Space(notesBox.height/50);
                if(actorSize > 0)
                player[index].notes =  GUILayout.TextArea(player[index].notes, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.9f));
                else
                profileTemp = GUILayout.TextArea(profileTemp, GUILayout.Width(notesBox.width - 5), GUILayout.Height(notesBox.height * 0.85f));
            GUILayout.EndArea();
        GUILayout.EndArea();
        GUILayout.EndArea();
    }

    /// <summary>
    /// This called when actor list on active.
    /// </summary>
    /// <param name="index">index of actor in a list.</param>
    public void ActorListSelected(int index)
    {
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if(actorSize>0)
            {
                if(player[index].face == null)
                    faceImage = defTex;
                else
                    faceImage = textureFromSprite(player[index].face);


                if(player[index].characterWorld == null)
                    characterImage = defTex;
                else
                    characterImage = textureFromSprite(player[index].characterWorld);

                    
                if(player[index].battler == null)
                    battlerImage = defTex;
                else
                    battlerImage = textureFromSprite(player[index].battler);
            }
        }
    }
    //////////////////////////////////////////////////

    private void ClassesTab()
    {

    }


    //////////////////////////////////////////////////
    #endregion

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
    private void ChangeMaximum(int size)
    {
        actorSize = actorSizeTemp;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
         while(counter <= actorSize)
        {
            player.Add(CreateInstance<ActorData>());
            AssetDatabase.CreateAsset(player[counter], "Assets/Resources/Data/ActorData/Actor_" + counter + ".asset");
            AssetDatabase.SaveAssets();
            actorDisplayName.Add(player[counter].actorName);
            counter++;
        }
         if(counter > actorSize)
        {
            player.RemoveRange(actorSize, player.Count - actorSize);
            actorDisplayName.RemoveRange(actorSize, actorDisplayName.Count - actorSize);
            for(int i=actorSize;i<=counter;i++)
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

    private void changeFaceImage()
    {
        string relativepath;
        string[] path = StandaloneFileBrowser.OpenFilePanel("Choose Face", "Assets/Resources/Image",extensions, false);
        relativepath = "Image/";
        relativepath += Path.GetFileNameWithoutExtension(path[0]);
        Sprite imageChosen = Resources.Load<Sprite>(relativepath);
        player[index].face = imageChosen;
        ActorListSelected(index);
    }

    private void changeCharacterImage()
    {
        string relativepath;
        string[] path = StandaloneFileBrowser.OpenFilePanel("Choose Character", "Assets/Resources/Image",extensions, false);
        relativepath = "Image/";
        relativepath += Path.GetFileNameWithoutExtension(path[0]);
        Sprite imageChosen = Resources.Load<Sprite>(relativepath);
        player[index].characterWorld = imageChosen;
        ActorListSelected(index);
    }

    private void changeBattlerImage()
    {
        string relativepath;
        Debug.Log("called");
        string[] path = StandaloneFileBrowser.OpenFilePanel("Choose Face", "Assets/Resources/Image",extensions, false);
        relativepath = "Image/";
        relativepath += Path.GetFileNameWithoutExtension(path[0]);
        Sprite imageChosen = Resources.Load<Sprite>(relativepath);
        player[index].battler = imageChosen;
        ActorListSelected(index);
    }



    #endregion
}