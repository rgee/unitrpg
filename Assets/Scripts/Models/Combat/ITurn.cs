namespace Models.Combat {
    public interface ITurn {
        int TurnCount { get; }
        TurnControl Control { get; }
        void End();
        void RecordMove(Models.Combat.Unit unit, int squares);
        void RecordAction(Models.Combat.Unit unit);
        int GetRemainingMoves(Models.Combat.Unit unit);
        bool CanAct(Models.Combat.Unit unit);
    }
}