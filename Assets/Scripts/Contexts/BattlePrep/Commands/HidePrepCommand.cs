using Contexts.BattlePrep.Signals;
using strange.extensions.command.impl;

namespace Contexts.BattlePrep.Commands {
    public class HidePrepCommand : Command {
        [Inject]
        public TransitionOutSignal TransitionOutSignal { get; set; }

        public override void Execute() {
            TransitionOutSignal.Dispatch();
        }
    }
}
