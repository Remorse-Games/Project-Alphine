using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/SkillsToLearn")]
public class SkillsToLearn : ScriptableObject
{
    public string skillToLearnName;
    public int selectedArrayIndex;
    public int level;

    [TextArea]
    public string notes;
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
