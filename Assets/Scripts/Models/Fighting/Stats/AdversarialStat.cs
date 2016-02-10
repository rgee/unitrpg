using Models.Combat;

namespace Models.Fighting.Stats {
    public abstract class AdversarialStat : IStat {
        public ICombatant Initiator;
        public ICombatant Defender;

        public abstract int Value { get; }

        public StatType Type { get; protected set; }

        protected AdversarialStat(ICombatant initiator, ICombatant defender) {
            Initiator = initiator;
            Defender = defender;
        }
    }
}