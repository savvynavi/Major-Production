using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility  {

	public static T InstantiateSameName<T>(T original) where T : Object
	{
		T clone = Object.Instantiate(original);
		clone.name = original.name;
		return clone;
	}

	public static T InstantiateSameName<T>(T original, Transform parent) where T : Object
	{
		T clone = Object.Instantiate(original, parent);
		clone.name = original.name;
		return clone;
	}

	public static T InstantiateSameName<T>(T original, Transform parent, bool instantiateInWorldSpace) where T : Object
	{
		T clone = Object.Instantiate(original, parent, instantiateInWorldSpace);
		clone.name = original.name;
		return clone;
	}

	public static T InstantiateSameName<T>(T original, Vector3 position, Quaternion rotation) where T : Object
	{
		T clone = Object.Instantiate(original, position, rotation);
		clone.name = original.name;
		return clone;
	}

	public static T InstantiateSameName<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
	{
		T clone = Object.Instantiate(original,position,rotation, parent);
		clone.name = original.name;
		return clone;
	}

	public static string TrimCloned(string name)
	{
		return name.Replace("(Clone)", string.Empty);
	}
}
