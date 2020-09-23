using UnityEngine;
using UnityEditor;

public class ClassExpWindow : EditorWindow {

    GUIStyle classStyle;
    GUIStyle tabStyle;
    GUIStyle columnStyle;

    static ClassesData thisClass;
    public static void ShowWindow(ClassesData classData) {
        var window = GetWindow<ClassExpWindow>();
        //Sizing
        window.maxSize = new Vector2(810,540);
        window.minSize = new Vector2(810,540);
        window.titleContent = new GUIContent("Exp Curve");
        thisClass = classData;
        window.Show();
    }

    private void OnGUI() {

        //Styling
        classStyle = new GUIStyle(GUI.skin.box);
        classStyle.normal.background = CreateTexture(1, 1, Color.gray);
        columnStyle = new GUIStyle(GUI.skin.box);
        columnStyle.normal.background = CreateTexture(1, 1, new Color32(70, 70, 70, 200));
        tabStyle = new GUIStyle(GUI.skin.box);
        if(EditorGUIUtility.isProSkin)
        tabStyle.normal.background = CreateTexture(1, 1, new Color32(76, 76, 76, 100));
        else
        tabStyle.normal.background = CreateTexture(1,1, new Color32(200,200,200,100));


        //ExpCurve Window
        Rect expCurveBox = new Rect(0,0, position.width, position.height);
        GUILayout.BeginArea(expCurveBox, columnStyle);
            GUILayout.BeginVertical();
                GUILayout.Space(5);
                GUILayout.Label("CURVE IS READ ONLY, DO NOT EDIT VALUE IN CURVE EDITOR", EditorStyles.boldLabel);
                EditorGUILayout.CurveField(thisClass.expCurve, GUILayout.Height(380));
                GUI.BeginGroup(new Rect(0,25, position.width, 380), tabStyle);
                    GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                            // for(int i=0; i < 20;i++)
                            // {
                            //     GUILayout.Label("L "+i.ToString() + ":     " + thisClass.getExp(i+1).ToString());
                            // }
                        GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                GUI.EndGroup();          
                GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                        GUILayout.Label("Base Value:");
                        GUILayout.BeginHorizontal();
                            thisClass.baseValue = Mathf.RoundToInt(GUILayout.HorizontalSlider(thisClass.baseValue, 10f, 50f, GUILayout.Width(200f), GUILayout.Height(20f)));
                            thisClass.baseValue = EditorGUILayout.IntField(thisClass.baseValue, GUILayout.Width(50), GUILayout.Height(20));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Label("Acceleration A");
                        GUILayout.BeginHorizontal();
                            thisClass.accelA = Mathf.RoundToInt(GUILayout.HorizontalSlider(thisClass.accelA, 10, 50, GUILayout.Width(200), GUILayout.Height(20f)));
                            thisClass.accelA = EditorGUILayout.IntField(thisClass.accelA, GUILayout.Width(50), GUILayout.Height(20));
                        GUILayout.EndHorizontal();
                    GUILayout.EndVertical(); 
                    GUILayout.BeginVertical(); 
                        GUILayout.Label("Extra Value:");
                        GUILayout.BeginHorizontal();
                            thisClass.extraValue = Mathf.RoundToInt(GUILayout.HorizontalSlider(thisClass.extraValue, 0,40, GUILayout.Width(200), GUILayout.Height(20f)));
                            thisClass.extraValue = EditorGUILayout.IntField(thisClass.extraValue, GUILayout.Width(50), GUILayout.Height(20));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Label("Acceleration B: ");
                        GUILayout.BeginHorizontal();
                            thisClass.accelB = Mathf.RoundToInt(GUILayout.HorizontalSlider(thisClass.accelB, 10, 50, GUILayout.Width(200), GUILayout.Height(20f)));
                            thisClass.accelB = EditorGUILayout.IntField(thisClass.accelB, GUILayout.Width(50), GUILayout.Height(20));
                        GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                if(GUILayout.Button("Update"))
                {
                    UpdateCurve();
                }
            GUILayout.EndVertical();
        GUILayout.EndArea();
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
            Keyframe key = new Keyframe(i, thisClass.getExp(i+1));
            key.inTangent = float.PositiveInfinity;
            key.outTangent = float.NegativeInfinity;
            AnimationUtility.SetKeyLeftTangentMode(thisClass.expCurve, i, AnimationUtility.TangentMode.Constant);
            thisClass.expCurve.MoveKey(i,key);
        }
    }
}
