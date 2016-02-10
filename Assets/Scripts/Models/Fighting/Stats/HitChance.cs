using Models.Combat;

namespace Models.Fighting.Stats {
    public class HitChance : AdversarialStat {
        private const int BaseHitChance = 50;

        public override int Value {
            get {
                var theirSpeed = Defender.GetAttribute(Attribute.AttributeType.Speed).Value;
                var mySkill = Initiator.GetAttribute(Attribute.AttributeType.Skill).Value;

                return ((mySkill*3) + BaseHitChance) - theirSpeed;
            }
        }

        public HitChance(ICombatant initiator, ICombatant defender) : base(initiator, defender) {
            Type = StatType.HitChance;
        }
    }
}