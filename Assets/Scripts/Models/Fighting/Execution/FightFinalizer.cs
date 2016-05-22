using System.Linq;
using Models.Fighting.Effects;
using Models.Fighting.Skills;

namespace Models.Fighting.Execution {
    public class FightFinalizer {

        private readonly SkillDatabase _skillDatabase;

        public FightFinalizer(SkillDatabase skillDatabase) {
            _skillDatabase = skillDatabase;
        }

        public FinalizedFight Finalize(FightForecast forecast, IRandomizer randomizer) {
            var builder = new FinalizedFightBuilder();
            var firstPhase = ComputePhase(forecast.AttackerForecast, randomizer);
            builder.Initial(firstPhase);


            var receiverDamageTally = new DamageTallyer(firstPhase.Receiver);
            ApplyDamageFromPhase(firstPhase, receiverDamageTally);
            var receiverEffects = firstPhase.Effects.ReceiverEffects;
            var shouldSuppressCounter = receiverEffects.OfType<SuppressCounter>().Any();

            // The receiver survived the first hit
            if (!receiverDamageTally.IsDead() && !shouldSuppressCounter) {
                // It's possible there is no flanker
                if (forecast.FlankerForecast != null) {
                    var flankPhase = ComputePhase(forecast.FlankerForecast, randomizer);
                    builder.Flank(flankPhase);
                    ApplyDamageFromPhase(flankPhase, receiverDamageTally);
                }

                // If they survived the potential flank
                if (!receiverDamageTally.IsDead()) {
                    var initiatorDamageTally = new DamageTallyer(firstPhase.Initiator);

                    // The defender may counter first
                    if (forecast.DefenderForecast != null) {
                        var counterPhase = ComputePhase(forecast.DefenderForecast, randomizer);
                        builder.Counter(counterPhase);
                        ApplyDamageFromPhase(counterPhase, initiatorDamageTally);
                    }

                    // If the initiator survives the counter, they may double
                    if (!initiatorDamageTally.IsDead()) {
                        var firstHit = forecast.AttackerForecast.Hit;

                        // The defender doubles if the skill forecast says they hit twice
                        if (firstHit.HitCount > 1) {
                            var doublePhase = ComputePhase(forecast.AttackerForecast, randomizer);
                            ApplyDamageFromPhase(doublePhase, receiverDamageTally);
                            builder.Double(doublePhase);
                        }
                    }
                }
            }

            return builder.Build();
        }

        private static void ApplyDamageFromPhase(FightPhase phase, DamageTallyer tally) {
            tally.ApplyDamage(phase.Effects.GetDefenderDamage());
        }

        private FightPhase ComputePhase(SkillForecast skillForecast, IRandomizer randomizer) {
            var initiator = skillForecast.Attacker;
            var receiver = skillForecast.Defender;
            var skill = _skillDatabase.GetStrategyByType(skillForecast.Type);
            var effects = skill.FinalizeForecast(skillForecast, randomizer);
            var damage = effects.GetDefenderDamage();
            var receiverHealth = receiver.Health;
            var receiverDead = receiverHealth <= damage;

            return new FightPhase {
                Initiator = initiator,
                Receiver = receiver,
                ReceverDies = receiverDead,
                Response = DefenderResponse.GetHit, // TODO: Skill strategy responsible for this?
                Effects = effects,
                Skill = skillForecast.Type
            };
        }
    }
}