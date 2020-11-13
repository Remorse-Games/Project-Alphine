using UnityEngine;

public class TypeEquipmentData : ScriptableObject
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
        dataName = "equipment";
    }
}