namespace Models.Fighting.Skills {
    public class SkillForecast {
        public SkillHit Hit { get; set; }
        public SkillChances Chances { get; set; }
        public ICombatant Attacker { get; set; }
        public ICombatant Defender { get; set; }
    }
}