using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Remorse.Localize
{
    public class LocalizationLanguage
    {
        CSVLoader csvLoader = new CSVLoader();
        List<List<string>> CSV;

        public void AddLanguage(string newLanguage)
        {
            CSV = csvLoader.getListCSV();
            
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
         
            File.WriteAllLines(csvLoader.getCSVPath(), CSV.Select(x => string.Join(",", x)));
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

            File.WriteAllLines(csvLoader.getCSVPath(), CSV.Select(x => string.Join(",", x)));
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

            File.WriteAllLines(csvLoader.getCSVPath(), CSV.Select(x => string.Join(",", x)));
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}
