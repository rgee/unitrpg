using Models;
using UnityEngine;

public class Unit : MonoBehaviour {
    private Character character;
    public string characterName;
    public int currentHealth;
    public bool isDead;

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