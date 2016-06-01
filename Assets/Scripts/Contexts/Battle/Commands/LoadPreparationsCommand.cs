using Assets.Contexts.Application.Signals;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class LoadPreparationsCommand : Command {
        [Inject]
        public AddSceneSignal AddSceneSignal { get; set; }

        public override void Execute() {
            AddSceneSignal.Dispatch("BattlePrep");
        }
    }
}
