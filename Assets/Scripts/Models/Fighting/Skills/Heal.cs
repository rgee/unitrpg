using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;
using Models.Fighting.Equip;

namespace Models.Fighting.Skills {
    public class Heal : AbstractSkillStrategy {
        public Heal() : base(SkillType.Heal, false, false) {
        }

        protected override ICombatBuffProvider GetBuffProvider(ICombatant attacker) {
            return attacker.EquippedWeapons.First(weapon => weapon.Range == 1);
        }

        protected override SkllEffects ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) { 
            var mySkill = attacker.GetAttribute(Attribute.AttributeType.Skill).Value;
            return new SkllEffects(
                new List<IEffect> {
                    new Damage(-mySkill)
                }
            );
        }
    }
}