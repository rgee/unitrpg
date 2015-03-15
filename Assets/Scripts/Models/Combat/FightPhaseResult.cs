using UnityEngine;
using System.Collections;

public class FightPhaseResult {

	private readonly Participants Participants;

	public int AttackerDamage {
		get {
			return AttackerParams.Damage;
		}
	}

	public int DefenderDamage {
		get {
			return CounterParams.Damage;
		}
	}
	
	public readonly FightParameters AttackerParams;
	public readonly FightParameters CounterParams;

	public bool AttackerDies {
		get {
			return !DefenderDies && Participants.Attacker.Health <= DefenderDamage;
		}
	}

	public bool DefenderDies {
		get {
			return Participants.Defender.Health <= AttackerDamage;
		}
	}

	public FightPhaseResult (Participants participants, FightParameters attackerParams, FightParameters counterParams)
	{
		this.Participants = participants;
		this.AttackerParams = attackerParams;
		this.CounterParams = counterParams;
	}
}
