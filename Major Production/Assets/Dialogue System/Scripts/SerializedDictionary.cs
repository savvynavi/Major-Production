using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    /// <summary>
    /// Generic class which can be used to make serializable dictionary classes
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [Serializable]
    public class SerializedDictionary<TKey, TValue>
    {
        [SerializeField]
        private List<TKey> keys;
        [SerializeField]
        private List<TValue> values;

        public SerializedDictionary()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
        }

        // Converts Serialized Dictionary to a Dictionary object
        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            int dictCount = Math.Min(keys.Count, values.Count);
            for (int i = 0; i < dictCount; ++i)
            {
                dict.Add(keys[i], values[i]);
            }
            return dict;
        }

        // Copies keys and values from a Dictionary object
        public void CopyDictionary(Dictionary<TKey,TValue> dictionary)
        {
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }
    }

    [Serializable]
    public class StringActorDict : SerializedDictionary<string, DialogueActor> { };

    [Serializable]
    public class StringAnimatorDict : SerializedDictionary<string, Animator> { };

	[Serializable]
	public class StringEventDict : SerializedDictionary<string, UnityEvent> { };
}
