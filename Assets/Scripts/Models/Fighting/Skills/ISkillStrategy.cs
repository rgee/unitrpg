namespace Models.Fighting.Skills {
    public interface ISkillStrategy {
        SkillResult Compute(ICombatant attacker, ICombatant defener, ICombatBuffProvider buffProvider, IRandomizer randomizer);
    }
}