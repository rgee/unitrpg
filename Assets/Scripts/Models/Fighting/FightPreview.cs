namespace Models.Fighting {
    public class FightPreview {
        public AttackPreview InitiatorAttack { get; private set; }
        public AttackPreview DefenderAttack { get; private set; }
        
        public FightPreview(AttackPreview init, AttackPreview def) {
            InitiatorAttack = init;
            DefenderAttack = def;
        }
    }
}