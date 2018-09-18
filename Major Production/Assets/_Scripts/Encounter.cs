using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGsys;

namespace RPG
{
	[CreateAssetMenu(fileName = "Encounter", menuName = "RPG/Encounter")]
	public class Encounter : ScriptableObject
	{
		[SerializeField] List<Character> enemies;
		
		public GameObject InstantiateEnemyTeam()
		{
			GameObject team = new GameObject();
			foreach(Character enemy in enemies)
			{
				Instantiate(enemy, team.transform);
			}
			team.SetActive(false);
			return team;
		}
	}
}