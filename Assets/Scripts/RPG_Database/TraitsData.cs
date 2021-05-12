using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/TraitData")]
public class TraitsData : ScriptableObject
{
    // General
    public string traitName;

    // Index
    public int selectedTabToggle;
    public int selectedTabIndex;
    public int selectedArrayIndex;

    // Value
    public int traitValue;

    public void OnEnable()
    {
        if (traitName == null)
        {
            Init();
        }
    }
    public void Init()
    {

    }
}
