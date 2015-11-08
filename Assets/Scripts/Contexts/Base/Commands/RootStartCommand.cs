using Assets.Contexts.Application.Signals;
using strange.extensions.command.impl;

namespace Contexts.Base.Commands {
    public class RootStartCommand : Command {
        [Inject] public AddSceneSignal AddSceneSignal { get; set; }

        public override void Execute() {
            AddSceneSignal.Dispatch("Global");            
        }
    }
}
