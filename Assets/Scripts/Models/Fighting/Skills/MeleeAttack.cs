using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Stats;

namespace Models.Fighting.Skills {
    public class MeleeAttack : AbstractSkillStrategy {
        public MeleeAttack() : base(SkillType.Melee, true, true) {
        }

        protected MeleeAttack(SkillType type, bool supportsFlanking, bool supportsDoubles) : base(type, supportsFlanking, supportsDoubles) {
            
        }

        protected override SkillForecast ComputeForecast(ICombatant attacker, ICombatant defender) {
            var hitChance = new HitChance(attacker, defender);
            var critChance = new CritChance(attacker, defender);
            var glanceChance = new GlanceChance(attacker, defender);

            var chances = new SkillChances {
                CritChance = critChance.Value,
                HitChance = hitChance.Value,
                GlanceChance = glanceChance.Value
            };

            var attackCount = new AttackCount(attacker, defender);

            var hit = new SkillHit {
                BaseDamage = DamageUtils.ComputeMeleeDamage(attacker, defender),
                HitCount = attackCount.Value
            };

            return new SkillForecast {
                Type = Type,
                Hit = hit,
                Chances = chances,
                Attacker = attacker,
                Defender = defender
            };
        }

        protected override SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer) {
            var chances = forecast.Chances;
            var effects = new List<IEffect> {
                DamageUtils.GetFinalizedPhysicalDamage(forecast.Hit.BaseDamage, chances, randomizer)
            };

            return new SkillEffects(effects);
        }

        protected override SkillEffects ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            var defenderEffects = new List<IEffect>();
            var firstHit = DamageUtils.ComputeHit(attacker, defender, randomizer);
            defenderEffects.Add(firstHit);
            
            return new SkillEffects(defenderEffects);
        }

        protected override ICombatBuffProvider GetBuffProvider(ICombatant attacker) {
            return attacker.EquippedWeapons.First(weapon => weapon.Range == 1);
        }
    }
}