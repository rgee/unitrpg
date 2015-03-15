using UnityEngine;
using System;
using System.Collections;

public class DefaultFightResolution : ResolutionStrategy {
	public FightPhaseResult SimulateFightPhase (Participants participants, AttackType attack) {
		FightParameters attackParams = ComputeAttackParams(participants);
		FightParameters counterParams = ComputeCounterParams(participants);

		return new FightPhaseResult(
			participants,
			attackParams,
			counterParams,
			SimulateParams(attackParams),
			SimulateParams(counterParams)
		);
	}

	private int SimulateParams(FightParameters parameters) {
		int result = ComputeActualDamage(parameters);
		if (parameters.Hits == 2) {
			result += ComputeActualDamage(parameters);
		}

		return result;
	}


	private int ComputeActualDamage(FightParameters parameters) {
		int actualDamage = parameters.Damage;
		bool didHit =  RollDice() > parameters.HitChance;
		if (!didHit) {
			actualDamage = 0;
		} else {
			bool didCrit = RollDice() > parameters.CritChance;
			bool didGlance = RollDice() > parameters.GlanceChance;

			// Crits cannot also glance.
			if (didCrit) {
				actualDamage *= 2;
			} else if (didGlance) {
				actualDamage /= 2;
			}
		}
		
		return actualDamage;
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

		int numHits = atkChar.Speed - defChar.Speed > 10 ? 2 : 1;
		int hitChance = Percentage((atkChar.Skill + 50) - defChar.Speed);
		int critChance = Percentage(atkChar.Skill - defChar.Speed);
		int glanceChance = Percentage(atkChar.Skill - defChar.Skill);
		int damage = Math.Abs(defChar.Defense - atkChar.Strength);

		return new FightParameters(
			damage,
			numHits,
			hitChance,
			critChance,
			glanceChance
		);
	}

	private static int Percentage(int val) {
		return Math.Min(val, 100);
	}

	private static int RollDice() {
		return (int)(UnityEngine.Random.value * 100);
	}
}
