using Models.Fighting.Items;

namespace Models.Fighting.Battle {
    public class UseItemAction : ICombatAction {
        private readonly ICombatant _user;
        private readonly string _itemId;

        public bool IsValid(Turn turn) {
            return turn.CanAct(_user);
        }

        public void Perform(Turn turn) {
            var item = ItemDatabase.Instance.GetItemById(_itemId);
            item.Use(_user);
        }
    }
}