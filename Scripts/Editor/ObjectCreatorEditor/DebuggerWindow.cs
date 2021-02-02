using UnityEngine;
using UnityEditor;

namespace LastBossEditor.Creator
{
    public class DebuggerWindow : EditorWindow
    {
        public string texting;
        [MenuItem("Alife/Debugger Window")]
        public static void ShowWindow()
        {
            GetWindow<DebuggerWindow>("Debugger Tools");
        }

        private void OnGUI()
        {
            GUILayout.TextField("Text", texting);
            EditorGUILayout.TextField("Text ", texting);
        }
    }
}
