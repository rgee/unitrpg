using System.Linq;

namespace Models.Combat.Objectives {
    public class Rout : IObjective {
        public bool IsFailed(IOldBattle battle) {
            return battle.GetFriendlyUnits()
                         .All(unit => !unit.IsAlive);
        }

        public bool IsComplete(IOldBattle battle) {
            return battle.GetEnemyUnits()
                         .All(unit => unit.IsAlive);
        }
    }
}