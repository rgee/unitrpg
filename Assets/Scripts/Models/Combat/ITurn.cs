namespace Models.Combat {
    public interface ITurn {
        int TurnCount { get; }
        TurnControl Control { get; }
        void End();
        void RecordMove(Unit unit, int squares);
        void RecordAction(Unit unit);
        int GetRemainingMoves(Unit unit);
        bool CanAct(Unit unit);
    }
}