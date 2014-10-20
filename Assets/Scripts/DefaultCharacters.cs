using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultCharacters {

	private static Dictionary<string, Character> DEFAULT_CHARACTERS = new Dictionary<string, Character>();

	static DefaultCharacters ()	{
		Character liat = new Character();
		liat.name = "Liat";
		liat.maxHealth = 10;
		liat.army = Character.Army.PLAYER;
		DEFAULT_CHARACTERS.Add(liat.name, liat);

		Character gatsu = new Character();
		gatsu.name = "Gatsu";
		gatsu.maxHealth = 10;
		gatsu.army = Character.Army.ENEMY;
		DEFAULT_CHARACTERS.Add (gatsu.name, gatsu);
	}
	
	public static Character getByName(string name) { 
		return DefaultCharacters.DEFAULT_CHARACTERS[name];
	}
}
