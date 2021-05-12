using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Database/EffectData")]
public class EffectData : ScriptableObject
{
    // General
    public string effectName;

    // Index
    public int selectedTabToggle;
    public int selectedTabIndex;
    public int selectedArrayIndex;

    // Value
    public int[] effectValue = new int[2] { 100, 0 };

    public void OnEnable()
    {
        if(effectName == null)
        {
            Init();
        }
    }

    public void Init()
    {

    }
}
