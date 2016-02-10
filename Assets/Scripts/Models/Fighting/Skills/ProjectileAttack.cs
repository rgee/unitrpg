using System.Collections.Generic;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class ProjectileAttack : ISkillStrategy {
        public SkillResult Compute(Skill skill, IRandomizer randomizer) {
            var defender = skill.Receiver;
            var parryChance = defender.GetStat(StatType.ProjectileParryChance).Value;
            var didParry = randomizer.GetNextRandom() < parryChance;

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