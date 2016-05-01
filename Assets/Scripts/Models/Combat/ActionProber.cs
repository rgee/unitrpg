using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models.Combat {
    public class ActionProber : IActionProber {
        private readonly IOldMap _map;
        private readonly ITurn _turn;

        public ActionProber(IOldMap map, ITurn turn) {
            _map = map;
            _turn = turn;
        }

        public IEnumerable<CombatActionType> GetAvailableFightActions(Unit unit) {
            if (!CanFight(unit)) {
                return new List<CombatActionType>();
            }

            return unit.Character.Actions;
        } 

        public IEnumerable<CombatActionType> GetAvailableActions(Unit unit) {
            var results = new List<CombatActionType>();
            if (CanFight(unit)) {
                results.Add(CombatActionType.Fight);
            }

            if (CanAct(unit)) {
                if (!results.Contains(CombatActionType.Fight)) {
                    results.Add(CombatActionType.Brace);
                }

                results.Add(CombatActionType.Item);

                if (AnyAdjacentFriendlies(unit)) {
                    results.Add(CombatActionType.Trade);
                }

                if (GetUsableAdjacentInteractiveTiles(unit).Any()) {
                    results.Add(CombatActionType.Use);
                }
            }


            if (HasRemainingMoves(unit)) {
                results.Add(CombatActionType.Move);
            }

            results.Add(CombatActionType.Wait);
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

            var adjacentUnits = GetAdjacentUnits(unit).ToList();
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

        private IEnumerable<InteractiveTile> GetAdjacentInteractiveTiles(Unit unit) {
            return MathUtils.GetAdjacentPoints(unit.GridPosition)
                .Where(p => !_map.IsOccupied(p))
                .Select(p => _map.GetTileByPosition(p))
                .Where(tile => tile != null);
        }

        private IEnumerable<InteractiveTile> GetUsableAdjacentInteractiveTiles(Unit unit) {
            return from tile in GetAdjacentInteractiveTiles(unit)
                   where tile.CanTrigger()
                   select tile;

        } 

        private IEnumerable<Unit> GetAdjacentUnits(Unit unit) {
            return from point in MathUtils.GetAdjacentPoints(unit.GridPosition)
                   where _map.IsOccupied(point)
                   select _map.GetUnitByPosition(point);
        } 
    }
}