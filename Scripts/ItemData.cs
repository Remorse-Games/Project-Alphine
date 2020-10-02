using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite Icon;

    [TextArea]
    public string itemDescription;
    public string itemUserNameMessage;
    public string itemMessage;

    //Index for selected Class.
    public int selecteditemTypeIndex;
    public int selecteditemScopeIndex;
    public int selecteditemOccasionIndex;
    public int selecteditemHitTypeIndex;
    public int selecteditemAnimationIndex;
    public int selectedConsumableIndex;

    public int selectedTypeIndex;
    public int selectedElementIndex;
    public int selectedCriticalHits;

    public int itemPrice;


    public int itemSpeed;
    public int itemSuccessLevel;
    public int itemRepeat;
    public int itemTPGain;

    public string itemFormula;
    public int itemVariance;

    //TODO: Effects

    [TextArea]
    public string notes;

    public void OnEnable()
    {
        Sprite sp = Resources.Load<Sprite>("Image");

        itemName = "item";
        Icon = sp;
        itemDescription = "Insert your description here";
        itemPrice = 0;
        itemSpeed = 0;
        itemSuccessLevel = 100;
        itemRepeat = 1;
        itemTPGain = 10;
        itemFormula = "b.mhp / 2";
        itemVariance = 20;
        notes = "";
    }
}
