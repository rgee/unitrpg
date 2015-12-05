using strange.extensions.command.impl;

namespace Contexts.Base.Commands {
    public class RootStartCommand : Command {
        public override void Execute() {
            UnityEngine.Application.LoadLevelAdditive("Global");
        }
    }
}
