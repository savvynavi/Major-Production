using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	//holds all info specific to a character class

	[CreateAssetMenu(fileName = "Class", menuName = "RPG/Class", order = 3)]
	public class ClassInfo : ScriptableObject {

		public List<Powers> classPowers;

	}
}
