using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Save
{
	public class Factory<T> where T : Object
	{
		static Dictionary<string, T> objects = new Dictionary<string, T>();

		// Call from OnEnable of class to register on loading assets
		public static void Register(T obj)
		{
			if(obj.name != "" && !obj.name.EndsWith("(Clone)"))
			{
				if (!objects.ContainsKey(obj.name))
				{
					objects[obj.name] = obj;
				}
			}
		}

		public static T Find(string name)
		{
			T value = null;
			objects.TryGetValue(name, out value);
			return value;
		}

		public static T Instantiate(string name)
		{
			T instance = null;
			T source = Find(name);
			if(source != null)
			{
				instance = Utility.InstantiateSameName(source);
			}
			return instance;
		}

		public static string TrimCloned(string name)
		{
			return name.Replace(" (Clone)", string.Empty);
		}
	}
}