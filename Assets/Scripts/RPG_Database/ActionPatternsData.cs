using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class ActionPatternsData : ScriptableObject
{
    // General
    public string actionName;

    // Index
    public int selectedSkillIndex;
    public int selectedConditionIndex;
    public int additionalSelectedIndex;

    // Values Inside ActionPatterns Window
    public int ratingValue;
        // Turn or HP or MP
        public int additionalValue1;
        public int additionalValue2;

    public void OnEnable()
    {
        if(actionName == null)
        {
            Init();
        }
    }

    public void Init()
    {

    }
}
