using System.Collections.Generic;
using UnityEngine;
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

    SerializedProperty actorName;
    ActorData actorData;
    #endregion

    #region Init Function
    [MenuItem("Database/Database")]
    static void Init()
    {
        DatabaseMain dbMain = (DatabaseMain)EditorWindow.GetWindow(typeof(DatabaseMain));
        dbMain.minSize = new Vector2(1280f, 720f);
        dbMain.Show();
    }

    //////////////////////////////////////////////////

    /// <summary>
    /// OnGUI Method. Initialize all variables and function in here.
    /// </summary>
    private void OnGUI()
    {
        ValueInit();
        DBTab();
        TabSelection(selectedTab);
    }

    //////////////////////////////////////////////////

    /// <summary>
    /// We Initialize all value that doesn't static in here.
    /// </summary>
    private void ValueInit()
    {
        tabAreaWidth = position.width / 8;
        tabAreaHeight = position.height * .75f;
    }

    //////////////////////////////////////////////////

    /// <summary>
    /// Database Tab. Create selection grid so we can choose which tab is
    /// active currently.
    /// </summary>
    private void DBTab()
    {
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
        switch(selectedTab)
        {
            case 1:
                ClassesTab();
                break;
            case 0:
                ActorsTab();
                break;
        }
    }

    private void OnEnable()
    {
        SerializedObject serializedObject = new SerializedObject(actorData);
        actorName = serializedObject.FindProperty("actorName");
    }
    #endregion

    #region Functionality Tab  
    //////////////////////////////////////////////////
    bool once = false;
    public int actorSize;
    public int actorSizeTemp;

    public class Player
    {
        public string name;
        public int damage;
    }
    List<Player> player = new List<Player>();

    List<string> playerName = new List<string>();
    int index = 0;
    int indexTemp = -1;
    Vector2 scrollPos = Vector2.zero;
    private void ActorsTab()
    {
        if(!once)
        {
            onceCalled();
        }

        //Initialize value that actor tab needed.
        GUIStyle actorStyle;

        float tabWidth = position.width * .85f;
        float tabHeight = position.height - 10f;

        //Actor page
        float firstTabWidth = tabWidth * 3 / 10;

        //Size array of Actor
        //actorSize = 4;

        actorStyle = new GUIStyle(GUI.skin.box);
        actorStyle.normal.background = CreateTexture(1, 1, Color.gray);

        //Button Creation does here.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

        //The whole Actor Page
        GUILayout.Box(" ", actorStyle, GUILayout.Width(position.width - tabAreaWidth), GUILayout.Height(position.height - 25f));

        //First Tab
        GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
        GUILayout.Box("Actors", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

        //Scroll View
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
        index = GUILayout.SelectionGrid(index, playerName.ToArray(), 1, GUILayout.Width(firstTabWidth-20), GUILayout.Height(30*actorSize));
        GUILayout.EndScrollView();

        //Happen everytime selection grid is updated
        if(GUI.changed && index != indexTemp)
        {
            indexTemp = index;
            chooseActor(indexTemp);
            indexTemp = -1;
        }
        

        //if (GUILayout.Button("Choose!", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10))) { chooseActor(index); }

        actorSizeTemp = EditorGUILayout.IntField(actorSizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
        if(GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
        {
            actorSize = actorSizeTemp;
            once = false;
        }



        //EditorGUILayout.PropertyField(actorName, new GUIContent("Actor Name"), GUILayout.Height(20));




        GUILayout.EndArea();

        GUILayout.EndArea();
    }

    public void chooseActor(int index)
    {
        if(index!=-1)
        {
            Debug.Log(player[index].name);
        }
    }

    int counter = 0;
    public void onceCalled()
    {
        while(counter <= actorSize)
        {
            player.Add(new Player { name = "keju" + counter.ToString(), damage = 1 });
            playerName.Add(player[counter].name);
            counter++;
            once = true;
        }
        if(counter > actorSize)
        {
            player.RemoveRange(actorSize, player.Count - actorSize);
            playerName.RemoveRange(actorSize, playerName.Count - actorSize);
            counter = actorSize;
        }
        once = true;
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
    #endregion
}
