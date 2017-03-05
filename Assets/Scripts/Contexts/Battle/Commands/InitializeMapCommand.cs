using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Common.Model;
using Contexts.Global.Models;
using Contexts.Global.Services;
using Models.Fighting;
using Models.Fighting.Battle.Objectives;
using Models.Fighting.Characters;
using Models.Fighting.Maps.Configuration;
using Models.Fighting.Maps.Triggers;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;
using MapConfiguration = Contexts.Battle.Utilities.MapConfiguration;

namespace Contexts.Battle.Commands {
    public class InitializeMapCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public MapConfiguration Configuration { get; set; }

        [Inject]
        public Game Game { get; set; }

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
            var mapConfig = MapConfigRepository.GetConfigByMapName(Configuration.Id);

            Model.Battle = new global::Models.Fighting.Battle.Battle(
                combatants: Configuration.Combatants,
                randomizer: new BasicRandomizer(),
                map: Model.Map,
                turnOrder: turnOrder,
                objectives: objectives,
                mapConfig: mapConfig
            );

            Model.State = BattleUIState.SelectingUnit;
            Model.ChapterIndex = Configuration.ChapterNumber;
            
            Model.Battle.EventTileSignal.AddListener(eventName => {
                Model.EventsThisActionPhase.Add(eventName);
            });
        }

    }
}
