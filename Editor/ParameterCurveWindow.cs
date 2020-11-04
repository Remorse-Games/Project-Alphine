using UnityEngine;
using UnityEditor;

#region HP Window
//Show max hp window
public class MaxHPWindow : EditorWindow
{
    ClassesData thisClass;
    int levelIndex;
    int editedKeyframeValue;

    int genCurveStartValue;
    int genCurveEndValue;
    int genCurveGrowthRate;
    public MaxHPWindow(ClassesData classData)
    {
        var window = GetWindow<MaxHPWindow>();
        window.maxSize = new Vector2(810, 540);
        window.minSize = new Vector2(810, 540);
        window.titleContent = new GUIContent("MaxHPWindow");
        thisClass = classData;
        levelIndex = 1;
        editedKeyframeValue = Mathf.RoundToInt(thisClass.maxHPCurve.keys[levelIndex].value);
        window.Show();
    }



    private void OnGUI()
    {
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
        if (GUILayout.Button("Update Value", GUILayout.Width(100), GUILayout.Height(50)))
        {
            thisClass.maxHPCurve.MoveKey(levelIndex, new Keyframe(levelIndex, editedKeyframeValue));
        }
        GUILayout.Space(1000);
        GUILayout.EndHorizontal();
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.CurveField(thisClass.maxHPCurve, new Color32(255, 150, 0, 255), new Rect(0, 0, 100, 9999), GUILayout.Height(300), GUILayout.Width(800));
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
        GUILayout.Space(800);
        GUILayout.EndHorizontal();
        if (GUI.Button(new Rect(200, 500, 200, 50), "Generate Curve!"))
        {
            MakeCurve(genCurveStartValue, genCurveEndValue, genCurveGrowthRate);
        }
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
        for (int i = 0; i < 100; i++)
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
        thisClass.growthRateHP = rate;
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.maxHPCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.maxHPCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.maxHPCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.maxHPCurve.MoveKey(i, new Keyframe(i, thisClass.GetCurveValue(i, thisClass.minHP, thisClass.maxHP, thisClass.growthRateHP)));
        }
    }
}
#endregion

#region MP Window
public class MaxMPWindow : EditorWindow
{
    ClassesData thisClass;
    int levelIndex;
    int editedKeyframeValue;

    int genCurveStartValue;
    int genCurveEndValue;
    int genCurveGrowthRate;
    public MaxMPWindow(ClassesData classData)
    {
        var window = GetWindow<MaxMPWindow>();
        window.titleContent = new GUIContent("Max MP");
        window.minSize = new Vector2(810, 540);
        window.maxSize = new Vector2(810, 540);
        thisClass = classData;
        levelIndex = 1;
        editedKeyframeValue = Mathf.RoundToInt(thisClass.maxMPCurve.keys[levelIndex].value);
        window.Show();
    }

    private void OnGUI()
    {
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
        if (GUILayout.Button("Update Value", GUILayout.Width(100), GUILayout.Height(50)))
        {
            thisClass.maxMPCurve.MoveKey(levelIndex, new Keyframe(levelIndex, editedKeyframeValue));
        }
        GUILayout.Space(1000);
        GUILayout.EndHorizontal();
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.CurveField(thisClass.maxMPCurve, new Color32(0, 0, 255, 255), new Rect(0, 0, 100, 2000), GUILayout.Height(300), GUILayout.Width(800));
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
        GUILayout.Space(800);
        GUILayout.EndHorizontal();
        if (GUI.Button(new Rect(200, 500, 200, 50), "Generate Curve!"))
        {
            MakeCurve(genCurveStartValue, genCurveEndValue, genCurveGrowthRate);
        }
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
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.maxMPCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.maxMPCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.maxMPCurve, i, AnimationUtility.TangentMode.Constant);
        }
    }

    private void MakeCurve(int start, int end, int rate)
    {
        thisClass.minMP = start;
        thisClass.maxMP = end;
        thisClass.growthRateMP = rate;
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.maxMPCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.maxMPCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.maxMPCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.maxMPCurve.MoveKey(i, new Keyframe(i, thisClass.GetCurveValue(i, thisClass.minMP, thisClass.maxMP, thisClass.growthRateMP)));
        }
    }
}
#endregion

