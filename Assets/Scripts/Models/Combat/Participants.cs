using UnityEngine;
using System.Collections;

public struct Participants {
	public readonly Models.Unit Attacker;
	public readonly Models.Unit Defender;
	public Participants (Models.Unit attacker, Models.Unit defender)
	{
		this.Attacker = attacker;
		this.Defender = defender;
	}
	
	public Participants Invert() {
		return new Participants(Attacker, Defender);
	}
}
