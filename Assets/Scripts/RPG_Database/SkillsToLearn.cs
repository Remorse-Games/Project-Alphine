using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/SkillsToLearn")]
public class SkillsToLearn : ScriptableObject
{
    // General
    public string skillToLearnName;
    [TextArea]
    public string notes;

    // Index
    public int selectedArrayIndex;

    // Level
    public int level;

    public void OnEnable()
    {
        if (skillToLearnName == null)
        {
            Init();
        }
    }
    public void Init()
    {

    }
}
