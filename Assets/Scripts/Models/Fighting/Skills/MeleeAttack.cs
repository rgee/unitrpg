using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;
using Models.Fighting.Equip;
using Models.Fighting.Stats;

namespace Models.Fighting.Skills {
    public class MeleeAttack : AbstractSkillStrategy {
        public MeleeAttack() : base(SkillType.Melee, true, true) {
        }

        protected override SkillResult ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            var defenderEffects = new List<IEffect>();
            var firstHit = ComputeHit(attacker, defender, randomizer);
            defenderEffects.Add(firstHit);
            
            return new SkillResult(defenderEffects);
        }

        protected override ICombatBuffProvider GetBuffProvider(ICombatant attacker) {
            return attacker.EquippedWeapons.First(weapon => weapon.Range == 1);
        }

        private static WeaponHit ComputeHit(ICombatant attacker, ICombatant defender, IRandomizer random) {
            var hitChance = new HitChance(attacker, defender);
            var glanceChance = new GlanceChance(attacker, defender);
            var critChance = new CritChance(attacker, defender);
            if (random.GetNextRandom() < hitChance.Value) {
                var baseDamage = ComputeDamage(attacker, defender);
                if (random.GetNextRandom() < critChance.Value) {
                    baseDamage *= 2;     
                } else if (random.GetNextRandom() < glanceChance.Value) {
                    baseDamage /= 2;
                }
                
                return new WeaponHit(baseDamage);
            }
            
            return new Miss(MissReason.Miss);
        }
        
        private static int ComputeDamage(ICombatant attacker, ICombatant defender) {
            var strength = attacker.GetAttribute(Attribute.AttributeType.Strength);
            var defense = defender.GetAttribute(Attribute.AttributeType.Defense);
            return strength.Value - defense.Value;
        }
    }
}