#region Attack Window
public class AttackWindow : EditorWindow
{
    ClassesData thisClass;
    int levelIndex;
    int editedKeyframeValue;

    int genCurveStartValue;
    int genCurveEndValue;
    int genCurveGrowthRate;
    public AttackWindow(ClassesData classData)
    {
        var window = GetWindow<AttackWindow>();
        window.titleContent = new GUIContent("Attack");
        window.minSize = new Vector2(810, 540);
        window.maxSize = new Vector2(810, 540);
        thisClass = classData;
        levelIndex = 1;
        editedKeyframeValue = Mathf.RoundToInt(thisClass.AttackCurve.keys[levelIndex].value);
        window.Show();
    }

    private void OnGUI()
    {
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
        if (GUILayout.Button("Update Value", GUILayout.Width(100), GUILayout.Height(50)))
        {
            thisClass.AttackCurve.MoveKey(levelIndex, new Keyframe(levelIndex, editedKeyframeValue));
        }
        GUILayout.Space(1000);
        GUILayout.EndHorizontal();
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.CurveField(thisClass.AttackCurve, new Color32(255, 0, 0, 255), new Rect(0, 0, 100, 250), GUILayout.Height(300), GUILayout.Width(800));
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
        GUILayout.Space(800);
        GUILayout.EndHorizontal();
        if (GUI.Button(new Rect(200, 500, 200, 50), "Generate Curve!"))
        {
            MakeCurve(genCurveStartValue, genCurveEndValue, genCurveGrowthRate);
        }
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
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.AttackCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.AttackCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.AttackCurve, i, AnimationUtility.TangentMode.Constant);
        }
    }

    private void MakeCurve(int start, int end, int rate)
    {
        thisClass.minAtk = start;
        thisClass.maxAtk = end;
        thisClass.growthRateAtk = rate;
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.AttackCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.AttackCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.AttackCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.AttackCurve.MoveKey(i, new Keyframe(i, thisClass.GetCurveValue(i, thisClass.minAtk, thisClass.maxAtk, thisClass.growthRateAtk)));
        }
    }
}
#endregion

#region Defense Window
public class DefenseWindow : EditorWindow
{
    ClassesData thisClass;
    int levelIndex;
    int editedKeyframeValue;

    int genCurveStartValue;
    int genCurveEndValue;
    int genCurveGrowthRate;
    public DefenseWindow(ClassesData classData)
    {
        var window = GetWindow<DefenseWindow>();
        window.titleContent = new GUIContent("Defense");
        window.minSize = new Vector2(810, 540);
        window.maxSize = new Vector2(810, 540);
        thisClass = classData;
        levelIndex = 1;
        editedKeyframeValue = Mathf.RoundToInt(thisClass.DefenseCurve.keys[levelIndex].value);
        window.Show();
    }

    private void OnGUI()
    {
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
        if (GUILayout.Button("Update Value", GUILayout.Width(100), GUILayout.Height(50)))
        {
            thisClass.DefenseCurve.MoveKey(levelIndex, new Keyframe(levelIndex, editedKeyframeValue));
        }
        GUILayout.Space(1000);
        GUILayout.EndHorizontal();
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.CurveField(thisClass.DefenseCurve, new Color32(0, 255, 0, 255), new Rect(0, 0, 100, 250), GUILayout.Height(300), GUILayout.Width(800));
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
        GUILayout.Space(800);
        GUILayout.EndHorizontal();
        if (GUI.Button(new Rect(200, 500, 200, 50), "Generate Curve!"))
        {
            MakeCurve(genCurveStartValue, genCurveEndValue, genCurveGrowthRate);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void UpdateCurve()
    {
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.DefenseCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.DefenseCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.DefenseCurve, i, AnimationUtility.TangentMode.Constant);
        }
    }

    private void MakeCurve(int start, int end, int rate)
    {
        thisClass.minDef = start;
        thisClass.maxDef = end;
        thisClass.grwothRateDef = rate;
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.DefenseCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.DefenseCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.DefenseCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.DefenseCurve.MoveKey(i, new Keyframe(i, thisClass.GetCurveValue(i, thisClass.minDef, thisClass.maxDef, thisClass.grwothRateDef)));
        }
    }
}
#endregion

#region MAttack Window
public class MAttackWindow : EditorWindow
{
    ClassesData thisClass;
    int levelIndex;
    int editedKeyframeValue;

