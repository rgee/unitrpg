using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Battle.Utilities;
using Contexts.Global.Services;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using Models.Fighting.Maps;
using Models.SaveGames;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class InitializeMapCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public MapConfiguration Configuration { get; set; }

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        public override void Execute() {
            var dimensions = Configuration.Dimensions;
            Model.Map = new Map(dimensions.Width, dimensions.Height);

            var turnOrder = new List<ArmyType> {ArmyType.Friendly, ArmyType.Enemy, ArmyType.Other};
            var saveGame = SaveGameService.CurrentSave;
            var database = new CombatantDatabase(Configuration.Combatants, saveGame);
            Model.Battle = new global::Models.Fighting.Battle.Battle(Model.Map, Configuration.Randomizer, database, turnOrder);
        }
    }
}
