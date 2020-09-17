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

    public List<ActorData> player = new List<ActorData>();
    List<string> actorDisplayName = new List<string>();

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

    int index = 0;
    int indexTemp = -1;
    Vector2 scrollPos = Vector2.zero;

    private void ActorsTab()
    {
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
        index = GUILayout.SelectionGrid(index, actorDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(30 * actorSize));
        GUILayout.EndScrollView();

        //Happen everytime selection grid is updated
        if (GUI.changed && index != indexTemp)
        {
            indexTemp = index;
            ActorListSelected(indexTemp);
            indexTemp = -1;
        }

        actorSize = EditorGUILayout.IntField(actorSize, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
        if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
        {
            ChangeMaximum(actorSize);
        }

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

    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from actorSize</param>


    int counter = 0;
    private void ChangeMaximum(int size)
    {
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
        //for (int i = 0; i < size; i++)
        //{
        //    player.Add(CreateInstance<ActorData>());
        //    AssetDatabase.CreateAsset(player[i], "Assets/Resources/Data/ActorData/Actor_" + count + ".asset");
        //    AssetDatabase.SaveAssets();
        //    actorDisplayName.Add(player[i].name);
        //    count++;
        //}
    }
    #endregion
}