    int genCurveStartValue;
    int genCurveEndValue;
    int genCurveGrowthRate;
    public MAttackWindow(ClassesData classData)
    {
        var window = GetWindow<MAttackWindow>();
        window.titleContent = new GUIContent("M.Attack");
        window.minSize = new Vector2(810, 540);
        window.maxSize = new Vector2(810, 540);
        thisClass = classData;
        levelIndex = 1;
        editedKeyframeValue = Mathf.RoundToInt(thisClass.mAttackCurve.keys[levelIndex].value);
        window.Show();
    }

    private void OnGUI()
    {
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
        if (GUILayout.Button("Update Value", GUILayout.Width(100), GUILayout.Height(50)))
        {
            thisClass.mAttackCurve.MoveKey(levelIndex, new Keyframe(levelIndex, editedKeyframeValue));
        }
        GUILayout.Space(1000);
        GUILayout.EndHorizontal();
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.CurveField(thisClass.mAttackCurve, new Color32(255, 0, 255, 255), new Rect(0, 0, 100, 250), GUILayout.Height(300), GUILayout.Width(800));
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
        GUILayout.Space(800);
        GUILayout.EndHorizontal();
        if (GUI.Button(new Rect(200, 500, 200, 50), "Generate Curve!"))
        {
            MakeCurve(genCurveStartValue, genCurveEndValue, genCurveGrowthRate);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void UpdateCurve()
    {
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.mAttackCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.mAttackCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.mAttackCurve, i, AnimationUtility.TangentMode.Constant);
        }
    }

    private void MakeCurve(int start, int end, int rate)
    {
        thisClass.minMAtk = start;
        thisClass.maxMAtk = end;
        thisClass.growthRateMAtk = rate;
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.mAttackCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.mAttackCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.mAttackCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.mAttackCurve.MoveKey(i, new Keyframe(i, thisClass.GetCurveValue(i, thisClass.minMAtk, thisClass.maxMAtk, thisClass.growthRateMAtk)));
        }
    }
}
#endregion

#region MDefense Window
public class MDefenseWindow : EditorWindow
{
    ClassesData thisClass;
    int levelIndex;
    int editedKeyframeValue;

    int genCurveStartValue;
    int genCurveEndValue;
    int genCurveGrowthRate;
    public MDefenseWindow(ClassesData classData)
    {
        var window = GetWindow<MDefenseWindow>();
        window.titleContent = new GUIContent("M.Defense");
        window.minSize = new Vector2(810, 540);
        window.maxSize = new Vector2(810, 540);
        thisClass = classData;
        levelIndex = 1;
        editedKeyframeValue = Mathf.RoundToInt(thisClass.mDefenseCurve.keys[levelIndex].value);
        window.Show();
    }

    private void OnGUI()
    {
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
        if (GUILayout.Button("Update Value", GUILayout.Width(100), GUILayout.Height(50)))
        {
            thisClass.mDefenseCurve.MoveKey(levelIndex, new Keyframe(levelIndex, editedKeyframeValue));
        }
        GUILayout.Space(1000);
        GUILayout.EndHorizontal();
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.CurveField(thisClass.mDefenseCurve, new Color32(11, 156, 49, 255), new Rect(0, 0, 100, 250), GUILayout.Height(300), GUILayout.Width(800));
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
        GUILayout.Space(800);
        GUILayout.EndHorizontal();
        if (GUI.Button(new Rect(200, 500, 200, 50), "Generate Curve!"))
        {
            MakeCurve(genCurveStartValue, genCurveEndValue, genCurveGrowthRate);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void UpdateCurve()
    {
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.mDefenseCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.mDefenseCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.mDefenseCurve, i, AnimationUtility.TangentMode.Constant);
        }
    }

    private void MakeCurve(int start, int end, int rate)
    {
        thisClass.minMDef = start;
        thisClass.maxMDef = end;
        thisClass.growthRateMDef = rate;
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.mDefenseCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.mDefenseCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.mDefenseCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.mDefenseCurve.MoveKey(i, new Keyframe(i, thisClass.GetCurveValue(i, thisClass.minMDef, thisClass.maxMDef, thisClass.growthRateMDef)));
        }
    }
}
#endregion

