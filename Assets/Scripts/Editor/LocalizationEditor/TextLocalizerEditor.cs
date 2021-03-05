using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RemorseEditor.Localization
{
    public class TextLocalizerEditor : EditorWindow
    {
        public static void Open(string key)
        {
            TextLocalizerEditor window = (TextLocalizerEditor)CreateInstance(typeof(TextLocalizerEditor));
            window.titleContent = new GUIContent("Localizer Window");
            window.ShowUtility();
            window.key = key;
        }

        public string key;
        public string english;
        public string indonesia;

        public void OnGUI()
        {
            key = EditorGUILayout.TextField("Key :", key);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("English :", GUILayout.MaxWidth(70));

            EditorStyles.textArea.wordWrap = true;
            english = EditorGUILayout.TextArea(english, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Indonesia :", GUILayout.MaxWidth(70));

            EditorStyles.textArea.wordWrap = true;
            indonesia = EditorGUILayout.TextArea(indonesia, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
            EditorGUILayout.EndHorizontal();

            string[] values = { english, indonesia };

            if (GUILayout.Button("Add"))
            {
                if (Remorse.Localize.Localization.GetLocalizedValue(key) != string.Empty)
                {
                    Remorse.Localize.Localization.Replace(key, values);
                }
                else
                {
                    Remorse.Localize.Localization.Add(key, values);
                }
            }

            minSize = new Vector2(460, 250);
            maxSize = minSize;
        }
    }

    public class TextLocalizerSearchWindow : EditorWindow
    {
        public static void Open()
        {
            TextLocalizerSearchWindow window = (TextLocalizerSearchWindow)CreateInstance(typeof(TextLocalizerSearchWindow));
            window.titleContent = new GUIContent("Localization Search");

            Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Rect r = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
            window.ShowAsDropDown(r, new Vector2(500, 300));
        }

        public string value = "";
        public Vector2 scroll;
        public Dictionary<string, string> dictionary;

        private void OnEnable()
        {
            dictionary = Remorse.Localize.Localization.GetDictionaryForEditor();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
            value = EditorGUILayout.TextField(value);
            EditorGUILayout.EndHorizontal();

            GetSearchResult();
        }

        private void GetSearchResult()
        {
            if (value == null) return;

            EditorGUILayout.BeginVertical();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (KeyValuePair<string, string> element in dictionary)
            {
                if (element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower()))
                {
                    EditorGUILayout.BeginHorizontal("Box");
                    Texture closeIcon = (Texture)Resources.Load("Close");

                    GUIContent content = new GUIContent(closeIcon);

                    if (GUILayout.Button(content, GUILayout.MaxHeight(20), GUILayout.MaxWidth(20)))
                    {
                        if (EditorUtility.DisplayDialog("Remove Key" + element.Key + "?", "This will remove the element of localization, are you sure?", "Yes"))
                        {
                            Remorse.Localize.Localization.Remove(element.Key);
                            AssetDatabase.Refresh();
                            Remorse.Localize.Localization.Init();
                            dictionary = Remorse.Localize.Localization.GetDictionaryForEditor();
                        }
                    }

                    EditorGUILayout.TextField(element.Key);
                    EditorGUILayout.LabelField(element.Value);
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}