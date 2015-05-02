using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models.Combat {
    public class ActionProber : IActionProber {
        private readonly IMap _map;
        private readonly ITurn _turn;

        public ActionProber(IMap map, ITurn turn) {
            _map = map;
            _turn = turn;
        }

        public List<CombatAction> GetAvailableActions(Unit unit) {
            var results = new List<CombatAction>();
            if (CanFight(unit)) {
                results.Add(CombatAction.Fight);
            }

            if (CanAct(unit)) {
                results.Add(CombatAction.Item);
                results.Add(CombatAction.Brace);
            }

            if (AnyAdjacentFriendlies(unit)) {
                results.Add(CombatAction.Cover);
                results.Add(CombatAction.Trade);
            }

            if (HasRemainingMoves(unit)) {
                results.Add(CombatAction.Move);
            }

            results.Add(CombatAction.Wait);
            return results;
        }

        private bool HasRemainingMoves(Unit unit) {
            return _turn.GetRemainingMoves(unit) > 0;
        }

        private bool AnyAdjacentFriendlies(Unit unit) {
            return GetAdjacentUnits(unit)
                .Any(adjUnit => adjUnit.IsFriendly);
        }

        private bool CanFight(Unit unit) {
            if (!CanAct(unit)) {
                return false;
            }

            var adjacentUnits = GetAdjacentUnits(unit);
            var attackable = from enemy in adjacentUnits
                             where !enemy.IsFriendly && WithinRange(unit, enemy)
                             select enemy;

            return attackable.Any();
        }

        private static bool WithinRange(Unit attacker, Unit defender) {
            return MathUtils.ManhattanDistance(attacker.GridPosition, defender.GridPosition) <=
                   attacker.Character.AttackRange;
        }


        private bool CanAct(Unit unit) {
            return _turn.CanAct(unit);
        }

        private IEnumerable<Unit> GetAdjacentUnits(Unit unit) {
            return from point in GetAdjacentPoints(unit.GridPosition)
                   select _map.GetUnitByPosition(point);
        } 

        private static IEnumerable<Vector2> GetAdjacentPoints(Vector2 point) {
            return new List<Vector2> {
                new Vector2(point.x-1, point.y),
                new Vector2(point.x+1, point.y),
                new Vector2(point.x, point.y-1),
                new Vector2(point.x-1, point.y)
            };
        }
    }
}