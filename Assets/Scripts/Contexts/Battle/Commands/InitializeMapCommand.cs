using Contexts.Battle.Models;
using Contexts.Battle.Utilities;
using Models.Fighting.Maps;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class InitializeMapCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public MapDimensions Dimensions { get; set; }

        public override void Execute() {
            Model.Map = new Map(Dimensions.Width, Dimensions.Height);
        }
    }
}
