using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow.Utility
{
    public class RemorseImaging
    {
        public RemorseImaging(){
        }
        
       public static Texture2D LoadImageToTexture(string filePath, ref Texture2D tex) 
       {
             if(tex == null)
                 tex = new Texture2D(1, 1);
             
             byte[] fileData;
             if (File.Exists(filePath)){
                 fileData = File.ReadAllBytes(filePath);
                 tex.LoadImage(fileData);
             }
             return tex;
         }
       public static Texture2D LoadImageToTexture(string filePath) 
       {
            Texture2D tex = new Texture2D(1, 1);
             
             byte[] fileData;
             if (File.Exists(filePath)){
                 fileData = File.ReadAllBytes(filePath);
                 tex.LoadImage(fileData);
             }
             return tex;
         }
    }
}
