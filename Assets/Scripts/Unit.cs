using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public bool isDead;
	public int currentHealth;
	public string characterName;

	private Models.Character character;

	public void Start() {
		character = SaveGame.current.getByName(characterName);
		currentHealth = character.MaxHealth;
	}

	public bool IsEnemy() {
		return character.IsEnemy;
	}

	public int GetMovement() { 
		return character.Movement;
	}
}
