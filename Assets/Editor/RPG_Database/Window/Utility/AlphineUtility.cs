
namespace Remorse.Tools.RPGDatabase.Utility
{
    /* The Helper */
    public static class AlphineHelper
    {
        public static int NumberMinFilter(ref int value, int defaultValue)
        {
            return value = value < defaultValue ? defaultValue : value;
        }
        
        public static int NumberMaxFilter(ref int value, int defaultValue)
        {
            return value = value > defaultValue ? defaultValue : value;
        }
        
        public static int NumberMinMaxFilter(ref int value, int defaultMinValue, int defaultMaxValue)
        {
            NumberMinFilter(ref value, defaultMinValue);
            return NumberMaxFilter(ref value, defaultMaxValue);
        }
    }
    
   /*  Extended Version for Custom EditorGUILayout */
    public static class EditorGUILayoutExt
    {
        public static int IntField(int min, int max, int value, UnityEngine.GUILayoutOption guiLayoutWidth, UnityEngine.GUILayoutOption guiLayoutHeight)
        {
            AlphineHelper.NumberMinMaxFilter(ref value, min, max);
            return UnityEditor.EditorGUILayout.IntField(value, guiLayoutWidth, guiLayoutHeight);
        }
        public static int IntField(int min, int max, int value, UnityEngine.GUILayoutOption guiLayoutWidth)
        {
            AlphineHelper.NumberMinMaxFilter(ref value, min, max);
            return UnityEditor.EditorGUILayout.IntField(value, guiLayoutWidth);
        }
    }
}

