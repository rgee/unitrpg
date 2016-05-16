﻿using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Battle.Utilities;
using Contexts.Common.Model;
using Contexts.Global.Services;
using Models.Fighting;
using Models.Fighting.Battle.Objectives;
using Models.Fighting.Characters;
using strange.extensions.command.impl;

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

        public override void Execute() {
            var dimensions = Configuration.Dimensions;
            Model.Dimensions = dimensions;  
            Model.Map = Configuration.Map;

            var objectives = new List<IObjective>();
            var config = BattleConfigRepository.GetConfigByIndex(Configuration.ChapterNumber);
            objectives.Add(config.Objective);

            var turnOrder = new List<ArmyType> {ArmyType.Friendly, ArmyType.Enemy, ArmyType.Other};
            Model.Battle = new global::Models.Fighting.Battle.Battle(Model.Map, new BasicRandomizer(), Configuration.Combatants, turnOrder, objectives);
            Model.State = BattleUIState.SelectingUnit;
        }
    }
}
