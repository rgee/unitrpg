using Contexts.Global.Signals;
using strange.extensions.command.impl;

namespace Contexts.MainMenu.Commands {
    public class LoadOptionsCommand : Command {
        [Inject]
        public LoadSceneSignal LoadSceneSignal { get; set; }

        public override void Execute() {
            LoadSceneSignal.Dispatch("Options");
        }
    }
}
