using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Models.Combat;
using Models.Fighting;
using Models.Fighting.Characters;

namespace Contexts.Battle.Utilities {
    public class AvailableActions {
        [Inject]
        public BattleViewState BattleViewModel { get; set; }

        public HashSet<CombatActionType> GetAvailableActionTypes(ICombatant combatant) {
            var battle = BattleViewModel.Battle;
            var map = BattleViewModel.Map;

            var results = new HashSet<CombatActionType>();
            if (battle.CanAct(combatant)) {
                results.Add(CombatActionType.Item);

                var attackableSquares = map.FindSurroundingPoints(combatant.Position, battle.GetMaxWeaponAttackRange(combatant));
                var attackableUnits = attackableSquares
                    .Select(square => map.GetAtPosition(square))
                    .Where(unit => unit != null && unit.Army == ArmyType.Enemy);

                if (attackableUnits.Any()) {
                    results.Add(CombatActionType.Attack);
                    results.Add(CombatActionType.Brace);

                    if (combatant.SpecialSkill.HasValue) {
                        results.Add(CombatActionType.Special);
                    }
                }

                var friendlyUnits = attackableSquares
                    .Select(square => map.GetAtPosition(square))
                    .Where(unit => unit != null && unit.Army == ArmyType.Friendly);
                if (friendlyUnits.Any()) {
                    results.Add(CombatActionType.Trade);
                }
            }

            if (battle.CanMove(combatant)) {
                results.Add(CombatActionType.Move);
            }

            return results;
        }
    }
}