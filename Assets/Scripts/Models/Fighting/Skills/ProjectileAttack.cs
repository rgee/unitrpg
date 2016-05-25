using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;
using Models.Fighting.Stats;

namespace Models.Fighting.Skills {
    public class ProjectileAttack : AbstractSkillStrategy {
        public ProjectileAttack() : base(SkillType.Ranged, true, true) {
        }

        public override SkillForecast ComputeForecast(ICombatant attacker, ICombatant defender) {
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

        public override SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer) {
            var defender = forecast.Defender;
            var parryChance = defender.GetStat(StatType.ProjectileParryChance);
            var didParry = RandomUtils.DidEventHappen(parryChance.Value, randomizer);
            if (didParry) {
                return new SkillEffects(
                    new List<IEffect> { new Miss(MissReason.Parry) }
                );
            }

            var baseDamage = forecast.Hit.BaseDamage;
            var hit = DamageUtils.GetFinalizedPhysicalDamage(baseDamage, forecast.Chances, randomizer);
            return new SkillEffects(new List<IEffect>() { hit });
        }

        public override ICombatBuffProvider GetBuffProvider(ICombatant attacker, ICombatant defender) {
            return attacker.EquippedWeapons.First(weapon => weapon.Range >= 1);
        }

        public override SkillEffects ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            var parryChance = defender.GetStat(StatType.ProjectileParryChance);
            var didParry = randomizer.GetNextRandom() < (100 - parryChance.Value);

            if (didParry) {
                return new SkillEffects(
                    new List<IEffect> { new Miss(MissReason.Parry) }
                );
            }

            var firstHit = DamageUtils.ComputeHit(attacker, defender, randomizer);
            return new SkillEffects(new List<IEffect>() { firstHit });
        }
    }
}