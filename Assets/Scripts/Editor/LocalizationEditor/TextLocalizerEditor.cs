using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Remorse.Localize;

namespace RemorseEditor.Localization
{
    public class TextLocalizerEditor : EditorWindow
    {
        public static List<string> languages;
        public static CSVLoader csvLoader = new CSVLoader();
        Vector2 scrollPos;

        public string key;
        string[] values = new string[languages.Count - 1];

        public static void Open(string key)
        {
            csvLoader.LoadCSV();
            string[] headers = csvLoader.GetCSVHeaders();
            languages = new List<string>(headers);

            TextLocalizerEditor window = (TextLocalizerEditor)CreateInstance(typeof(TextLocalizerEditor));
            window.titleContent = new GUIContent("Localizer Window");
            window.ShowUtility();
            window.key = key;

            for (int i = 1; i < languages.Count; i++)
            {
                csvLoader.GetDictionaryValues(languages[i]).TryGetValue(key, out window.values[i - 1]);
            }
        }

        public void OnGUI()
        {
            key = EditorGUILayout.TextField("Key :", key);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
            for (int i = 1; i < languages.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(languages[i] + " :", GUILayout.MaxWidth(70));

                EditorStyles.textArea.wordWrap = true;
                values[i - 1] = EditorGUILayout.TextArea(values[i - 1], EditorStyles.textArea, GUILayout.Height(100));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

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