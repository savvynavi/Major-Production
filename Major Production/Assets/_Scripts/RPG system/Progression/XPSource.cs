using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace RPG.XP
{
	public class XPSource : MonoBehaviour
	{
		[SerializeField] int xp;
		public int XP { get { return xp; } }
	}
}