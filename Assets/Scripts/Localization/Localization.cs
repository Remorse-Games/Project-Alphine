using System.Collections.Generic;
using UnityEngine;

namespace Remorse.Localize
{
    public class Localization
    {
        public enum Languange
        {
            English,
            Indonesia,
            Spanyol
        }

        public static Languange languange = Languange.English;

        private static Dictionary<string, string> localizedEN;
        private static Dictionary<string, string> localizedID;
        private static Dictionary<string, string> localizedSP;

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
          
            localizedSP = csvLoader.GetDictionaryValues("sp");
      
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
                case Languange.Spanyol:
                    localizedSP.TryGetValue(key, out value);
                    break;
            }
            
            return value;
        }
  
    }
}
