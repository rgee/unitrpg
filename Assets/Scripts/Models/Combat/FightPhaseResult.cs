using UnityEngine;
using System.Collections;

public class FightPhaseResult {

	private readonly Participants Participants;

	public int AttackerDamage {
		get {
			return AttackerParams.Damage;
		}
	}

	public int CounterDamage {
		get {
			return CounterParams.Damage;
		}
	}
	
	public readonly FightParameters AttackerParams;
	public readonly FightParameters CounterParams;
	public readonly int ActualAttackerDamage;
	public readonly int ActualDefenderDamage;

	public bool AttackerDies {
		get {
			return !DefenderDies && Participants.Attacker.Health <= CounterDamage;
		}
	}

	public bool DefenderDies {
		get {
			return Participants.Defender.Health <= AttackerDamage;
		}
	}

	public FightPhaseResult (Participants participants, FightParameters attackerParams, FightParameters counterParams, 
	                         int actualAttackerDamage, int actualDefenderDamage)
	{
		this.Participants = participants;
		this.AttackerParams = attackerParams;
		this.CounterParams = counterParams;
		this.ActualAttackerDamage = actualAttackerDamage;
		this.ActualDefenderDamage = actualDefenderDamage;
	}
}
