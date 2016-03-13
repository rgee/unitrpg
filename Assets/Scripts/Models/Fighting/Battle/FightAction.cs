using Models.Fighting.Execution;
using Models.Fighting.Skills;
using FightExecutor = Models.Fighting.Execution.FightExecutor;

namespace Models.Fighting.Battle {
    public class FightAction : ICombatAction {
        private readonly ICombatant _attacker;
        private readonly ICombatant _defender;
        private readonly FinalizedFight _fight;

        public bool IsValid(Turn turn) {
            return turn.CanAct(_attacker);
        }

        public void Perform(Turn turn) {
            var executor = new Execution.FightExecutor();
            executor.Execute(_fight);

            turn.MarkAction(_attacker);
        }
    }
}