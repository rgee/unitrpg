using UnityEngine;
using System.Collections;

public class Fight {
	public readonly AttackType Attack;
	public readonly Participants Participants;
	public readonly ResolutionStrategy Strategy;

	public Fight(Participants participants, AttackType type, ResolutionStrategy strategy) {
		Attack = type;
		Participants = participants;
		Strategy = strategy;
	}

	public FightResult SimulateFight() {
		FightPhaseResult initiatorAttack = Strategy.SimulateFightPhase(Participants, Attack);
		if (initiatorAttack.AttackerDies) {
			return new FightResult(initiatorAttack, Participants);
		}

		FightPhaseResult counterAttack = Strategy.SimulateFightPhase(Participants.Invert(), Attack);
		return new FightResult(initiatorAttack, counterAttack, Participants);
	}
}
