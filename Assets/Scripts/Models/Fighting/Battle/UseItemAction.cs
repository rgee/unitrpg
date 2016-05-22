using Models.Fighting.Items;

namespace Models.Fighting.Battle {
    public class UseItemAction : ICombatAction {
        private readonly ICombatant _user;
        private readonly string _itemId;

        public string GetValidationError(Turn turn) {
            if (!turn.CanAct(_user)) {
                return "The attacker, " + _user.Id + " has already acted this turn.";
            }

            return null;
        }


        public void Perform(Turn turn) {
            var item = ItemDatabase.Instance.GetItemById(_itemId);
            item.Use(_user);
            turn.MarkAction(_user);
        }
    }
}