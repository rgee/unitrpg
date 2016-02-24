namespace Models.Fighting.Battle {
    public interface CombatAction {
        bool IsValid(Turn turn);
        void Perform(Turn turn);
    }
}