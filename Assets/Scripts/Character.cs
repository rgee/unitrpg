using UnityEngine;
using System.Collections;

[System.Serializable]
public class Character {

	public enum Army {
		PLAYER,
		ENEMY,
		NEUTRAL
	};

	public string name;
	public int maxHealth;
	public int level = 1;
	public Army army;
	
}
