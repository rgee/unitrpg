using System.Collections.Generic;
using Models.Fighting.Effects;
using Models.Fighting.Stats;

namespace Models.Fighting.Skills {
    public class MeleeAttack : ISkillStrategy {
        public SkillResult Compute(Skill skill, IRandomizer randomizer) {
            var attacker = skill.Initiator;
            var defender = skill.Receiver;
            var attackCount = new AttackCount(attacker, defender);
            
            var defenderEffects = new List<IEffect>();
            var attackerEffects = new List<IEffect>();
            var firstHit = ComputeHit(attacker, defender, randomizer);
            defenderEffects.Add(firstHit);
            
            if (defender.Health > firstHit.Amount) {
                var counter = ComputeHit(defender, attacker, randomizer);
                attackerEffects.Add(counter);
                
                if (attacker.Health > counter.Amount && attackCount.Value > 1) {
                    var doubleHit = ComputeHit(attacker, defender, randomizer);
                    defenderEffects.Add(doubleHit);
                }
            }
  
            return new SkillResult(defenderEffects, attackerEffects);
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