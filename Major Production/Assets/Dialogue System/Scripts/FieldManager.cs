using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class FieldManager
    {

        HashSet<string> flags;
        Dictionary<string, float> numbers;

        public FieldManager()
        {
            flags = new HashSet<string>();
            numbers = new Dictionary<string, float>();
        }

        public void SetFlag(string flag)
        {
            flags.Add(flag);
        }

        public void UnsetFlag(string flag)
        {
            flags.Remove(flag);
        }

        public bool CheckFlag(string flag)
        {
            return flags.Contains(flag);
        }

        public void ClearFlags()
        {
            flags.Clear();
        }

        public void SetNumber(string name, float value)
        {
            numbers[name] = value;
        }

        public float GetNumber(string name)
        {
            //HACK figure out how to deal with null values
            float value;
            numbers.TryGetValue(name, out value);
            return value;
        }

        public void ClearNumbers()
        {
            numbers.Clear();
        }
    }
}