using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGsys;


public class CharacterBox : MonoBehaviour {

	[SerializeField] Text nameText;
	Character character;
	
	public Character ContainedCharacter { get { return character; } set { SetCharacter(value); } }

	void SetCharacter(Character c)
	{
		character = c;
		nameText.text = character.name;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
