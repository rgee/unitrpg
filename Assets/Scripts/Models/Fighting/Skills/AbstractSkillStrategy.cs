using Models.Fighting.Stats;

namespace Models.Fighting.Skills {
    public abstract class AbstractSkillStrategy : ISkillStrategy {
        public bool SupportsFlanking { get; private set; }
        public bool SupportsDoubleAttack { get; private set; }

        protected AbstractSkillStrategy(bool supportsFlanking, bool supportsDoubleAttack) {
            SupportsFlanking = supportsFlanking;
            SupportsDoubleAttack = supportsDoubleAttack;
        }

        protected abstract ICombatBuffProvider GetBuffProvider(ICombatant attacker);

        protected abstract SkillResult ComputeResult(ICombatant attacker, ICombatant defener, IRandomizer randomizer);

        public SkillResult Compute(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            var buffProvider = GetBuffProvider(attacker);
            buffProvider.ReceiverPreCombatBuffs.ForEach(buff => defender.AddTemporaryBuff(buff));
            buffProvider.InitiatorPreCombatBuffs.ForEach(buff => attacker.AddTemporaryBuff(buff));

            var result = ComputeResult(attacker, defender, randomizer);

            buffProvider.ReceiverPreCombatBuffs.ForEach(buff => defender.RemoveTemporaryBuff(buff));
            buffProvider.InitiatorPreCombatBuffs.ForEach(buff => attacker.RemoveTemporaryBuff(buff));

            return result;
        }

        public bool DidDouble(ICombatant attacker, ICombatant defender) {
            var buffProvider = GetBuffProvider(attacker);
            buffProvider.ReceiverPreCombatBuffs.ForEach(buff => defender.AddTemporaryBuff(buff));
            buffProvider.InitiatorPreCombatBuffs.ForEach(buff => attacker.AddTemporaryBuff(buff));

            var result = new AttackCount(attacker, defender).Value > 1;

            buffProvider.ReceiverPreCombatBuffs.ForEach(buff => defender.RemoveTemporaryBuff(buff));
            buffProvider.InitiatorPreCombatBuffs.ForEach(buff => attacker.RemoveTemporaryBuff(buff));

            return result;
        }
    }
}