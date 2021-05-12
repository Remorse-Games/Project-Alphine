using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
public static class DataIntegration
{
   public static void GetAllData<GameData>(List<GameData> listData, string labelName) where GameData : ScriptableObject
    {
        string[] list = AssetDatabase.FindAssets("l:" + labelName, null);
        for(int i = 0; i < list.Length; i++)
        {
            //Remove (0,6) To Remove "Assets/" Because To Use Resources.Load, It Can't Have "Assets/" At The Head
            listData.Add(Resources.Load<GameData>(AssetDatabase.GUIDToAssetPath(list[i].Remove(0, 6))));
        }
    }

    public static void GetAllData<GameData>(List<GameData> listData, string labelName, string folderPath) where GameData : ScriptableObject
    {
        string[] list = AssetDatabase.FindAssets("l:" + labelName, new[] { folderPath });
        for (int i = 0; i < list.Length; i++)
        {
            //Remove (0,6) To Remove "Assets/" Because To Use Resources.Load, It Can't Have "Assets/" At The Head
            listData.Add(Resources.Load<GameData>(AssetDatabase.GUIDToAssetPath(list[i].Remove(0, 6))));
        }
    }
}
