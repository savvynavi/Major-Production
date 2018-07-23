using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    /// <summary>
    /// Loads dialogue lines from a .csv file with the first row having the locale codes
    /// </summary>
    [CreateAssetMenu(menuName = "Dialogue/Localization/CSV Loader")]
    public class CSVLoader : LocalizationLoader
    {
        [SerializeField]
        TextAsset localizationData;

        public override Dictionary<string, string> LoadLanguage(string locale)
        {
            // Split file into rows
            string[] fileLines = localizationData.text.Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
            // Find column for locale code
            string[] header = SplitCSVRow(fileLines[0]);
            int localeIndex = Array.IndexOf(header, locale);
            if(localeIndex >= 0)
            {
                Dictionary<string, string> loadedLines = new Dictionary<string, string>();
                // Start at 1 because index 0 was header
                for (int i = 1; i < fileLines.Length; ++i)
                {
                    // Split row into entries
                    string[] values = SplitCSVRow(fileLines[i]);
                    try {
                        // Key is first entry in line, value wanted is in column selected
                        string key = values[0];
                        string translation = values[localeIndex];
                        loadedLines.Add(key, translation);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Debug.LogWarning("Translation not found on line " + i.ToString());
                    }
                }
                // Return dictionary of lines
                return loadedLines;
            }
            else
            {
                // Locale not found
                return null;
            }
        }

        string[] SplitCSVRow(string row)
        {
            // split by commas
            // However, if entry starts with ", ignore commas until matching quote
            // "" found within " does not match, only single "
            // Remove " from either end and change "" to "
            List<string> values = new List<string>();
            System.Text.StringBuilder current = new System.Text.StringBuilder();
            bool inQuotes = false;
            bool maybeDoubleQuote = false;  // last character was ", and is either first of "" or a close quote
            foreach(char c in row)
            {
                switch (c)
                {
                    case ',':
                        bool escape = false; // Should it be escaped or treated as delimiter?
                        if (inQuotes)
                        {
                            if (maybeDoubleQuote)
                            {
                                // If in quote and last was quote, quote has ended so don't escape
                                inQuotes = false;
                                escape = false;
                            } else
                            {
                                // If last wasn't quote (or second of double quote) still quoting so escape
                                escape = true;
                            }
                        }
                        if (escape)
                        {
                            // If still in quote, write ,
                            current.Append(',');
                        } else
                        {
                            // If outside quote, copy current to list and start new entry
                            values.Add(current.ToString());
                            current = new System.Text.StringBuilder();
                            inQuotes = false;
                        }
                        maybeDoubleQuote = false;
                        break;
                    case '"':
                        if (inQuotes)
                        {
                            if (maybeDoubleQuote)
                            {
                                // Double quote inside a quote, so write "
                                maybeDoubleQuote = false;
                                current.Append('"');
                            } else
                            {
                                // Quote inside quote might be first of pair
                                maybeDoubleQuote = true;
                            }
                        } else
                        {
                            // If not in quotes, must be start of quote
                            inQuotes = true;
                            maybeDoubleQuote = false;
                        }
                        break;
                    default:
                        current.Append(c);
                        if (maybeDoubleQuote)
                        {
                            inQuotes = false;
                        }
                        maybeDoubleQuote = false;
                        break;
                }
            }
            // Add last entry to list
            values.Add(current.ToString());
            return values.ToArray();
        }
    }
}