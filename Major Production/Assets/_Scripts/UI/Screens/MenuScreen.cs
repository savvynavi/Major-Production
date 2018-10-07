using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public abstract class MenuScreen : MonoBehaviour
	{

		public abstract void Open();
		public abstract void Close();
	}
}