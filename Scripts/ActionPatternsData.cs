using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/ActionData")]
public class ActionPatternsData : ScriptableObject
{
    public string actionName;
    public int selectedSkillIndex;
    public int ratingValue;

    public int selectedConditionIndex;

    public int additionalValue1;
    public int additionalValue2;
    public int additionalSelectedIndex;
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
