using Models.Combat;

namespace Models.Fighting.Stats {
    public abstract class AdversarialStat : Stat {
        public ICombatant Initiator;
        public ICombatant Defender;

        protected AdversarialStat(ICombatant initiator, ICombatant defender) {
            Initiator = initiator;
            Defender = defender;
        }
    }
}