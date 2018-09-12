using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Save
{
	public class Factory<T> where T : Object
	{
		static Dictionary<string, T> objects = new Dictionary<string, T>();

        static Factory()
        {
            Factory<T>.objects = new Dictionary<string, T>();
        }

		// Call from OnEnable of class to register on loading assets
		public static void Register(T obj)
		{
			if(obj.name != "" && !obj.name.EndsWith("(Clone)"))
			{
				if (!Factory<T>.objects.ContainsKey(obj.name))
				{
					Factory<T>.objects[obj.name] = obj;
				}
			}
		}

		public static T Find(string name)
		{
			T value = null;
			Factory<T>.objects.TryGetValue(name, out value);
			return value;
		}

		public static T CreateInstance(string name)
		{
			T instance = null;
			T source = Find(name);
			if(source != null)
			{
				instance = Object.Instantiate(source);
			}
			return instance;
		}
	}
}