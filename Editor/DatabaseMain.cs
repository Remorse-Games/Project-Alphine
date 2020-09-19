using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

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

    /// <summary>
    /// Database Tab. Create selection grid so we can choose which tab is
    /// active currently.
    /// </summary>
    private void DBTab()
    {
        tabAreaWidth = position.width / 8;
        tabAreaHeight = position.height * .75f;
        GUILayout.BeginVertical("Box");
        selectedTab = GUILayout.SelectionGrid(selectedTab, tabNames, 1, GUILayout.Width(tabAreaWidth), GUILayout.Height(tabAreaHeight));
        GUILayout.EndVertical();
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



        GUILayout.EndArea();
        GUILayout.EndArea();
    }

    /// <summary>
    /// This called when actor list on active.
    /// </summary>
    /// <param name="index">index of actor in a list.</param>
    public void ActorListSelected(int index)
    {
        if (index != -1)
        {
            Debug.Log(player[index].actorName);
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



    #endregion
}