using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Remorse.Localize;
using TMPro;




    public class TextLocalizatorUI : MonoBehaviour
    {
    public TextMeshProUGUI fronttext;
    public TextMeshProUGUI playbutton;
    public TextMeshProUGUI exitbutton;
     

        private void Start()
        {

        fronttext.text = Localization.GetLocalizedValue("textdepan");
        playbutton.text = Localization.GetLocalizedValue("playbutton");
        exitbutton.text = Localization.GetLocalizedValue("exitbutton");
        }

    private void Update()
    {
        fronttext.text = Localization.GetLocalizedValue("textdepan");
        playbutton.text = Localization.GetLocalizedValue("playbutton");
        exitbutton.text = Localization.GetLocalizedValue("exitbutton");
    }

    public void setLanguage(string language)
    {
        Debug.Log(language);
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
    public void exit()
    {
        Application.Quit();
    }

    public void nextscene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }



}

