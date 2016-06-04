using Contexts.Global.Signals;
using strange.extensions.command.impl;

namespace Contexts.Global.Commands {
    public class CompleteScenePopCommand : Command {
        [Inject]
        public ScenePopCompleteSignal ScenePopCompleteSignal { get; set; }

        public override void Execute() {
            ScenePopCompleteSignal.Dispatch();
        }
    }
}