using UnityEngine;
using TMPro;
//using Remorse.System;

namespace Remorse.Localize
{
    public class TextUpdater : MonoBehaviour
    {
        TextMeshProUGUI textField;
        public string textOutput;
        void Start()
        {
            //Localization.languange = GameManager.Instance.languange;
            //textField = GetComponent<TextMeshProUGUI>();
            //string value = Localization.GetLocalizedValue(textOutput);
            //textField.text = value;
        }
    }
}
