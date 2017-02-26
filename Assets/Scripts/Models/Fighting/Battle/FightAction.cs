using Models.Fighting.Execution;
using Models.Fighting.Skills;
using FightExecutor = Models.Fighting.Execution.FightExecutor;

namespace Models.Fighting.Battle {
    public class FightAction : ICombatAction {
        public readonly FinalizedFight Fight;
        private readonly ICombatant _attacker;
        private readonly ICombatant _defender;

        public FightAction(FinalizedFight fight) {
            _attacker = fight.InitialPhase.Initiator;
            _defender = fight.InitialPhase.Receiver;
            Fight = fight;
        }

        public FightAction(ICombatant attacker, ICombatant defender, FinalizedFight fight) {
            _attacker = attacker;
            _defender = defender;
            Fight = fight;
        }

        public string GetValidationError(Turn turn) {
            if (!turn.CanAct(_attacker)) {
                return "The attacker, " + _attacker.Id + " has already acted this turn.";
            }

            return null;
        }

        public void Perform(Turn turn) {
            var executor = new Execution.FightExecutor();
            executor.Execute(Fight);

            turn.MarkAction(_attacker);
        }

        public IPointOfInterest GetPointofInterest(ICombatActionVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}