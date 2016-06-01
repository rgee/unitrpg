using Contexts.Battle.Models;
using Contexts.BattlePrep.Signals.Public;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class BeginSurveyingCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public HidePrepSignal HidePrepSignal { get; set; }

        public override void Execute() {
            Model.State = BattleUIState.Surveying;
            HidePrepSignal.Dispatch();
        }
    }
}
