using System.Collections.Generic;
using Remorse.Localize;

namespace Remorse.Chat{
    [System.Serializable]
    public class Dialogue
    {
        public LocalizedString name;
        public List<LocalizedString> sentences;
    }
}