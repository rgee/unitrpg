using System.Collections.Generic;
using System.Linq;

public class FightPhaseResult {
    public readonly List<Hit> AttackerHits;
    public readonly FightParameters AttackerParams;
    public readonly FightParameters CounterParams;
    public readonly List<Hit> DefenderHits;
    private readonly Participants Participants;

    public FightPhaseResult(Participants participants, FightParameters attackerParams, FightParameters counterParams,
                            List<Hit> attackerHits, List<Hit> defenderHits) {
        Participants = participants;
        AttackerParams = attackerParams;
        CounterParams = counterParams;
        AttackerHits = attackerHits;
        DefenderHits = defenderHits;
    }

    public int AttackerDamage {
        get { return AttackerParams.Damage; }
    }

    public int CounterDamage {
        get { return CounterParams.Damage; }
    }

    public bool AttackerDoubles {
        get { return AttackerHits.Count > 2; }
    }

    public bool DefenderDoubles {
        get { return DefenderHits.Count > 2; }
    }

    public bool AttackerDies {
        get {
            var trueCounterDamage = DefenderHits
                .Select(hit => { return hit.Damage; })
                .Sum();
            return !DefenderDies && Participants.Attacker.Health <= trueCounterDamage;
        }
    }

    public bool DefenderDies {
        get {
            var trueAttackerDamage = AttackerHits
                .Select(hit => { return hit.Damage; })
                .Sum();

            return Participants.Defender.Health <= trueAttackerDamage;
        }
    }
}