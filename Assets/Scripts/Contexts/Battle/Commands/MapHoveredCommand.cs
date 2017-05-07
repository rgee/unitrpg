﻿using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Models.Fighting;
using Models.Fighting.Maps;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Contexts.Battle.Commands {
    public class MapHoveredCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public GridPosition Position { get; set; }

        [Inject]
        public HoveredTileChangeSignal HoveredTileChangeSignal { get; set; }

        [Inject]
        public HoverTileDisableSignal HoverTileDisableSignal { get; set; }

        [Inject]
        public MovementPathReadySignal PathReadySignal { get; set; }

        [Inject]
        public MovementPathUnavailableSignal PathUnavailableSignal { get; set; }

        public override void Execute() {
            var map = Model.Map;
            if (map == null) {
                return;
            }

            if (Model.State == BattleUIState.SelectingMoveLocation) {

                var combatant = Model.SelectedCombatant;
                var moveRange = combatant.GetAttribute(Attribute.AttributeType.Move).Value;
                var destination = Position.GridCoordinates;
                var filter = PathfindingUtils.GetCombatantTileFilter(combatant);
                var path = map.FindPath(combatant.Position, destination, filter);
                if (path == null || path.Count - 1 > moveRange) {
                    PathUnavailableSignal.Dispatch();
                    Model.CurrentMovementPath = null;
                } else {
                    PathReadySignal.Dispatch(path);
                    Model.CurrentMovementPath = new MovementPath(path, Model.SelectedCombatant);
                }
            } else if (Model.State != BattleUIState.Fighting || Model.State != BattleUIState.CombatantMoving) {

                if (map.IsBlockedByEnvironment(Position.GridCoordinates)) {
                    HoveredTileChangeSignal.Dispatch(new Vector2(float.MaxValue, float.MaxValue));
                } else {
                    Model.HoveredTile = Position.GridCoordinates;
                    HoveredTileChangeSignal.Dispatch(Position.WorldCoordinates);
                }
            }
        }
    }
}
