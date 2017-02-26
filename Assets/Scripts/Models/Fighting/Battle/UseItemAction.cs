using Models.Fighting.Items;

namespace Models.Fighting.Battle {
    public class UseItemAction : ICombatAction {
        public readonly ICombatant Combatant;
        private readonly string _itemId;

        public string GetValidationError(Turn turn) {
            if (!turn.CanAct(Combatant)) {
                return "The attacker, " + Combatant.Id + " has already acted this turn.";
            }

            return null;
        }


        public void Perform(Turn turn) {
            var item = ItemDatabase.Instance.GetItemById(_itemId);
            item.Use(Combatant);
            turn.MarkAction(Combatant);
        }

        public IPointOfInterest GetPointofInterest(ICombatActionVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}