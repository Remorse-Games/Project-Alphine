using UnityEngine;

public class TypeElementData : ScriptableObject
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
        dataName = "element";
    }
}