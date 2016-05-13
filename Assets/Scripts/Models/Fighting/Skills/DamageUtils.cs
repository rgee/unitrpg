using Models.Fighting.Effects;
using Models.Fighting.Stats;

namespace Models.Fighting.Skills {
    public static class DamageUtils {
        public static WeaponHit ComputeHit(ICombatant attacker, ICombatant defender, IRandomizer random) {
            var hitChance = new HitChance(attacker, defender);
            var glanceChance = new GlanceChance(attacker, defender);
            var critChance = new CritChance(attacker, defender);
            if (random.GetNextRandom() > (100 - hitChance.Value)) {
                var baseDamage = ComputeMeleeDamage(attacker, defender);
                if (random.GetNextRandom() > (100 - critChance.Value)) {
                    baseDamage *= 2;     
                } else if (random.GetNextRandom() > (100 - glanceChance.Value)) {
                    baseDamage /= 2;
                }
                
                return new WeaponHit(baseDamage);
            }
            
            return new Miss(MissReason.Miss);
        }

        /// <summary>
        /// Get the actual melee damage effect after glance, crit, hit and stuff are accounted for.
        /// </summary>
        /// <param name="baseDamage">The raw damage</param>
        /// <param name="chances">The chances for other things to modify it</param>
        /// <param name="randomizer">A randomizer</param>
        /// <returns></returns>
        public static WeaponHit GetFinalizedPhysicalDamage(int baseDamage, SkillChances chances, IRandomizer randomizer) {
            // MISS :<
            if (!RandomUtils.DidEventHappen(chances.HitChance, randomizer)) {
                return new Miss(MissReason.Miss);
            }

            // CRIT :)
            if (RandomUtils.DidEventHappen(chances.CritChance, randomizer)) {
                return new WeaponHit(baseDamage * 2);
            }


            // GLANCE :(
            if (RandomUtils.DidEventHappen(chances.GlanceChance, randomizer)) {
                return new WeaponHit(baseDamage / 2);
            }


            // BASE :|
            return new WeaponHit(baseDamage);
        }
        
        public static int ComputeMeleeDamage(ICombatant attacker, ICombatant defender) {
            var strength = attacker.GetAttribute(Attribute.AttributeType.Strength);
            var defense = defender.GetAttribute(Attribute.AttributeType.Defense);
            return strength.Value - defense.Value;
        }
    }
}