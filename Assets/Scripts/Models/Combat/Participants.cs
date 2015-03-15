using UnityEngine;
using System.Collections;

public struct Participants {
	public readonly Grid.Unit Attacker;
	public readonly Grid.Unit Defender;
	public Participants (Grid.Unit attacker, Grid.Unit defender)
	{
		this.Attacker = attacker;
		this.Defender = defender;
	}
	
	public Participants Invert() {
		return new Participants(Attacker, Defender);
	}
}
