using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialogue
{
    class StringUtility
    {
        public static string TruncateString(string text, int maxLength, string truncator = "...")
        {
            if(maxLength > 0 && text.Length > maxLength)
            {
                text = text.Substring(0, maxLength) + truncator;
            }
            return text;
        }
    }
}
