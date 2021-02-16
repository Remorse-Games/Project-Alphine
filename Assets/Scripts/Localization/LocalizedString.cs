using System;

namespace Remorse.Localize
{
    [Serializable]
    public class LocalizedString
    {
        public string key;

        public LocalizedString(string key)
        {
            this.key = key;
        }

        public string value
        {
            get
            {
                return Localization.GetLocalizedValue(key);
            }
        }

        public static explicit operator string(LocalizedString v)
        {
            throw new NotImplementedException();
        }

        public static implicit operator LocalizedString(string key)
        {
            return new LocalizedString(key);
        }
    }
}
