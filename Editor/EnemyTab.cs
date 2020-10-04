using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SFB;
using System.ComponentModel;

public class EnemyTab : BaseTab
{

    //Having list of all enemys exist in data.
    public List<EnemyData> enemy = new List<EnemyData>();

    //List of names. Why you ask? because selectionGrid require
    //array of string, which we cannot obtain in enemyData.
    //I hope later got better solution about this to not do
    //a double List for this kind of thing.
    List<string> enemyDisplayName = new List<string>();

    //All GUIStyle variable initialization.
    GUIStyle tabStyle;
    GUIStyle columnStyle;
    GUIStyle enemyStyle;

    //Scroll position. Is this necessary?
    Vector2 scrollPos = Vector2.zero;
    Vector2 equipmentScrollPos = Vector2.zero;
    Vector2 traitsScrollPos = Vector2.zero;

    //How many enemy in ChangeMaximum Func
    public int enemySize;
    public int enemySizeTemp;

    //i don't know about this but i leave this to handle later.
    int index = 0;
    int indexTemp = -1;

    Texture2D enemyImage;

    public void Init(Rect position)
    {
        #region A Bit Explanation About Local Tab
        ///So there is 2 types of Tab,
        ///One is in Database that not included here.
        ///Second, there is 3 tab slicing in enemysTab itself.
        ///So make sure you understand that tabbing in here does not
        ///have any corelation with DatabaseMain.cs Tab system.
        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////START REGION OF VALUE INIT/////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        float tabWidth = position.width * .85f;
        float tabHeight = position.height - 10f;
        float firstTabWidth = tabWidth * 3 / 10;

        //Style area.
        enemyStyle = new GUIStyle(GUI.skin.box);
        enemyStyle.normal.background = CreateTexture(1, 1, Color.gray);
        columnStyle = new GUIStyle(GUI.skin.box);
        columnStyle.normal.background = CreateTexture(1, 1, new Color32(99, 100, 100, 200));
        tabStyle = new GUIStyle(GUI.skin.box);
        if (EditorGUIUtility.isProSkin)
            tabStyle.normal.background = CreateTexture(1, 1, new Color32(76, 76, 76, 200));
        else
            tabStyle.normal.background = CreateTexture(1, 1, new Color32(200, 200, 200, 200));


        ////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////END REGION OF VALUE INIT///////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////

        #region Entry Of enemysTab GUILayout
        //Start drawing the whole enemyTab.
        GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the enemysTab? yes, this one.
            GUILayout.Box(" ", enemyStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));

