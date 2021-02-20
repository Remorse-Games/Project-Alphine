using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Remorse.Localize;

public class textcontroll2 : MonoBehaviour
{
    public Text a1;
    public Text a2;
    public Text b1;
    public Text b2;

    public TextMeshProUGUI nextbtn;
    public TextMeshProUGUI menubtn;

    int i = -1;
    // Start is called before the first frame update
    void Start()
    {
        a1.text = "AGUS : " + Localization.GetLocalizedValue("text1");
        nextbtn.text = Localization.GetLocalizedValue("next");
        menubtn.text = Localization.GetLocalizedValue("back");
    }

    // Update is called once per frame
    void Update()
    {
        nextbtn.text = Localization.GetLocalizedValue("next");
        menubtn.text = Localization.GetLocalizedValue("back");
        if (i==-1) { }
        else if (i == 0)
        {
            a1.text = "AGUS : " + Localization.GetLocalizedValue("text1");
            b1.text = Localization.GetLocalizedValue("text2") + " : BENY";
        }
        else if (i == 1)
        {
            a1.text = "AGUS : " + Localization.GetLocalizedValue("text1");
            b1.text = Localization.GetLocalizedValue("text2") + " : BENY";
            a2.text = "AGUS : " + Localization.GetLocalizedValue("text3");
        }
        else if (i == 2)
        {
            a1.text = "AGUS : " + Localization.GetLocalizedValue("text1");
            b1.text = Localization.GetLocalizedValue("text2") + " : BENY";
            a2.text = "AGUS : " + Localization.GetLocalizedValue("text3");
            b2.text = Localization.GetLocalizedValue("text4") + " : BENY";
        }
        else
        {
            a1.text = "AGUS : " + Localization.GetLocalizedValue("text1");
            b1.text = Localization.GetLocalizedValue("text2") + " : BENY";
            a2.text = "AGUS : " + Localization.GetLocalizedValue("text3");
            b2.text = Localization.GetLocalizedValue("text4") + " : BENY";

        }
    }

    public void setLanguage(string language)
    {

        switch (language)
        {
            case "id":
                Localization.languange = Localization.Languange.Indonesia;
                break;
            case "en":
                Localization.languange = Localization.Languange.English;
                break;
            case "sp":
                Localization.languange = Localization.Languange.Spanyol;
                break;
            default:
                Localization.languange = Localization.Languange.English;
                break;

        }

        Debug.Log(Localization.languange);
    }

    public void next() 
    {
        
        if (i == 0)
        {
            b1.text = Localization.GetLocalizedValue("text2") + " : BENY";
        }else if (i == 1)
        {
            a2.text = "AGUS : " + Localization.GetLocalizedValue("text3");
        }else if (i == 2)
        {
            b2.text = Localization.GetLocalizedValue("text4") + " : BENY";
        }
        i++;
    
    }
    public void nextscene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
