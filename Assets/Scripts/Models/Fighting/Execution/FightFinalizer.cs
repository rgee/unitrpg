using System.Linq;
using Models.Fighting.Effects;
using Models.Fighting.Skills;
using UnityEngine;

namespace Models.Fighting.Execution {
    public class FightFinalizer {

        private readonly SkillDatabase _skillDatabase;

        public FightFinalizer(SkillDatabase skillDatabase) {
            _skillDatabase = skillDatabase;
        }

        public FinalizedFight Finalize(FightForecast forecast, IRandomizer randomizer) {
            var builder = new FinalizedFightBuilder();
            var receiverDamageTally = new DamageTallyer(forecast.AttackerForecast.Defender);
            var firstPhase = ComputePhase(forecast.AttackerForecast, receiverDamageTally, randomizer);
            builder.Initial(firstPhase);


            var receiverEffects = firstPhase.Effects.ReceiverEffects;
            var shouldSuppressCounter = receiverEffects.OfType<SuppressCounter>().Any();

            // The receiver survived the first hit
            if (!receiverDamageTally.IsDead() && !shouldSuppressCounter) {
                // It's possible there is no flanker
                if (forecast.FlankerForecast != null) {
                    var flankPhase = ComputePhase(forecast.FlankerForecast, receiverDamageTally, randomizer);
                    builder.Flank(flankPhase);
                }

                // If they survived the potential flank
                if (!receiverDamageTally.IsDead()) {
                    var initiatorDamageTally = new DamageTallyer(firstPhase.Initiator);

                    // The defender may counter first
                    if (forecast.DefenderForecast != null) {
                        var counterPhase = ComputePhase(forecast.DefenderForecast, receiverDamageTally, randomizer);
                        builder.Counter(counterPhase);
                    }

                    // If the initiator survives the counter, they may double
                    if (!initiatorDamageTally.IsDead()) {
                        var firstHit = forecast.AttackerForecast.Hit;

                        // The defender doubles if the skill forecast says they hit twice
                        if (firstHit.HitCount > 1) {
                            var doublePhase = ComputePhase(forecast.AttackerForecast, receiverDamageTally, randomizer);
                            builder.Double(doublePhase);
                        }
                    }
                }
            }

            return builder.Build();
        }

        private FightPhase ComputePhase(SkillForecast skillForecast, DamageTallyer damageSoFar, IRandomizer randomizer) {
            var initiator = skillForecast.Attacker;
            var receiver = skillForecast.Defender;
            var skill = _skillDatabase.GetStrategyByType(skillForecast.Type);
            var effects = skill.FinalizeForecast(skillForecast, randomizer);
            var didMiss = effects.ReceiverEffects.OfType<Miss>().Any();

            damageSoFar.ApplyDamage(effects.GetDefenderDamage());

            return new FightPhase {
                Initiator = initiator,
                Receiver = receiver,
                ReceverDies = damageSoFar.IsDead(),
                Response = didMiss ? DefenderResponse.Dodge : DefenderResponse.GetHit, 
                Effects = effects,
                Skill = skillForecast.Type
            };
        }
    }
}