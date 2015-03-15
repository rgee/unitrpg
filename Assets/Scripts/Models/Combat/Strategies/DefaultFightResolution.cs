using UnityEngine;
using System;
using System.Collections;

public class DefaultFightResolution : ResolutionStrategy {

	public FightPhaseResult SimulateFightPhase (Participants participants, AttackType attack) {
		return new FightPhaseResult(
			participants,
			ComputeAttackParams(participants),
			ComputeCounterParams(participants)
		);
	}

	private FightParameters ComputeAttackParams(Participants participants) {
		return ComputeParams(participants.Attacker, participants.Defender);
	}

	private FightParameters ComputeCounterParams(Participants participants) {
		return ComputeParams(participants.Defender, participants.Defender);
	}

	private FightParameters ComputeParams(Models.Unit attacker, Models.Unit defender) {
		Models.Character atkChar = attacker.Character;
		Models.Character defChar = defender.Character;

		int damage = Math.Abs(defChar.Defense - atkChar.Strength);
		int numHits = atkChar.Speed - defChar.Speed > 10 ? 2 : 1;
		int hitChance = percentage((atkChar.Skill + 50) - defChar.Speed);
		int critChance = percentage(atkChar.Skill - defChar.Speed);
		int glanceChance = percentage(atkChar.Skill - defChar.Skill);

		return new FightParameters(
			damage,
			numHits,
			hitChance,
			critChance,
			glanceChance
		);
	}

	private static int percentage(int val) {
		return Math.Min(val, 100);
	}
}
