using System.Linq;

namespace Models.Combat.Objectives {
    public class Rout : IObjective {
        public bool IsFailed(IBattle battle) {
            return battle.GetFriendlyUnits()
                         .All(unit => !unit.IsAlive);
        }

        public bool IsComplete(IBattle battle) {
            return battle.GetEnemyUnits()
                         .All(unit => unit.IsAlive);
        }
    }
}