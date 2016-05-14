using Contexts.Battle.Models;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class FightConfirmedCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        public override void Execute() {
            Model.State = BattleUIState.Fighting;
        }
    }
}
