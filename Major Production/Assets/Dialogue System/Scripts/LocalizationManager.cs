using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    public class LocalizationManager : MonoBehaviour
    {

        Dictionary<string, string> lines;

        [SerializeField]
        LocalizationLoader loader;

        [SerializeField]
        string locale;

        public string Locale { get { return locale; } }

        // Use this for initialization
        void Start()
        {
            // load lines from currently set locale
            lines = loader.LoadLanguage(locale);
            if(lines == null)
            {
                Debug.LogError("Could not load localization data for locale " + locale);
            }
        }

        // Loads the lines from the new locale, or returns false on failure
        public bool SetLocale(string locale)
        {
            Dictionary<string, string> newLines = loader.LoadLanguage(locale);
            if(newLines == null)
            {
                return false;
            } else
            {
                lines = newLines;
                this.locale = locale;
                return true;
            }

        }

        public string GetLine(string key)
        {
            string line;
            try
            {
                line = lines[key];
            }
            catch (KeyNotFoundException)
            {
                line = "No translation found for " + key;
            }
            return line;
        }
    }
}
