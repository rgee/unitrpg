using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;
using Models.Fighting.Equip;

namespace Models.Fighting.Skills {
    public class ProjectileAttack : AbstractSkillStrategy {
        public ProjectileAttack() : base(SkillType.Ranged, true, true) {
        }

        protected override ICombatBuffProvider GetBuffProvider(ICombatant attacker) {
            return attacker.EquippedWeapons.First(weapon => weapon.Range >= 1);
        }

        protected override SkllEffects ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            var parryChance = defender.GetStat(StatType.ProjectileParryChance);
            var didParry = randomizer.GetNextRandom() < (100 - parryChance.Value);

            if (didParry) {
                return new SkllEffects(
                    new List<IEffect> { new Miss(MissReason.Parry) }
                );
            }

            var firstHit = DamageUtils.ComputeHit(attacker, defender, randomizer);
            return new SkllEffects(new List<IEffect>() { firstHit });
        }
    }
}