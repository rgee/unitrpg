using UnityEngine;
using System;

public struct Hit {
	public readonly int Damage;
	public readonly bool Glanced;
	public readonly bool Crit;

	public Hit (int damage, bool glanced, bool crit)
	{
		this.Damage = damage;
		this.Glanced = glanced;
		this.Crit = crit;
	}
}
