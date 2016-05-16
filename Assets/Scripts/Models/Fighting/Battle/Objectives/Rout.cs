using Models.Fighting.Characters;

namespace Models.Fighting.Battle.Objectives {
    public class Rout : IObjective {
        public string Description {
            get { return "Eliminate all enemies."; }
        }

        public bool IsComplete(IBattle battle) {
            return battle.GetAliveByArmy(ArmyType.Enemy).Count <= 0;
        }

        public bool HasFailed(IBattle battle) {
            return battle.GetAliveByArmy(ArmyType.Friendly).Count <= 0;
        }
    }
}