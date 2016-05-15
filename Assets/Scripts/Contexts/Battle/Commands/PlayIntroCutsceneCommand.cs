using Contexts.Battle.Signals;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class PlayIntroCutsceneCommand : Command {
        [Inject]
        public IntroCutsceneStartSignal CutsceneStartSignal { get; set; }

        public override void Execute() {
            CutsceneStartSignal.Dispatch();
        }
    }
}
