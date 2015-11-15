using Contexts.BattlePrep.Signals;
using strange.extensions.command.impl;

namespace Contexts.BattlePrep.Commands
{
    public class BattlePrepStartCommand : Command {
        [Inject]
        public UpdateObjectiveSignal UpdateObjectiveSignal { get; set; }

        public override void Execute() {
            UpdateObjectiveSignal.Dispatch();
        }
    }
}
