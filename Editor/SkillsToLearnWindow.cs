using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Runtime.CompilerServices;
using System.IO;
using SFB;
using System.Linq;

public class SkillsToLearnWindow : EditorWindow
{
    //Data(s) reference
    static List<ActorTraitsData> traits;
    public static void ShowWindow(List<ActorTraitsData> actorTraitData, int index, int size)
    {
        var window = GetWindow<ActorTraitWindow>();
        var position = window.position;
        //Sizing
        window.maxSize = new Vector2(500, 190);
        window.minSize = new Vector2(500, 190);
        window.titleContent = new GUIContent("Skills To Learn");
        position.center = new Rect(Screen.width * -1 * .05f, Screen.height * -1 * .05f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        window.position = position;
        window.Show();
    }
}
