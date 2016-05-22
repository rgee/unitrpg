using Models.Fighting.Execution;
using Models.Fighting.Skills;
using FightExecutor = Models.Fighting.Execution.FightExecutor;

namespace Models.Fighting.Battle {
    public class FightAction : ICombatAction {
        private readonly ICombatant _attacker;
        private readonly ICombatant _defender;
        private readonly FinalizedFight _fight;

        public FightAction(ICombatant attacker, ICombatant defender, FinalizedFight fight) {
            _attacker = attacker;
            _defender = defender;
            _fight = fight;
        }

        public string GetValidationError(Turn turn) {
            if (!turn.CanAct(_attacker)) {
                return "The attacker, " + _attacker.Id + " has already acted this turn.";
            }

            return null;
        }

        public void Perform(Turn turn) {
            var executor = new Execution.FightExecutor();
            executor.Execute(_fight);

            turn.MarkAction(_attacker);
        }
    }
}