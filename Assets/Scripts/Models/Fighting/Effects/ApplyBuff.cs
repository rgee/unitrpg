namespace Models.Fighting.Effects {
    public class ApplyBuff : IEffect {
        private readonly IBuff _buff;

        public ApplyBuff(IBuff buff) {
            _buff = buff;
        }

        public void Apply(ICombatant combatant) {
            combatant.AddBuff(_buff);
        }
    }
}