using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/TraitData")]
public class TraitsData : ScriptableObject
{
    public string traitName;
    public int selectedTabToggle;
    public int selectedTabIndex;
    public int selectedArrayIndex;
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
