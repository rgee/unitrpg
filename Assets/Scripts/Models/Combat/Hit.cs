using UnityEngine;
using System;

public struct Hit {
	public readonly int Damage;
	public readonly bool Glanced;
	public readonly bool Crit;
	public readonly bool Missed;

	public Hit (int damage, bool glanced, bool crit, bool missed)
	{
		this.Damage = damage;
		this.Glanced = glanced;
		this.Crit = crit;
		this.Missed = missed;
	}
}
