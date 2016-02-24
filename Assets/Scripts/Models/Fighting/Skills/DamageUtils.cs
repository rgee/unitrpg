﻿using Models.Fighting.Effects;
using Models.Fighting.Stats;

namespace Models.Fighting.Skills {
    public static class DamageUtils {
        public static WeaponHit ComputeHit(ICombatant attacker, ICombatant defender, IRandomizer random) {
            var hitChance = new HitChance(attacker, defender);
            var glanceChance = new GlanceChance(attacker, defender);
            var critChance = new CritChance(attacker, defender);
            if (random.GetNextRandom() > (100 - hitChance.Value)) {
                var baseDamage = ComputeDamage(attacker, defender);
                if (random.GetNextRandom() > (100 - critChance.Value)) {
                    baseDamage *= 2;     
                } else if (random.GetNextRandom() > (100 - glanceChance.Value)) {
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