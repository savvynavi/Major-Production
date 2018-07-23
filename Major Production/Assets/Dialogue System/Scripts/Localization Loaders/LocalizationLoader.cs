using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    public class LocalizationLoader : ScriptableObject
    {
        // Get dictionary of keys to lines for specified locale from text asset
        public virtual Dictionary<string, string> LoadLanguage(string locale)
        {
            throw new System.NotImplementedException();
        }
    }
}
