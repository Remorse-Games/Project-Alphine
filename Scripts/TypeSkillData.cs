using UnityEngine;

public class TypeSkillData : ScriptableObject
{
    public string dataName;

    public void OnEnable()
    {
        if (dataName == null)
        {
            Init();
        }
    }

    public void Init()
    {
        dataName = "New Skill Type";
    }
}