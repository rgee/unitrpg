using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Battle.Utilities;
using Contexts.Common.Model;
using Contexts.Global.Services;
using Models.Fighting;
using Models.Fighting.Battle.Objectives;
using Models.Fighting.Characters;
using Models.Fighting.Maps.Configuration;
using Models.Fighting.Maps.Triggers;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class InitializeMapCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public MapConfiguration Configuration { get; set; }

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public IMapConfigRepository MapConfigRepository { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject ContextGameObject { get; set; }

        public override void Execute() {

            var context = ContextGameObject.GetComponent<BattleRoot>();
            var dimensions = Configuration.Dimensions;
            Model.Dimensions = dimensions;  
            Model.Map = Configuration.Map;

            var objectives = new List<IObjective>();
            var config = BattleConfigRepository.GetConfigByIndex(Configuration.ChapterNumber);
            objectives.Add(config.Objective);

            var turnOrder = new List<ArmyType> {ArmyType.Friendly, ArmyType.Enemy, ArmyType.Other};
            var mapConfig = MapConfigRepository.GetConfigByMapName(context.MapName);

            // If the map config is not here, that means it isn't configured.
            var eventTiles = mapConfig != null ? mapConfig.EventTiles : new List<EventTile>();
            Debug.LogWarning("No map config found for: " + context.MapName);
            
            Model.Battle = new global::Models.Fighting.Battle.Battle(Model.Map, new BasicRandomizer(), Configuration.Combatants, turnOrder, objectives,
                eventTiles);

            Model.State = BattleUIState.SelectingUnit;
            Model.ChapterIndex = Configuration.ChapterNumber;
            
            Model.Battle.EventTileSignal.AddListener(eventName => {
                Model.EventsThisActionPhase.Add(eventName);
            });
        }

    }
}
