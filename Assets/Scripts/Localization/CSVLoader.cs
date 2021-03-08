using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using UnityEditor;
namespace Remorse.Localize
{
    public class CSVLoader
    {
        private TextAsset csvFile;
        private char lineSeparator = '\n';
        private char surround = '"';
        private string[] fieldSeparator = { "\",\"" };

        private string csvPath = "Assets/Resources/localization.csv";
        string[] CSVDump;
        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        List<List<string>> CSV;
        Vector2 scrollPos;

        public void LoadCSV()
        {
            csvFile = Resources.Load<TextAsset>("localization");
            CSVDump = File.ReadAllLines(csvPath);
            CSV = CSVDump.Select(x => CSVParser.Split(x).ToList()).ToList();
        }

        public Dictionary<string, string> GetDictionaryValues(string attributeId)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] lines = csvFile.text.Split(lineSeparator);
            int attributeIndex = -1;
            string[] headers = lines[0].Split(fieldSeparator, StringSplitOptions.None);

            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Contains(attributeId))
                {
                    attributeIndex = i;
                    break;
                }
            }

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = CSVParser.Split(line);

                for (int f = 0; f < fields.Length; f++)
                {
                    fields[f] = fields[f].TrimStart(' ', surround);
                    fields[f] = fields[f].TrimEnd('\r',surround);
                }

                if (fields.Length > attributeIndex)
                {
                    var key = fields[0];
                    if (dictionary.ContainsKey(key)) { continue; }
                    var value = fields[attributeIndex];
                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }

        public void Add(string key, string[] values)
        {
            string append = string.Format("\n\"{0}\"", key);
            foreach(string languageValue in values)
            {
                append += string.Format(",\"{0}\"", languageValue);
            }
            File.AppendAllText(csvPath, append);

            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }

        public void Remove(string key)
        {
            string[] lines = csvFile.text.Split(lineSeparator);
            string[] keys = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                keys[i] = line.Split(fieldSeparator, StringSplitOptions.None)[0];
            }

            int index = -1;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].Contains(key))
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                string[] newLines;
                newLines = lines.Where(w => w != lines[index]).ToArray();
               
                string replaced = string.Join(lineSeparator.ToString(), newLines);
                Debug.Log(replaced);
                File.WriteAllText(csvPath, replaced);
            }
        }

        public void Edit(string key, string[] values)
        {
            Remove(key);
            Add(key, values);
        }

        public string[] GetCSVHeaders()
        {
            string[] lines = csvFile.text.Split(lineSeparator);
            string[] headers = lines[0].Split(fieldSeparator, StringSplitOptions.None);
            
            for(int i = 0; i < headers.Length; i++)
            {
                headers[i] = headers[i].TrimStart(' ', surround);
                headers[i] = headers[i].TrimEnd('\r', surround);
            }

            return headers;
        }

        public void AddLanguage(string newLanguage)
        {
            if (string.IsNullOrEmpty(newLanguage))
            {
                Debug.Log("Input field can't be empty");
                return;
            }

            CSV[0].Insert(CSV[0].Count, string.Format("\"{0}\"", newLanguage));

            for (int i = 1; i < CSV.Count; i++)
            {
                CSV[i].Insert(CSV[i].Count, "\"\"");
            }

            File.WriteAllLines(csvPath, CSV.Select(x => string.Join(",", x)));
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public void RemoveLanguage(int langguageIndex)
        {
            for (int i = 0; i < CSV.Count; i++)
            {
                CSV[i].RemoveAt(langguageIndex);
            }

            File.WriteAllLines(csvPath, CSV.Select(x => string.Join(",", x)));
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public void EditLanguage(int langguageIndex, string newLanguage)
        {
            if (string.IsNullOrEmpty(newLanguage))
            {
                Debug.Log("language Id can't be null");
                return;
            }

            CSV[0][langguageIndex] = string.Format("\"{0}\"", newLanguage);

            File.WriteAllLines(csvPath, CSV.Select(x => string.Join(",", x)));
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}
