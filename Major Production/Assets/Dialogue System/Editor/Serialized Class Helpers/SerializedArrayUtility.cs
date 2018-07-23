using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    // Holds functions to work with serialized arrays
    class SerializedArrayUtility
    {
        public static SerializedProperty FindPropertyByValue(SerializedProperty array, string relativePath, int intValue)
        {
            SerializedProperty currentElement;
            if (array.isArray)
            {
                for(int i = 0; i < array.arraySize; ++i)
                {
                    currentElement = array.GetArrayElementAtIndex(i);
                    if(currentElement.FindPropertyRelative(relativePath).intValue == intValue)
                    {
                        return currentElement;
                    }
                }
            }
            return null;
        }

        public static int FindIndexByValue(SerializedProperty array, string relativePath, int intValue)
        {
            SerializedProperty currentElement;
            if (array.isArray)
            {
                for(int i = 0; i < array.arraySize; ++i)
                {
                    currentElement = array.GetArrayElementAtIndex(i);
                    if (currentElement.FindPropertyRelative(relativePath).intValue == intValue)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
