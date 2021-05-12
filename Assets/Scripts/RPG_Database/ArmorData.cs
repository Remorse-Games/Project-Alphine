using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorData : ScriptableObject
{
    // General
    public string armorName;
    [TextArea]
    public string armorDescription;
    [TextArea]
    public string notes;

    // Index
    public int selectedArmorTypeIndex;
    public int selectedArmorEquipmentIndex;

    // Basic Values
    public int armorPrice;
    public int armorAttack;
    public int armorDefense;
    public int armorMAttack;
    public int armorMDefense;
    public int armorAgility;
    public int armorLuck;
    public int armorMaxHP;
    public int armorMaxMP;

    // Sprite Image
    public Sprite Icon;

    public void OnEnable()
    {
        if (armorName == null)
        {
            Init();
        }
    }
    public void Init()
    {
        armorName = "New Armor";
    }
}
