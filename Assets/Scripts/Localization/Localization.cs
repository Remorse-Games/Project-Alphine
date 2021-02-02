using System.Collections.Generic;

namespace LastBoss.Localize
{
    public class Localization
    {
        public enum Languange
        {
            English,
            Indonesia
        }

        public static Languange languange = Languange.English;

        private static Dictionary<string, string> localizedEN;
        private static Dictionary<string, string> localizedID;

        public static bool isInit;

        public static CSVLoader csvLoader;

        public static void Init()
        {
            csvLoader = new CSVLoader();
            csvLoader.LoadCSV();

            UpdateDictionaries();

            isInit = true;
        }

        public static void UpdateDictionaries()
        {
            localizedEN = csvLoader.GetDictionaryValues("en");
            localizedID = csvLoader.GetDictionaryValues("id");
        }

        public static Dictionary<string, string> GetDictionaryForEditor()
        {
            if (!isInit) { Init(); }
            switch (languange)
            {
                case Languange.English:
                    return localizedEN;
                case Languange.Indonesia:
                    return localizedID;
            }

            return localizedEN;
        }

        public static string GetLocalizedValue(string key)
        {
            if (!isInit) { Init(); }
            string value = key;

            switch (languange)
            {
                case Languange.English:
                    localizedEN.TryGetValue(key, out value);
                    break;
                case Languange.Indonesia:
                    localizedID.TryGetValue(key, out value);
                    break;
            }

            return value;
        }

        public static void Add(string key, string value)
        {
            if (value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Add(key, value);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }

        public static void Replace(string key, string value)
        {
            if (value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Edit(key, value);
            csvLoader.LoadCSV();

            UpdateDictionaries();

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

            UpdateDictionaries();

        }
    }
}
