namespace Models.Fighting.Skills {
    public abstract class AbstractSkillStrategy : ISkillStrategy {
        protected abstract SkillResult Compute(ICombatant attacker, ICombatant defener, IRandomizer randomizer);

        public SkillResult Compute(ICombatant attacker, ICombatant defender, ICombatBuffProvider buffProvider, IRandomizer randomizer) {

            buffProvider.ReceiverPreCombatBuffs.ForEach(buff => defender.AddTemporaryBuff(buff));
            buffProvider.InitiatorPreCombatBuffs.ForEach(buff => attacker.AddTemporaryBuff(buff));

            var result = Compute(attacker, defender, randomizer);

            buffProvider.ReceiverPreCombatBuffs.ForEach(buff => defender.RemoveTemporaryBuff(buff));
            buffProvider.InitiatorPreCombatBuffs.ForEach(buff => attacker.RemoveTemporaryBuff(buff));

            return result;
        }
    }
}