using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite Icon;

    [TextArea]
    public string weaponDescription;

    //Index for selected Class.
    public int selectedweaponTypeIndex;
    public int selectedweaponAnimationIndex;

    public int weaponPrice;
    public int weaponAttack;
    public int weaponDefense;
    public int weaponMAttack;
    public int weaponMDefense;
    public int weaponAgility;
    public int weaponLuck;
    public int weaponMaxHP;
    public int weaponMaxMP;

    [TextArea]
    public string notes;

    public void OnEnable()
    {
        Sprite sp = Resources.Load<Sprite>("Image");

        weaponName = "weapon";
        Icon = sp;
        weaponDescription = "Insert your description here";
        weaponPrice = 500;
        weaponAttack = 10;
        weaponDefense = 0;
        weaponMAttack = 0;
        weaponMDefense = 0;
        weaponAgility = 0;
        weaponLuck = 0;
        weaponMaxHP = 0;
        weaponMaxMP = 0;
        notes = "";
    }
}
