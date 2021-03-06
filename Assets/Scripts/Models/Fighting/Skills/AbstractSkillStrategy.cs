﻿using System;
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

        public abstract ICombatBuffProvider GetBuffProvider(ICombatant attacker, ICombatant defender);

        public abstract SkillEffects ComputeResult(ICombatant attacker, ICombatant defener, IRandomizer randomizer);

        public abstract SkillForecast ComputeForecast(ICombatant attacker, ICombatant defender);

        public abstract SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer);

        public SkillEffects FinalizeForecast(SkillForecast forecast, IRandomizer randomizer) {
            return ComputeBuffedResult(forecast.Attacker, forecast.Defender, () => ComputeEffects(forecast, randomizer));
        }

        public SkillForecast Forecast(ICombatant attacker, ICombatant defender) {
            return ComputeBuffedResult(attacker, defender, () => ComputeForecast(attacker, defender));
        }

        public SkillEffects Compute(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            return ComputeBuffedResult(attacker, defender, () => ComputeResult(attacker, defender, randomizer));
        }

        private T ComputeBuffedResult<T>(ICombatant attacker, ICombatant defender, Func<T> computation) {
            
            var buffProvider = GetBuffProvider(attacker, defender);
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