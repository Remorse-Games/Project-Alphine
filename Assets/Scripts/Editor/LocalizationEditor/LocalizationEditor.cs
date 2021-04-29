using UnityEngine;
using UnityEditor;
using Remorse.Localize;
using System.Collections.Generic;

public class LocalizationEditor : EditorWindow
{
    #region Variabel Declaration
    public static CSVLoader csvLoader = new CSVLoader();
    public static LocalizationLanguage localizationLanguage = new LocalizationLanguage();
    
    public static TextAsset csvFile;

    public static List<string> languages;

    Vector2 scrollPos;

    string newLanguage;
    int index;
    #endregion

    #region Action State
    enum State
    {
        Add,
        Edit
    }
    State state = State.Add;
    #endregion

    [MenuItem("Remorse/Localization/Language")]
    static void Init()
    {
        csvLoader.LoadCSV();
        string[] headers = csvLoader.GetCSVHeaders();
        languages = new List<string>(headers);


        LocalizationEditor window = (LocalizationEditor)EditorWindow.GetWindow(typeof(LocalizationEditor));
        window.Show();
    }
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Language");

        Color tempColor1 = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add", GUILayout.Width(50)))
        {
            state = State.Add;
            newLanguage = "";
            index = 0;
        }
        GUI.backgroundColor = tempColor1;
        GUILayout.EndHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(100));
        LoadLanguagesList();
        EditorGUILayout.EndScrollView();

        newLanguage = EditorGUILayout.TextField(newLanguage);
        switch (state)
        {
            case State.Add:
                tempColor1 = GUI.backgroundColor;
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Add New Language"))
                {
                    Init();
                    localizationLanguage.AddLanguage(newLanguage);
                    Init();
                }
                GUI.backgroundColor = tempColor1;
                break;
            case State.Edit:
                tempColor1 = GUI.backgroundColor;
                GUI.backgroundColor = Color.magenta;
                if (GUILayout.Button("Edit"))
                {
                    Init();
                    localizationLanguage.EditLanguage(index, newLanguage);
                    Init();
                }
                GUI.backgroundColor = tempColor1;
                break;
        }    
    }
    void LoadLanguagesList()
    {
        foreach (string language in languages)
        {
            if (language == languages[0]) continue;

            string languageId = "";
            foreach (char i in language)
            {
                if (i == '"') continue;

                languageId += i;
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(languageId, GUILayout.Height(20)))
            {
                newLanguage = languageId;
                index = languages.IndexOf(language);
                state = State.Edit;
            }

            Color tempColor1 = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Delete", GUILayout.Height(20), GUILayout.Width(50)))
            {
                Init();
                localizationLanguage.RemoveLanguage(languages.IndexOf(language));
                Init();
                state = State.Add;
                newLanguage = "";
                index = 0;
            }
            GUI.backgroundColor = tempColor1;
            GUILayout.EndHorizontal();
        }
    }
}
