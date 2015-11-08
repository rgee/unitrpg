using Assets.Contexts.Application.Signals;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace Contexts.Application.Commands {
    public class RootStartCommand : Command {
        [Inject] public AddSceneSignal AddSceneSignal { get; set; }

        public override void Execute() {
            AddSceneSignal.Dispatch("Global");            
        }
    }
}
