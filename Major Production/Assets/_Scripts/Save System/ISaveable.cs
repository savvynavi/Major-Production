using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace RPG.Save
{
	public interface ISaveable
	{
		JObject Save();
		void Load(JObject data);
	}
}