using System.Net;
using Models.Combat;

namespace Models.Fighting.Stats {
    public class AttackCount : AdversarialStat {
        private const int SpeedDifferential = 10;
        public override int Value {
            get {
                var attackerSpeed = Initiator.GetAttribute(Attribute.AttributeType.Speed).Value;
                var defenderSpeed = Defender.GetAttribute(Attribute.AttributeType.Speed).Value;

                if ((attackerSpeed - defenderSpeed) >= SpeedDifferential) {
                    return 2;
                }

                return 1;
            }
        }

        public AttackCount(ICombatant initiator, ICombatant defender) : base(initiator, defender) {
            Type = StatType.AttackCount;
        }
    }
}