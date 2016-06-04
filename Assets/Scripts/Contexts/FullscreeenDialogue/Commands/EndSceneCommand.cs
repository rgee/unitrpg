using Contexts.Global.Signals;
using strange.extensions.command.impl;

namespace Assets.Contexts.FullscreeenDialogue.Commands {
    public class EndSceneCommand : Command {
        [Inject]
        public PopSceneSignal PopSceneSignal { get; set; }

        public override void Execute() {
            PopSceneSignal.Dispatch();
        }
    }
}