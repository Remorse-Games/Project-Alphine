using UnityEngine;
using UnityEditor;


//Show max hp window
public class MaxHPWindow : EditorWindow 
{
    //Styling, global
    GUIStyle classStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    ClassesData thisClass;
    int levelIndex;
    int editedKeyframeValue;

    int genCurveStartValue;
    int genCurveEndValue;
    int genCurveGrowthRate;
    public MaxHPWindow(ClassesData classData) 
    {
        var window = GetWindow<MaxHPWindow>();
        window.maxSize = new Vector2(810,540);
        window.minSize = new Vector2(810,540);
        window.titleContent = new GUIContent("MaxHPWindow");
        thisClass = classData;
        levelIndex = 1;
        editedKeyframeValue = Mathf.RoundToInt(thisClass.maxHPCurve.keys[levelIndex].value);
        window.Show();
    }



    private void OnGUI() 
    {
        classStyle = new GUIStyle(GUI.skin.box);
        classStyle.normal.background = CreateTexture(1, 1, Color.gray);
        columnStyle = new GUIStyle(GUI.skin.box);
        columnStyle.normal.background = CreateTexture(1, 1, new Color32(70, 70, 70, 200));
        tabStyle = new GUIStyle(GUI.skin.box);
        if(EditorGUIUtility.isProSkin)
        tabStyle.normal.background = CreateTexture(1, 1, new Color32(150, 150, 150, 100));
        else
        tabStyle.normal.background = CreateTexture(1,1, new Color32(200,200,200,100));

        UpdateCurve();

        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                    GUILayout.Label("Level: ");
                    levelIndex = EditorGUILayout.IntField(levelIndex, GUILayout.Width(50), GUILayout.Height(30));
                GUILayout.EndVertical();
                GUILayout.Space(20);
                GUILayout.Label(" \n\n > ");
                GUILayout.BeginVertical();
                    GUILayout.Label("Value: ");
                    editedKeyframeValue = EditorGUILayout.IntField(editedKeyframeValue, GUILayout.Width(50), GUILayout.Height(30));
                GUILayout.EndVertical();
                GUILayout.Space(30);
                if(GUILayout.Button("Update Value", GUILayout.Width(100), GUILayout.Height(50)))
                {
                    thisClass.maxHPCurve.MoveKey(levelIndex, new Keyframe(levelIndex, editedKeyframeValue));
                }
                GUILayout.Space(1000);
            GUILayout.EndHorizontal();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.CurveField(thisClass.maxHPCurve, new Color32(208, 128, 96, 255) ,new Rect(0,0,100,9999), GUILayout.Height(300), GUILayout.Width(800));
            }
            GUILayout.Label("Generate Curve: ");
            GUILayout.Space(10);
            GUILayout.Label("Endpoint Values", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                GUILayout.BeginVertical();
                    GUILayout.Label("Level 1:");
                    genCurveStartValue = EditorGUILayout.IntField(genCurveStartValue, GUILayout.Height(30), GUILayout.Width(50));
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                    GUILayout.Label("Level 100:");
                    genCurveEndValue = EditorGUILayout.IntField(genCurveEndValue, GUILayout.Height(30), GUILayout.Width(50));
                GUILayout.EndVertical();
                GUILayout.Space(600);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Growth Type", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                GUILayout.BeginVertical();
                    genCurveGrowthRate = Mathf.RoundToInt(GUILayout.HorizontalSlider(genCurveGrowthRate, -1, 1, GUILayout.Width(150), GUILayout.Height(30)));
                    Debug.Log(genCurveGrowthRate);
                        GUILayout.BeginHorizontal();
                            GUILayout.Label("Fast");
                            GUILayout.Space(15);
                            GUILayout.Label("Normal");
                            GUILayout.Space(20);
                            GUILayout.Label("Slow");
                            GUILayout.Space(50);
                            if(GUILayout.Button("Generate Curve!", GUILayout.Height(30), GUILayout.Width(150)))
                            {
                                MakeCurve(genCurveStartValue, genCurveEndValue, genCurveGrowthRate);
                            }
                            GUILayout.Space(500);
                        GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }


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

    private void UpdateCurve()
    {
        for(int i=0;i<100;i++)
        {
            Keyframe key = thisClass.maxHPCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.maxHPCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.maxHPCurve, i, AnimationUtility.TangentMode.Constant);
        }
    }

    private void MakeCurve(int start, int end, int rate)
    {
        thisClass.minHP = start;
        thisClass.maxHP = end;
        thisClass.growthRate = rate;
        for(int i=0;i<100;i++)
        {
            Keyframe key = thisClass.maxHPCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.maxHPCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.maxHPCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.maxHPCurve.MoveKey(i, new Keyframe(i, thisClass.getMaxHP(i+1)));
        }
    }
}