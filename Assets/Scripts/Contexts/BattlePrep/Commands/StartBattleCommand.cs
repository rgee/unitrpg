using Contexts.Battle.Signals;
using strange.extensions.command.impl;

namespace Contexts.BattlePrep.Commands {
    public class StartBattleCommand : Command {
        [Inject]
        public BattleStartSignal BattleStartSignal { get; set; }

        public override void Execute() {
            BattleStartSignal.Dispatch();
        }
    }
}
