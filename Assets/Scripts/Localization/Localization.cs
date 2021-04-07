using System.Collections.Generic;

namespace Remorse.Localize
{
    public class Localization
    {
        public static string languageId = "en";

        private static Dictionary<string, string> localizedDictionary;

        public static bool isInit;

        public static CSVLoader csvLoader;

        public static void Init()
        {
            csvLoader = new CSVLoader();
            csvLoader.LoadCSV();

            UpdateDictionary();

            isInit = true;
        }

        public static void UpdateDictionary()
        {
            localizedDictionary = csvLoader.GetDictionaryValues(languageId);
        }

        public static Dictionary<string, string> GetDictionaryForEditor()
        {
            if (!isInit) Init();

            return localizedDictionary;
        }

        public static string GetLocalizedValue(string key)
        {
            if (!isInit) Init();
            string value;
            localizedDictionary.TryGetValue(key, out value);
            return value;
        }

        public static void Add(string key, string[] values)
        {
            foreach(string value in values)
            {
                if (value.Contains("\""))
                {
                    value.Replace('"', '\"');
                }
            }

            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Add(key, values);
            csvLoader.LoadCSV();

            UpdateDictionary();
        }

        public static void Replace(string key, string[] values)
        {
            foreach(string value in values)
            {
                if (value.Contains("\""))
                {
                    value.Replace('"', '\"');
                }
            }

            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Edit(key, values);
            csvLoader.LoadCSV();

            UpdateDictionary();

        }

        public static void Remove(string key)
        {
            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Remove(key);
            csvLoader.LoadCSV();

            UpdateDictionary();

        }

    }
}
