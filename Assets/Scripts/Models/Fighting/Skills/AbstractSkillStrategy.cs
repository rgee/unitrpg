using System;
using System.Linq;
using Models.Fighting.Stats;
using WellFired.Shared;

namespace Models.Fighting.Skills {
    public abstract class AbstractSkillStrategy : ISkillStrategy {
        public bool SupportsFlanking { get; private set; }
        public bool SupportsDoubleAttack { get; private set; }
        public SkillType Type { get; private set; }

        protected AbstractSkillStrategy(SkillType type, bool supportsFlanking, bool supportsDoubleAttack) {
            Type = type;
            SupportsFlanking = supportsFlanking;
            SupportsDoubleAttack = supportsDoubleAttack;
        }

        protected abstract ICombatBuffProvider GetBuffProvider(ICombatant attacker);

        protected abstract SkillEffects ComputeResult(ICombatant attacker, ICombatant defener, IRandomizer randomizer);

        protected abstract SkillForecast ComputeForecast(ICombatant attacker, ICombatant defender);

        public SkillForecast Forecast(ICombatant attacker, ICombatant defender) {
            return ComputeBuffedResult(attacker, defender, () => Forecast(attacker, defender));
        }

        public SkillEffects Compute(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            return ComputeBuffedResult(attacker, defender, () => ComputeResult(attacker, defender, randomizer));
        }

        private T ComputeBuffedResult<T>(ICombatant attacker, ICombatant defender, Func<T> computation) {
            
            var buffProvider = GetBuffProvider(attacker);
            buffProvider.ReceiverPreCombatBuffs
                .Where(buff => buff.AppliesToSkill(Type))
                .Each(buff => defender.AddTemporaryBuff(buff));

            buffProvider.InitiatorPreCombatBuffs
                .Where(buff => buff.AppliesToSkill(Type))
                .Each(buff => attacker.AddTemporaryBuff(buff));

            var result = computation();

            buffProvider.ReceiverPreCombatBuffs
                .Where(buff => buff.AppliesToSkill(Type))
                .Each(buff => defender.RemoveTemporaryBuff(buff));

            buffProvider.InitiatorPreCombatBuffs
                .Where(buff => buff.AppliesToSkill(Type))
                .Each(buff => attacker.RemoveTemporaryBuff(buff));

            return result;
        }

        public bool DidDouble(ICombatant attacker, ICombatant defender) {
            if (!SupportsDoubleAttack) {
                return false;
            }

            return ComputeBuffedResult(attacker, defender, () => new AttackCount(attacker, defender).Value > 1);
        }
    }
}