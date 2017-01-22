using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using Models.Fighting.Equip;
using Models.Fighting.Maps;
using Models.Fighting.Skills;
using UnityEngine;

namespace Models.Fighting.AI.Brains {
    public class Guard : ICombatantBrain {
        private readonly ICombatant _self;
        private ICombatant _target;

        public Guard(ICombatant self) {
            _self = self;
        }

        public IEnumerable<ICombatAction> ComputeActions(IBattle battle) {
            var map = battle.Map;
            if (_target != null) {
                // If the target has died since we last acted, give up on it and 
                // try to find a new thing to do.
                if (!_target.IsAlive) {
                    _target = null;
                    return ComputeActions(battle);
                }

                // If the target is in range of one of our weapons, use it
                var weapon = ChooseWeapon();
                if (IsInAttackRange(_target, weapon, map)) {
                    var skill = weapon.Range > 1 ? SkillType.Ranged : SkillType.Melee;
                    var forecast = battle.ForecastFight(_self, _target, skill);
                    var finalized = battle.FinalizeFight(forecast);
                    return new List<ICombatAction> { new FightAction(finalized) };
                }

                // If there's actually a path to the target, move there
                var path = map.FindPathToAdjacentTile(_self.Position, _target.Position);
                if (path != null) {
                    // Remove the first node because it's exactly where we're standing
                    path.RemoveAt(0);
                    
                    // Move as far as we can toward the target, limited by our move range.
                    var moveRange = _self.GetAttribute(Attribute.AttributeType.Move).Value;
                    var maxPathLength = Math.Min(moveRange, path.Count);
                    path = path.GetRange(0, maxPathLength);

                    var destination = path[path.Count - 1];
                    var action = new MoveAction(map, _self, destination, path);
                    return new List<ICombatAction> { action };
                }

                // There's no path to the target, so do nothing for now.
                return null;
            }

            // Find a new target and restart
            var friendlies = battle.GetAliveByArmy(ArmyType.Friendly);
            var others = battle.GetAliveByArmy(ArmyType.Other);
            var potentials = friendlies.Concat(others).ToList();

            // If there's nothing on the field to attack do nothing.
            if (!potentials.Any()) {
                return null;
            }

            _target = potentials.OrderBy(target => {
                var path = map.FindPathToAdjacentTile(_self.Position, target.Position);
                return path == null ? int.MaxValue : path.Count;
            }).First();

            return ComputeActions(battle);
        }

        private bool IsInAttackRange(ICombatant target, Weapon weapon, IMap map) {
            var attackablePositions = map.BreadthFirstSearch(_self.Position, weapon.Range, true);
            return attackablePositions.Contains(target.Position);
        }

        private Weapon ChooseWeapon() {
            // TODO: Make this a more intelligent decision than just longest-range
            return _self.EquippedWeapons
                .OrderByDescending(weapon => weapon.Range)
                .First();
        }
    }
}