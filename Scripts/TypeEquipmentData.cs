using UnityEngine;

public class TypeEquipmentData : ScriptableObject
{
    public string dataName;
    public string equipmentItem;
    public int selectedArmor;

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
        equipmentItem = "None";
    }
}