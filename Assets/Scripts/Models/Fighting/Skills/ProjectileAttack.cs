using System.Collections.Generic;
using Models.Fighting.Effects;
using Models.Fighting.Equip;

namespace Models.Fighting.Skills {
    public class ProjectileAttack : AbstractSkillStrategy {
        public ProjectileAttack() : base(true, true) {
        }

        protected override ICombatBuffProvider GetBuffProvider(ICombatant attacker) {
            return WeaponDatabase.Instance.GetByName(attacker.PrimaryWeapon);
        }

        protected override SkillResult ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            var parryChance = defender.GetStat(StatType.ProjectileParryChance);
            var didParry = randomizer.GetNextRandom() < parryChance.Value;

            if (didParry) {
                return new SkillResult(
                    new List<IEffect> { new Miss(MissReason.Parry) }
                );
            }

            // TODO: Finish ranged weapon damage/hit computation
            return new SkillResult(
                new List<IEffect> { new WeaponHit(0) }
            );
        }
    }
}