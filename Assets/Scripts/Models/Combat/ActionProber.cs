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

        public IEnumerable<CombatAction> GetAvailableFightActions(Unit unit) {
            if (!CanFight(unit)) {
                return new List<CombatAction>();
            }

            return unit.Character.Actions;
        } 

        public IEnumerable<CombatAction> GetAvailableActions(Unit unit) {
            var results = new List<CombatAction>();
            if (CanFight(unit)) {
                results.Add(CombatAction.Fight);
            }

            if (CanAct(unit)) {
                if (!results.Contains(CombatAction.Fight)) {
                    results.Add(CombatAction.Brace);
                }

                results.Add(CombatAction.Item);

                if (AnyAdjacentFriendlies(unit)) {
                    results.Add(CombatAction.Trade);
                }

                if (GetUsableAdjacentInteractiveTiles(unit).Any()) {
                    results.Add(CombatAction.Use);
                }
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
            return from point in MathUtils.GetAdjacentPoints(unit.GridPosition)
                   where !_map.IsOccupied(point)
                   select _map.GetTileByPosition(point);
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