using Models.Combat;
using UnityEngine;

namespace Models.Fighting.Stats {
    public class GlanceChance : AdversarialStat {
        public override int Value {
            get {
                var mySkill = Initiator.GetAttribute(Attribute.AttributeType.Skill).Value;
                var theirSkill = Defender.GetAttribute(Attribute.AttributeType.Skill).Value;
                var result = mySkill - theirSkill;
                return Mathf.Clamp(result, 0, 100);
            }
        }

        public GlanceChance(ICombatant initiator, ICombatant defender) : base(initiator, defender) {
            Type = StatType.GlanceChance;
        }
    }
}