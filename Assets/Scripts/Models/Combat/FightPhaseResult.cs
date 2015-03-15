using UnityEngine;
using System.Collections;

public class FightPhaseResult {

	private readonly Participants Participants;

	public readonly int AttackerDamage;
	public readonly int DefenderDamage;
	
	public readonly FightParameters AttackerParams;
	public readonly FightParameters DefenderParams;

	public bool AttackerDies {
		get {
			return !DefenderDies && Participants.Attacker.model.Health <= DefenderDamage;
		}
	}

	public bool DefenderDies {
		get {
			return Participants.Defender.model.Health <= AttackerDamage;
		}
	}

	public FightPhaseResult (Participants participants, 
	                         int attackerDamage, int defenderDamage,
	                         FightParameters attackerParams, FightParameters defenderParams)
	{
		this.Participants = participants;
		this.AttackerDamage = attackerDamage;
		this.DefenderDamage = defenderDamage;
		this.AttackerParams = attackerParams;
		this.DefenderParams = defenderParams;
	}
}
