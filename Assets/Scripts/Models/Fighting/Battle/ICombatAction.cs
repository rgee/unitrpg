namespace Models.Fighting.Battle {
    public interface ICombatAction {
        bool IsValid(Turn turn);
        void Perform(Turn turn);
    }
}