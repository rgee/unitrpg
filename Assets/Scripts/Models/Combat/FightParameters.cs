using UnityEngine;
using System.Collections;

public struct FightParameters {
	public readonly int Damage;
	public readonly int Hits;
	public readonly int HitChance;
	public readonly int CritChance;
	public readonly int GlanceChance;

	public FightParameters (int damage, int hits, int hitChance, int critChance, int glanceChance) {
		this.Damage = damage;
		this.Hits = hits;
		this.HitChance = hitChance;
		this.CritChance = critChance;
		this.GlanceChance = glanceChance;
	}
}
