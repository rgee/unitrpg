using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class DefaultFightResolution : ResolutionStrategy {
    public FightPhaseResult SimulateFightPhase(Participants participants, AttackType attack) {
        var attackParams = ComputeAttackParams(participants);
        var counterParams = ComputeCounterParams(participants);

        return new FightPhaseResult(
            participants,
            attackParams,
            counterParams,
            SimulateParams(attackParams),
            SimulateParams(counterParams)
            );
    }

    private List<Hit> SimulateParams(FightParameters parameters) {
        var firstHit = ComputeActualDamage(parameters);
        var result = new List<Hit>();
        result.Add(firstHit);

        if (parameters.Hits == 2) {
            result.Add(ComputeActualDamage(parameters));
        }

        return result;
    }

    private Hit ComputeActualDamage(FightParameters parameters) {
        var crit = false;
        var glance = false;
        var missed = false;
        var actualDamage = parameters.Damage;
        var didHit = RollDice() < parameters.HitChance;
        if (!didHit) {
            actualDamage = 0;
            missed = true;
        } else {
            var didCrit = RollDice() < parameters.CritChance;
            var didGlance = RollDice() < parameters.GlanceChance;

            // Crits cannot also glance.
            if (didCrit) {
                crit = true;
                actualDamage *= 2;
            } else if (didGlance) {
                glance = true;
                actualDamage /= 2;
            }
        }

        return new Hit(actualDamage, glance, crit, missed);
    }

    private FightParameters ComputeAttackParams(Participants participants) {
        return ComputeParams(participants.Attacker, participants.Defender);
    }

    private FightParameters ComputeCounterParams(Participants participants) {
        return ComputeParams(participants.Defender, participants.Attacker, false);
    }

    private FightParameters ComputeParams(Models.Combat.Unit attacker, Models.Combat.Unit defender, bool canDouble = true) {
        var atkChar = attacker.Character;
        var defChar = defender.Character;

        var numHits = canDouble ? (atkChar.Speed - defChar.Speed > 10 ? 2 : 1) : 1;
        var hitChance = Percentage(((atkChar.Skill*3) + 50) - defChar.Speed);
        var critChance = Percentage(atkChar.Skill - defChar.Speed);
        var glanceChance = Percentage(atkChar.Skill - defChar.Skill);
        var damage = Math.Max(Math.Abs(defChar.Defense - atkChar.Strength), 0);

        return new FightParameters(
            damage,
            numHits,
            hitChance,
            critChance,
            glanceChance
            );
    }

    private static int Percentage(int val) {
        return Math.Min(Math.Max(val, 0), 100);
    }

    private static int RollDice() {
        return (int) (Random.value*100);
    }
}