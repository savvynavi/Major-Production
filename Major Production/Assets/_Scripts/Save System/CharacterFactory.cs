using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using RPG;
using RPGsys;

[Serializable]
public class StringObjectDict : Dialogue.SerializedDictionary<string, GameObject> { };  //might use this?

namespace RPG.Save
{
	public class CharacterFactory 
	{
        public static Character CreateCharacter(JObject data)
        {
            string prefabName = (string)data["name"];
            Character created = Factory<Character>.CreateInstance(prefabName);
            created.Load(data);     // If this breaks, probably due to awake not firing properly?
            return created;
        }

        public static GameObject CreatePlayerTeam(JArray teamData)
        {
            GameObject teamObject = new GameObject();
            foreach(JObject playerData in teamData)
            {
                Character c = CreateCharacter(playerData);
                c.transform.SetParent(teamObject.transform);
            }
            return teamObject;
        }
	}
}