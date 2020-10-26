using UnityEngine;

public class TypeWeaponData : ScriptableObject
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
        dataName = "weapon";
    }
}