using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	//holds all info specific to a character class

	public enum EClassType
	{
		None,
		Knight,
		Mage,
		Bard,
		Rogue
	}

	[CreateAssetMenu(fileName = "Class", menuName = "RPG/Class", order = 3)]
	public class ClassInfo : ScriptableObject {

		public EClassType classType;
		public List<Powers> classPowers;

	}
}
