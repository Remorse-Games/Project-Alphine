using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EconomySystem : MonoBehaviour
{
    int money;
    // Start is called before the first frame update
    TextMeshProUGUI moneyStatus;
    void Start()
    {
        Destroy(GameObject.Find("Warning Text"));
        money = PlayerPrefs.GetInt("Money");
        moneyStatus = GameObject.Find("Money Status").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyStatus.text = "Money: $" + money.ToString();
    }

    public void SpendOrEarnMoney(int _money)
    {
        if (money + _money >= 0)
            money += _money;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }
}
