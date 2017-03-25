using System;
using System.Collections;
using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Signals.Camera;
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

        [Inject]
        public CameraPanCompleteSignal CameraPanCompleteSignal { get; set; }

        [Inject]
        public CameraPanToPointOfInterestSignal PanToPointOfInterestSignal { get; set; }

        public override void Execute() {
            Model.State = BattleUIState.EventPlaying;
            Model.EventsThisActionPhase.Add(Tile.EventName);

            var poiFactory = new PointOfInterestFactory(Model.Dimensions);
            var interaction = new InteractAction(Tile, Model.SelectedCombatant, Model.Map);
            var pointOfInterest = interaction.GetPointofInterest(poiFactory);

            Retain();
            Action panListener = null;
            panListener = new Action(() => {
                Release();
                CameraPanCompleteSignal.RemoveListener(panListener);
                ActionAnimationCompleteSignal.Dispatch(new InteractAction(Tile, Model.SelectedCombatant, Model.Map));
            });
            CameraPanCompleteSignal.AddListener(panListener);
            PanToPointOfInterestSignal.Dispatch(pointOfInterest);
        }
    }
}