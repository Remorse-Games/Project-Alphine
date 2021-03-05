using UnityEngine;
using UnityEditor;
using Remorse.Localize;
using System.Collections.Generic;

public class LocalizationEditor : EditorWindow
{
    public static CSVLoader csvLoader = new CSVLoader();
    public static TextAsset csvFile;
    public static List<string> languages;
    public static string[] lines;

    [MenuItem("Remorse/Localization")]
    static void Init()
    {
        csvLoader.LoadCSV();
        string[] headers = csvLoader.GetLanguages();
        languages = new List<string>(headers);

        LocalizationEditor window = (LocalizationEditor)EditorWindow.GetWindow(typeof(LocalizationEditor));
        window.Show();
    }
    void OnGUI()
    {
        if (GUILayout.Button("Add", GUILayout.Height(20)))
        {
            csvLoader.AddLanguage();
            Init();
        }

        foreach (string language in languages)
        {
            if (language == languages[0]) continue;
            string languageBtn = "";
            foreach(char l in language)
            {
                if(l == '"') continue;

                languageBtn += l;
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(languageBtn, GUILayout.Width(150) ,GUILayout.Height(20)))
            {
                Debug.Log(languages.IndexOf(language));
            }
            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
            {
                Debug.Log(languages.IndexOf(language));
            }
            GUILayout.EndHorizontal();
        }
    }
}
