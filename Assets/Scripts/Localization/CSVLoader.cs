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
        private char fieldSeparator =';';

        public void LoadCSV()
        {
            csvFile = Resources.Load<TextAsset>("localization");
           
        }

        public Dictionary<string, string> GetDictionaryValues(string attributeId)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] lines = csvFile.text.Split(lineSeparator);
            int attributeIndex = -1;
            string[] headers = lines[0].Split(fieldSeparator);
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Contains(attributeId))
                {
                    attributeIndex = i;
                    break;
                }
            }
                      
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(fieldSeparator);
                string key = fields[0];
                if (dictionary.ContainsKey(key)) { continue; }
                string value = fields[attributeIndex];
                dictionary.Add(key, value);

            }
            

            return dictionary;
        }

               
    }
}
