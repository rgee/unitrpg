using System.Collections.Generic;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class ProjectileAttack : AbstractSkillStrategy {
        protected override SkillResult Compute(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            var parryChance = defender.GetStat(StatType.ProjectileParryChance);
            var didParry = randomizer.GetNextRandom() < parryChance.Value;

            if (didParry) {
                return new SkillResult(
                    new List<IEffect>(), 
                    new List<IEffect> { new Miss(MissReason.Parry) }
                );
            }

            // TODO: Finish ranged weapon damage/hit computation
            return new SkillResult(
                new List<IEffect>(), 
                new List<IEffect> { new WeaponHit(0) }
            );
        }
    }
}