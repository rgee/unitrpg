namespace Models.Fighting.Skills {
    public interface ISkillStrategy {
        SkillResult Compute(Skill skill, IRandomizer randomizer);
    }
}