#region Agility Window
public class AgilityWindow : EditorWindow
{
    ClassesData thisClass;
    int levelIndex;
    int editedKeyframeValue;

    int genCurveStartValue;
    int genCurveEndValue;
    int genCurveGrowthRate;
    public AgilityWindow(ClassesData classData)
    {
        var window = GetWindow<AgilityWindow>();
        window.titleContent = new GUIContent("Agility");
        window.minSize = new Vector2(810, 540);
        window.maxSize = new Vector2(810, 540);
        thisClass = classData;
        levelIndex = 1;
        editedKeyframeValue = Mathf.RoundToInt(thisClass.agilityCurve.keys[levelIndex].value);
        window.Show();
    }

    private void OnGUI()
    {
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
        if (GUILayout.Button("Update Value", GUILayout.Width(100), GUILayout.Height(50)))
        {
            thisClass.agilityCurve.MoveKey(levelIndex, new Keyframe(levelIndex, editedKeyframeValue));
        }
        GUILayout.Space(1000);
        GUILayout.EndHorizontal();
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.CurveField(thisClass.agilityCurve, new Color32(0, 255, 255, 255), new Rect(0, 0, 100, 500), GUILayout.Height(300), GUILayout.Width(800));
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
        GUILayout.Space(800);
        GUILayout.EndHorizontal();
        if (GUI.Button(new Rect(200, 500, 200, 50), "Generate Curve!"))
        {
            MakeCurve(genCurveStartValue, genCurveEndValue, genCurveGrowthRate);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void UpdateCurve()
    {
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.agilityCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.agilityCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.agilityCurve, i, AnimationUtility.TangentMode.Constant);
        }
    }

    private void MakeCurve(int start, int end, int rate)
    {
        thisClass.minAgi = start;
        thisClass.maxAgi = end;
        thisClass.growthRateAgi = rate;
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.agilityCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.agilityCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.agilityCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.agilityCurve.MoveKey(i, new Keyframe(i, thisClass.GetCurveValue(i, thisClass.minAgi, thisClass.maxAgi, thisClass.growthRateAgi)));
        }
    }
}
#endregion

#region Luck Window
public class LuckWindow : EditorWindow
{
    ClassesData thisClass;
    int levelIndex;
    int editedKeyframeValue;

    int genCurveStartValue;
    int genCurveEndValue;
    int genCurveGrowthRate;
    public LuckWindow(ClassesData classData)
    {
        var window = GetWindow<LuckWindow>();
        window.titleContent = new GUIContent("Luck");
        window.minSize = new Vector2(810, 540);
        window.maxSize = new Vector2(810, 540);
        thisClass = classData;
        levelIndex = 1;
        editedKeyframeValue = Mathf.RoundToInt(thisClass.luckCurve.keys[levelIndex].value);
        window.Show();
    }

    private void OnGUI()
    {
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
        if (GUILayout.Button("Update Value", GUILayout.Width(100), GUILayout.Height(50)))
        {
            thisClass.luckCurve.MoveKey(levelIndex, new Keyframe(levelIndex, editedKeyframeValue));
        }
        GUILayout.Space(1000);
        GUILayout.EndHorizontal();
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.CurveField(thisClass.luckCurve, new Color32(255, 200, 0, 255), new Rect(0, 0, 100, 500), GUILayout.Height(300), GUILayout.Width(800));
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
        GUILayout.Space(800);
        GUILayout.EndHorizontal();
        if (GUI.Button(new Rect(200, 500, 200, 50), "Generate Curve!"))
        {
            MakeCurve(genCurveStartValue, genCurveEndValue, genCurveGrowthRate);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void UpdateCurve()
    {
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.luckCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.luckCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.luckCurve, i, AnimationUtility.TangentMode.Constant);
        }
    }

    private void MakeCurve(int start, int end, int rate)
    {
        thisClass.minLuck = start;
        thisClass.maxLuck = end;
        thisClass.growthRateLuck = rate;
        for (int i = 0; i < 100; i++)
        {
            Keyframe key = thisClass.luckCurve.keys[i];
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.luckCurve, i, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(thisClass.luckCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.luckCurve.MoveKey(i, new Keyframe(i, thisClass.GetCurveValue(i, thisClass.minLuck, thisClass.maxLuck, thisClass.growthRateLuck)));
        }
    }
}
#endregion