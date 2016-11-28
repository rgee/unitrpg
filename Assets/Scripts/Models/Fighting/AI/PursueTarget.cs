using System.Linq;
using Models.Fighting.Battle;

namespace Models.Fighting.AI {
    /// <summary>
    /// 
    /// </summary>
    public class PursueTarget : ICombatantBrain {
        private readonly ICombatant _self;
        private readonly string _targetId;

        public PursueTarget(ICombatant self, string targetId) {
            _self = self;
            _targetId = targetId;
        }

        public ICombatAction ComputeAction(IBattle battle) {
            var map = battle.Map;

            // Misconfigured targetId - do nothing
            var target = battle.GetById(_targetId);
            if (target == null) {
                return null;
            }

            // Target is dead - do nothing
            if (!target.IsAlive) {
                return null;
            }

            var maxRangeWeapon = _self.EquippedWeapons
                .OrderByDescending(weapon => weapon.Range)
                .First();
            var maxRange = maxRangeWeapon.Range;

            var attackSquares = map.BreadthFirstSearch(_self.Position, maxRange, true);
            if (attackSquares.Contains(target.Position)) {
                // attack!
            } else {
                // we're out of range, move toward target
            }

            return null;
        }
    }
}