using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;
using RPGsys;

namespace RPG.Save
{
	[CreateAssetMenu(fileName = "Character Prefab List", menuName = "Serialization/Character Prefab List")]
	public class CharacterPrefabList : ScriptableObject
	{
		[SerializeField] List<Character> characters;

		private void OnEnable()
		{
            // This doesn't always go off? Figure out why (editor only loads it once?)
			foreach(Character c in characters)
			{
				Factory<Character>.Register(c);
			}
		}
	}
}