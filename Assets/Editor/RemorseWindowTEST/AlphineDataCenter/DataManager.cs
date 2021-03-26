using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace AlphineDataCenter
{
    public class DataManager
    {
        /* Gonna SingleTon */
        private DataManager()
        {
           listData = new Dictionary<string, ScriptableObject>();
           
           /* Add our core data from assets */
           AddData("ActorData");
           AddData("ClassData");
           AddData("ItemData");
           AddData("WeaponData");
        }
        
        public ScriptableObject GetScriptData( string name )
        {
            ScriptableObject so;
                if (listData.TryGetValue(name, out so))
                    return so;
            
            return null;
        }
        
        public static DataManager GetInstance()
        {
            if(singletonDataManager != null)
                return singletonDataManager;
            
            singletonDataManager =  new DataManager();
            return singletonDataManager;
        }
        
        private void CheckFolder(string name)
        {
                                                        /* "Assets/Resources/Data/dataName" */
            if ( !AssetDatabase.IsValidFolder( ExplicitDataPath + RelativeDataPath + name) )
            {
                AssetDatabase.CreateFolder( ExplicitDataPath + RelativeDataPath , name);
            }
        } 
        public void AddData(string name)
        {
            CheckFolder( name );
                                                               /*  "Data/dataName/dataName" */
            ScriptableObject temp = Resources.Load<ScriptableObject>(RelativeDataPath + name+ "/" + name);
            if(temp == null)
            {
                temp = ScriptableObject.CreateInstance(name);
                AssetDatabase.CreateAsset(temp, ExplicitDataPath + RelativeDataPath + name + "/" + name + ".asset");
                AssetDatabase.SaveAssets();
            }
            listData[name] = temp;
        }
        
        private Dictionary<string, ScriptableObject> listData;
        private static DataManager singletonDataManager = null;
        
        public static string ExplicitDataPath = @"Assets/Resources/";
        public static string RelativeDataPath = @"Data/";

        public static string CurrentPath = @"c:\";
        
        public static string [] listImageExtension = new string[]
        {
            ".jpg file", "jpg",
            ".png file" , "png",
            ".bmp file" , "bmp",
            "All Files" , "*"
        };
        public static string [] listSoundExtension = new string[]
        {
            ".mp3 file", "mp3",
            ".wav file" , "wav",
            ".ogg file" , "ogg",
            "All Files" , "*"
        };
        public static string [] listVideoExtension = new string[]
        {
            ".mp4 file", "mp3",
            ".mpeg file" , "mpeg",
            ".3gp file" , "3gp",
            "All Files" , "*"
        };
    }
}

