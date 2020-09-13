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
    private void ActorsTab()
    {
        //Initialize value that actor tab needed.
        GUIStyle actorStyle;

        float tabWidth = position.width * .85f;
        float tabHeight = position.height - 10f;

        //Actor page
        float firstTabWidth = tabWidth * 3 / 10;

        //Size array of Actor
        int actorSize = 4;

        actorStyle = new GUIStyle(GUI.skin.box);
        actorStyle.normal.background = CreateTexture(1, 1, Color.gray);

        //Button Creation does here.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

        //The whole Actor Page
        GUILayout.Box(" ", actorStyle, GUILayout.Width(position.width - tabAreaWidth), GUILayout.Height(position.height - 25f));

        //First Tab
        GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
        GUILayout.Box("Actors", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));
        actorSize = EditorGUILayout.IntField(actorSize, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
        GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));

        EditorGUILayout.PropertyField(actorName, new GUIContent("Actor Name"), GUILayout.Height(20));

        GUILayout.EndArea();
        GUILayout.EndArea();
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
