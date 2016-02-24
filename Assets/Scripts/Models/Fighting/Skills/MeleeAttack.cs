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
            var firstHit = DamageUtils.ComputeHit(attacker, defender, randomizer);
            defenderEffects.Add(firstHit);
            
            return new SkillResult(defenderEffects);
        }

        protected override ICombatBuffProvider GetBuffProvider(ICombatant attacker) {
            return attacker.EquippedWeapons.First(weapon => weapon.Range == 1);
        }
    }
}