using System.Collections.Generic;
using Models.Fighting.Effects;
using Models.Fighting.Equip;

namespace Models.Fighting.Skills {
    public class Heal : AbstractSkillStrategy {
        public Heal() : base(SkillType.Heal, false, false) {
        }

        protected override ICombatBuffProvider GetBuffProvider(ICombatant attacker) {
            return WeaponDatabase.Instance.GetByName(attacker.PrimaryWeapon);
        }

        protected override SkillResult ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) { 
            var mySkill = attacker.GetAttribute(Attribute.AttributeType.Skill).Value;
            return new SkillResult(
                new List<IEffect> {
                    new Damage(-mySkill)
                }
            );
        }
    }
}