            #region Tab 1/3
            //First Tab of three
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));

                GUILayout.Box("Enemies", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15));

                //Scroll View
                #region ScrollView
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .82f));
                index = GUILayout.SelectionGrid(index, enemyDisplayName.ToArray(), 1, GUILayout.Width(firstTabWidth - 20), GUILayout.Height(position.height / 24 * enemySize));
                GUILayout.EndScrollView();
                #endregion

                //Happen everytime selection grid is updated
                if (GUI.changed && index != indexTemp)
                {
                    indexTemp = index;
                    ItemTabLoader(indexTemp);
                    indexTemp = -1;
                }

                // Change Maximum field and button
                enemySizeTemp = EditorGUILayout.IntField(enemySizeTemp, GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                if (GUILayout.Button("Change Maximum", GUILayout.Width(firstTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
                {
                    ChangeMaximumPrivate(enemySize);
                }

            GUILayout.EndArea();
            #endregion // End Of First Tab

        
            #region Tab 2/3
            GUILayout.BeginArea(new Rect(firstTabWidth + 5, 0, firstTabWidth + 70, tabHeight - 25), columnStyle);

                Rect generalBox = new Rect(5, 5, firstTabWidth + 60, position.height / 4 + 60);
                    #region GeneralSettings
                    GUILayout.BeginArea(generalBox, tabStyle); // Start of General Settings tab
                        GUILayout.Label("General Settings", EditorStyles.boldLabel); // General Settings label
                        GUILayout.BeginHorizontal();
                            GUILayout.BeginVertical();
                                GUILayout.Label("Name:"); // Name label
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyName = GUILayout.TextField(enemy[index].enemyName, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                    enemyDisplayName[index] = enemy[index].enemyName;
                                }
                                else
                                {
                                    GUILayout.TextField("Null", GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height / 8));
                                }
                                GUILayout.Label("Image:"); // Image labe;
                                GUILayout.Box(enemyImage, GUILayout.Width(generalBox.width / 2 - 15), GUILayout.Height(generalBox.height * .50f - 10)); // Image Box preview
                                if (GUILayout.Button("Edit Image", GUILayout.Height(20), GUILayout.Width(generalBox.width / 2 - 15))) // Image changer Button
                                {
                                    enemy[index].Image = ImageChanger(
                                    index,
                                    "Choose Image",
                                    "Assets/Resources/Image"
                                    );
                                    ItemTabLoader(index);
                                }
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("Max HP:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyMaxHP = EditorGUILayout.IntField(enemy[index].enemyMaxHP, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                                GUILayout.Label("Attack:");
                                if (enemySize > 0)
                                { 
                                    enemy[index].enemyAttack = EditorGUILayout.IntField(enemy[index].enemyAttack, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8)); 
                                }
                                else
                                { 
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8)); 
                                }

                                GUILayout.Label("M.Attack:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyMAttack = EditorGUILayout.IntField(enemy[index].enemyMAttack, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                GUILayout.Label("Agility:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyAgility = EditorGUILayout.IntField(enemy[index].enemyAgility, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                                GUILayout.Label("Max MP:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyMaxMP = EditorGUILayout.IntField(enemy[index].enemyMaxMP, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                                GUILayout.Label("Defense:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyDefense = EditorGUILayout.IntField(enemy[index].enemyDefense, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                                GUILayout.Label("M.Defense:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyMDefense = EditorGUILayout.IntField(enemy[index].enemyMDefense, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                                GUILayout.Label("Luck:");
                                if (enemySize > 0)
                                {
                                    enemy[index].enemyLuck = EditorGUILayout.IntField(enemy[index].enemyLuck, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }
                                else
                                {
                                    EditorGUILayout.IntField(-1, GUILayout.Width(generalBox.width / 4 - 5), GUILayout.Height(generalBox.height / 8));
                                }

                            GUILayout.EndVertical();
                        GUILayout.EndHorizontal();


                    GUILayout.EndArea(); // End of GeneralSettings Tab
                    #endregion

            GUILayout.EndArea();
            #endregion // End of Second Tab

        GUILayout.EndArea();
        #endregion
    }

    #region Features
    /// <summary>
    /// Change Maximum function , when we change the size
    /// and click Change Maximum button in Editor, it will update
    /// and change the size while creating new data.
    /// </summary>
    /// <param name="size">get size from enemySize</param>

    int counter = 0;
    private void ChangeMaximumPrivate(int size)
    {
        enemySize = enemySizeTemp;
        //This count only useful when we doesn't have a name yet.
        //you can remove this when decide a new format later.
        while (counter <= enemySize)
        {
            enemy.Add(ScriptableObject.CreateInstance<EnemyData>());
            AssetDatabase.CreateAsset(enemy[counter], "Assets/Resources/Data/EnemyData/Enemy_" + counter + ".asset");
            AssetDatabase.SaveAssets();
            enemyDisplayName.Add(enemy[counter].enemyName);
            counter++;
        }
        if (counter > enemySize)
        {
            enemy.RemoveRange(enemySize, enemy.Count - enemySize);
            enemyDisplayName.RemoveRange(enemySize, enemyDisplayName.Count - enemySize);
            for (int i = enemySize; i <= counter; i++)
            {
                AssetDatabase.DeleteAsset("Assets/Resources/Data/EnemyData/Enemy_" + i + ".asset");
            }
            AssetDatabase.SaveAssets();
            counter = enemySize;
        }
    }

    public override void ItemTabLoader(int index)
    {
        Debug.Log(index + "index");
        Texture2D defTex = new Texture2D(256, 256);
        if (index != -1)
        {
            if (enemySize > 0)
            {
                if (enemy[index].Image == null)
                    enemyImage = defTex;
                else
                    enemyImage = TextureToSprite(enemy[index].Image);
            }
        }
    }
    #endregion
}
