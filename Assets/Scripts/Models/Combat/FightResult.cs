using UnityEngine;
using System.Collections;

public class FightResult {
	public readonly FightPhaseResult InitialAttack;
	public readonly FightPhaseResult CounterAttack;
	public readonly Participants Participants;

	public FightResult(FightPhaseResult initialAttack, Participants participants) {
		InitialAttack = initialAttack;
		CounterAttack = null;
		Participants = participants;
	}

	public FightResult(FightPhaseResult initialAttack, FightPhaseResult counter, Participants participants) {
		InitialAttack = initialAttack;
		CounterAttack = counter;
		Participants = participants;
	}
}
