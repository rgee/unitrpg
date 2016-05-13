using System;
using Models.Combat;
using UnityEngine;

namespace Models.Fighting.Stats {
    public class CritChance : AdversarialStat {
        public override int Value {
            get {
                var mySkill = Initiator.GetAttribute(Attribute.AttributeType.Skill).Value;
                var theirSpeed = Defender.GetAttribute(Attribute.AttributeType.Speed).Value;
                var result = mySkill - theirSpeed;
                return Mathf.Clamp(result, 0, 100);
            }
        }

        public CritChance(ICombatant initiator, ICombatant defender) : base(initiator, defender) {
            Type = StatType.CritChance;
        }
    }
}