using Assets.Contexts.Application.Signals;
using strange.extensions.command.impl;

namespace Assets.Contexts.Main {
    public class StartApplicationCommand : Command {
        [Inject]
        public AddSceneSignal AddSceneSignal { get; set; }

        public override void Execute() {
            AddSceneSignal.Dispatch("Main Menu");
        }
    }
}
