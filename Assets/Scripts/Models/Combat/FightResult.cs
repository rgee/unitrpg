using UnityEngine;
using System.Collections;

public class FightResult {
	public readonly FightPhaseResult InitialAttack;
	public readonly FightPhaseResult CounterAttack;

	public FightResult(FightPhaseResult initialAttack) {
		InitialAttack = initialAttack;
		CounterAttack = null;
	}

	public FightResult(FightPhaseResult initialAttack, FightPhaseResult counter) {
		InitialAttack = initialAttack;
		CounterAttack = counter;
	}
}
