using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public bool isDead;
	public int currentHealth;
	public string characterName;

	private Character character;

	public void Start() {
		character = SaveGame.current.getByName(characterName);
		currentHealth = character.maxHealth;
	}

	public Character.Army GetArmy() {
		return character.army;
	}
}
