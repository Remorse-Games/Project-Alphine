using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemData : ScriptableObject
{
    // Ask Keju For Further Details
    public List<string> startingParty;

    public string gameTitle;

    public string currencyUnit;

    public List<string> magicSkills;

    public Color32 windowColor;

    public void OnEnable()
    {
        if (gameTitle == null &&
            currencyUnit == null)
        {
            Init();
        }
    }

    public void Init()
    {
        windowColor = new Color32(128,128,128,255);
    }
}
