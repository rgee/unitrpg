using Models.Combat;

namespace Models.Fighting.Buffs {
    public class CountdownBuff : AbstractBuff {
        private readonly int _turns;
        private int _turnsApplied;

        public CountdownBuff(string name, int turns) : base(name) {
            _turns = turns;

            CombatEventBus.TurnChanges.AddListener(TickTurn);
        }

        private void TickTurn() {
            _turnsApplied -= 1;
        }

        public override bool CanApply(IBattle battle) {
            return _turnsApplied <= _turns;
        }

        public override void OnRemove() {
            CombatEventBus.TurnChanges.RemoveListener(TickTurn);
        }
    }
}