namespace Models.Fighting.Effects {
    public class Miss : WeaponHit {
        public MissReason Reason;

        public Miss(MissReason reason) : base(0) {
            Reason = reason;
        }
    }
}