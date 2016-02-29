namespace Models.Fighting.Execution {
    public class FinalizedFightBuilder {
        private FightPhase _initial;
        private FightPhase _flank;
        private FightPhase _counter;
        private FightPhase _double;

        public FinalizedFightBuilder Initial(FightPhase phase) {
            _initial = phase;
            return this;
        }

        public FinalizedFightBuilder Flank(FightPhase phase) {
            _flank = phase;
            return this;
        }

        public FinalizedFightBuilder Counter(FightPhase phase) {
            _counter = phase;
            return this;
        }

        public FinalizedFightBuilder Double(FightPhase phase) {
            _double = phase;
            return this;
        }

        public FinalizedFight Build() {
            return new FinalizedFight {
                InitialPhase = _initial,
                FlankerPhase = _flank,
                CounterPhase = _counter,
                DoubleAttackPhase = _double
            };
        } 
    }
}