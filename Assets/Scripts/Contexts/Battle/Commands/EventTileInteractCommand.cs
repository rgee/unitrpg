using System.Collections;
using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Models.Fighting.Battle;
using Models.Fighting.Maps.Triggers;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class EventTileInteractCommand : Command {
        [Inject]
        public EventTile Tile { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public ProcessEventHandlersSignal ProcessEventHandlersSignal { get; set; }

        [Inject]
        public EventHandlersCompleteSignal EventHandlersCompleteSignal { get; set; }
        
        [Inject]
        public BattleEventRegistry BattleEventRegistry { get; set; }

        [Inject]
        public ActionAnimationCompleteSignal ActionAnimationCompleteSignal { get; set; }

        public override void Execute() {
            Model.State = BattleUIState.EventPlaying;
            Model.EventsThisActionPhase.Add(Tile.EventName);
            ActionAnimationCompleteSignal.Dispatch(new InteractAction(Tile, Model.SelectedCombatant, Model.Map));
        }
    }
}