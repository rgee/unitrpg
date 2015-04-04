using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

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

	public readonly List<Hit> AttackerHits;
	public readonly List<Hit> DefenderHits;

	public bool AttackerDoubles {
		get {
			return AttackerHits.Count > 2;
		}
	}

	public bool DefenderDoubles {
		get {
			return DefenderHits.Count > 2;
		}
	}

	public bool AttackerDies {
		get {
            int trueCounterDamage = DefenderHits
                .Select((hit) => { return hit.Damage; })
                .Sum();
			return !DefenderDies && Participants.Attacker.Health <= trueCounterDamage;
		}
	}

	public bool DefenderDies {
		get {
            int trueAttackerDamage = AttackerHits
                .Select((hit) => { return hit.Damage; })
                .Sum();

            return Participants.Defender.Health <= trueAttackerDamage;
		}
	}

	public FightPhaseResult (Participants participants, FightParameters attackerParams, FightParameters counterParams, 
	                         List<Hit> attackerHits, List<Hit> defenderHits)
	{
		this.Participants = participants;
		this.AttackerParams = attackerParams;
		this.CounterParams = counterParams;
		this.AttackerHits = attackerHits;
		this.DefenderHits = defenderHits;
	}
}
