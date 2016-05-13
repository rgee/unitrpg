using Contexts.Battle.Models;

namespace Contexts.Battle.Utilities {
    public class StateTransition {
        public readonly BattleUIState Previous;
        public readonly BattleUIState Next;

        public StateTransition(BattleUIState previous, BattleUIState next) {
            Previous = previous;
            Next = next;
        }
    }
}