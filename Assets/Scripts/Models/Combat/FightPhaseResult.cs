using System.Collections.Generic;
using System.Linq;

public class FightPhaseResult {
    public readonly List<Hit> AttackerHits;
    public readonly FightParameters AttackerParams;
    public readonly FightParameters CounterParams;
    public readonly List<Hit> DefenderHits;
    private readonly Participants _participants;

    public FightPhaseResult(Participants participants, FightParameters attackerParams, FightParameters counterParams,
                            List<Hit> attackerHits, List<Hit> defenderHits) {
        _participants = participants;
        AttackerParams = attackerParams;
        CounterParams = counterParams;
        AttackerHits = attackerHits;
        DefenderHits = defenderHits;
    }

    public int AttackerDamage => AttackerParams.Damage;

    public int CounterDamage => CounterParams.Damage;

    public bool AttackerDoubles => AttackerHits.Count > 2;

    public bool DefenderDoubles => DefenderHits.Count > 2;

    public bool AttackerDies {
        get {
            var trueCounterDamage = DefenderHits
                .Select(hit => hit.Damage)
                .Sum();
            return !DefenderDies && _participants.Attacker.Health <= trueCounterDamage;
        }
    }

    public bool DefenderDies {
        get {
            var trueAttackerDamage = AttackerHits
                .Select(hit => hit.Damage)
                .Sum();

            return _participants.Defender.Health <= trueAttackerDamage;
        }
    }
}