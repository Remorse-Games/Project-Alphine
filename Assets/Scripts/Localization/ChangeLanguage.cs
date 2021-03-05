using UnityEngine;

namespace Remorse.Localize
{
    public class ChangeLanguage : MonoBehaviour
    {
        public string languageId;
        public void TriggerChangeLanguage()
        {
            ChangeLanguageValue();
        }

        private void ChangeLanguageValue()
        {
            Localization.languageId = languageId;
            Localization.UpdateDictionary();
        }
    }

}
