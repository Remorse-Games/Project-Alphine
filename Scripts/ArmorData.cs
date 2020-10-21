using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorData : ScriptableObject
{
    public string armorName;
    public Sprite Icon;

    [TextArea]
    public string armorDescription;

    //Index for selected Class.
    public int selectedArmorTypeIndex;
    public int selectedArmorEquipmentIndex;

    public int armorPrice;
    public int armorAttack;
    public int armorDefense;
    public int armorMAttack;
    public int armorMDefense;
    public int armorAgility;
    public int armorLuck;
    public int armorMaxHP;
    public int armorMaxMP;

    [TextArea]
    public string notes;
    public void OnEnable()
    {
        if (armorName == null)
        {
            Init();
        }
    }
    public void Init()
    {
        Sprite sp = Resources.Load<Sprite>("Image");

        armorName = "armor";
        Icon = sp;
        armorDescription = "Insert your description here";
        armorPrice = 500;
        armorAttack = 10;
        armorDefense = 0;
        armorMAttack = 0;
        armorMDefense = 0;
        armorAgility = 0;
        armorLuck = 0;
        armorMaxHP = 0;
        armorMaxMP = 0;
        notes = "";
    }
}
