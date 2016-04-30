using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class MapHoveredCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public GridPosition Position { get; set; }

        [Inject]
        public HoveredTileChangeSignal HoveredTileChangeSignal { get; set; }

        public override void Execute() {
            Model.HoveredTile = Position.GridCoordinates;
            HoveredTileChangeSignal.Dispatch(Position.WorldCoordinates);
        }
    }
}
