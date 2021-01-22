using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemData : ScriptableObject
{
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
        startingParty = new List<string>();
        magicSkills = new List<string>();

        gameTitle = "Game Title";

        currencyUnit = "G";
        windowColor = new Color32(128,128,128,255);





    }
}
