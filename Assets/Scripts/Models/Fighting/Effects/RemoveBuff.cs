namespace Models.Fighting.Effects {
    public class RemoveBuff : IEffect {
        private readonly string _buffName;

        public RemoveBuff(string buffName) {
            _buffName = buffName;
        }

        public void Apply(ICombatant combatant) {
            combatant.RemoveBuff(_buffName);
        }
    }
}