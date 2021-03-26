using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace RemorseWindow
{
    public class BaseControll
    {
        public BaseControll(EditorWindow currentEditorWindow, BaseControll parent, String name, Rect rect)
        {
            m_editorWindow = currentEditorWindow;
            this.parent = parent;
            this.name =  name;
            
            this.rect =  (this.parent != null)? 
                    new Rect( parent.rect.position + rect.position, rect.size) : rect;
                    
            listOptions = new List<GUILayoutOption>();
            guiContent  = new GUIContent();
            
            listOptions.Add( GUILayout.Width(rect.width) );
            listOptions.Add( GUILayout.Height(rect.height) );
            text = name;
            options = listOptions.ToArray();
            guiContent.text = text;
            
            isActive = true;
        }
        
        public  String               name;
        public  String               text;
        
        public  bool                 isActive;
        public  EditorWindow         m_editorWindow ;

        public  Rect                 rect;
        public  BaseControll         parent;
        
        public  GUIContent           guiContent;
        public  GUIStyle             guiStyle;
        protected GUILayoutOption[]    options;
        public  List<GUILayoutOption>   listOptions;
        
        protected List<bool>        condition;
        public  List<Action>         listEvents;
        public  List<BaseControll>   listDraw;
        
        public  virtual void Draw()
        { 
            for(int i = 0; i < listDraw.Count; i++)
            {
                if( listDraw[i].isActive )
                    listDraw[i].Draw();
            }
        }
        protected virtual void ExecuteEvents()
        { }
        
        public Texture2D SetCustomTextureColor( Color color, string name )
        {
            Texture2D texture = new Texture2D((int)rect.width, (int)rect.height);
            SetTexture( texture, color );
            
            customTexture[name] = texture;
            return texture;
        }

        public Texture2D GetCustomTextureColor( string name )
        {
            Texture2D texture;
            if (customTexture.TryGetValue(name, out texture))
                return texture;
            
            return null;
        }
        
        
        private void SetTexture(Texture2D texture, Color color)
        {
            Color[] pixels = Enumerable.Repeat( color, (int) (rect.width * rect.height) ).ToArray();
            texture.SetPixels(pixels);
            texture.Apply();
        }
        
        private Dictionary<string, Texture2D> customTexture = 
                        new Dictionary<string, Texture2D>(StringComparer.OrdinalIgnoreCase);
                        
        private Texture2D RedTexture     ;
        private Texture2D BlueTexture    ;
        private Texture2D GreenTexture   ;
        private Texture2D WhiteTexture   ;
        private Texture2D BlackTexture   ;
        private Texture2D YelowTexture   ;
        private Texture2D CyanTexture    ;
        private Texture2D MagentaTexture ;
        private Texture2D GrayTexture    ;
        
        public Texture2D GetRedTexture
        {
            get
            {
                if(RedTexture)
                    return RedTexture;
                
                RedTexture      =   new Texture2D((int)rect.width, (int)rect.height);
                SetTexture(RedTexture,  Color.red);
                return RedTexture;
            }
        }
        public Texture2D GetBlueTexture
        {
            get
            {
                if(BlueTexture)
                    return BlueTexture;
                
                BlueTexture      =   new Texture2D((int)rect.width, (int)rect.height);
                SetTexture(BlueTexture,  Color.blue);
                return BlueTexture;
            }
        }
        public Texture2D GetGreenTexture
        {
            get
            {
                if(GreenTexture)
                    return GreenTexture;
                
                GreenTexture      =   new Texture2D((int)rect.width, (int)rect.height);
                SetTexture(GreenTexture,  Color.green);
                return GreenTexture;
            }
        }
        public Texture2D GetWhiteTexture
        {
            get
            {
                if(WhiteTexture)
                    return WhiteTexture;
                
                WhiteTexture      =   new Texture2D((int)rect.width, (int)rect.height);
                SetTexture(WhiteTexture,  Color.white);
                return WhiteTexture;
            }
        }
        public Texture2D GetBlackTexture
        {
            get
            {
                if(BlackTexture)
                    return BlackTexture;
                
                BlackTexture      =   new Texture2D((int)rect.width, (int)rect.height);
                SetTexture(BlackTexture,  Color.black);
                return BlackTexture;
            }
        }
        public Texture2D GetYellowTexture
        {
            get
            {
                if(YelowTexture)
                    return YelowTexture;
                
                YelowTexture      =   new Texture2D((int)rect.width, (int)rect.height);
                SetTexture(YelowTexture,  Color.red);
                return YelowTexture;
            }
        }
        public Texture2D GetCyanTexture
        {
            get
            {
                if(CyanTexture)
                    return CyanTexture;
                
                CyanTexture      =   new Texture2D((int)rect.width, (int)rect.height);
                SetTexture(CyanTexture,  Color.cyan);
                return CyanTexture;
            }
        }
        public Texture2D GetMagentaTexture
        {
            get
            {
                if(MagentaTexture)
                    return MagentaTexture;
                
                MagentaTexture      =   new Texture2D((int)rect.width, (int)rect.height);
                SetTexture(MagentaTexture,  Color.magenta);
                return MagentaTexture;
            }
        }
        public Texture2D GetGrayTexture
        {
            get
            {
                if(GrayTexture)
                    return GrayTexture;
                
                GrayTexture      =   new Texture2D((int)rect.width, (int)rect.height);
                SetTexture(GrayTexture,  Color.gray);
                return GrayTexture;
            }
        }
    }
}
