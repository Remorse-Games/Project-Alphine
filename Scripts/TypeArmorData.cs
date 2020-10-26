using UnityEngine;

public class TypeArmorData : ScriptableObject
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
        dataName = "armor";
    }
}