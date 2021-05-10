using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    EconomySystem myValues;
    void Start()
    {
        myValues = GetComponent<EconomySystem>();
    }

    public void Earn50()
    {
        myValues.SpendOrEarnMoney(50);
    }
    public void Earn60()
    {
        myValues.SpendOrEarnMoney(60);
    }
    public void Earn70()
    {
        myValues.SpendOrEarnMoney(70);
    }
    public void Item80()
    {
        myValues.SpendOrEarnMoney(-80);
    }
    public void Item90()
    {
        myValues.SpendOrEarnMoney(-90);
    }
    public void Item100()
    {
        myValues.SpendOrEarnMoney(-100);
    }
}
