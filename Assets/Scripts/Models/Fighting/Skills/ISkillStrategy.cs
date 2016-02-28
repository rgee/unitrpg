namespace Models.Fighting.Skills {
    public interface ISkillStrategy {
        SkllEffects Compute(ICombatant attacker, ICombatant defener, IRandomizer randomizer);
        SkillType Type { get; }
        bool SupportsFlanking { get; }
        bool SupportsDoubleAttack { get; }

        bool DidDouble(ICombatant combatant, ICombatant defender);
    }
}