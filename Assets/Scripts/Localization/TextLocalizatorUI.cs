using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Remorse.Localize
{
    public class TextLocalizatorUI : MonoBehaviour
    {
        public LocalizedString localizedString;
        Text text;

        private void Start()
        {
            text = GetComponent<Text>();
        }
        public void Update()
        {
            text.text = localizedString.value;
        }
    }
}
