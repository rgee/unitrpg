using Models.Fighting.Characters;

namespace Models.Fighting.Battle.Objectives {
    public class Survive : IObjective {
        public string Description {
            get { return string.Format("Survive for {0} turns.", _turnCount); }
        }

        private readonly int _turnCount;

        public Survive(int turnCount) {
            _turnCount = turnCount;
        }

        public bool IsComplete(IBattle battle) {
            return battle.TurnNumber >= _turnCount;
        }

        public bool HasFailed(IBattle battle) {
            return battle.GetAliveByArmy(ArmyType.Friendly).Count <= 0;
        }
    }
}