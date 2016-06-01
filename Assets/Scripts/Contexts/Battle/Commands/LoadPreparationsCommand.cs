using Assets.Contexts.Application.Signals;
using Contexts.Battle.Models;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class LoadPreparationsCommand : Command {
        [Inject]
        public AddSceneSignal AddSceneSignal { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }

        public override void Execute() {
            AddSceneSignal.Dispatch("BattlePrep");
            Model.State = BattleUIState.Preparations;
        }
    }
}
