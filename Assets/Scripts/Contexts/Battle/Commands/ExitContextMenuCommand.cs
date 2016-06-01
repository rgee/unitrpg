using Contexts.Battle.Models;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class ExitContextMenuCommand : Command {
        [Inject]
        public BattleViewState ViewState { get; set; }

        public override void Execute() {
            if (ViewState.State == BattleUIState.ContextMenu) {
                ViewState.State = BattleUIState.SelectingUnit;
            }
        }
    }
}