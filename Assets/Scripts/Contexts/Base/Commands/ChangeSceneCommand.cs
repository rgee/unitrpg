using Contexts.Common;
using strange.extensions.command.impl;

namespace Contexts.Base.Commands {
    public class ChangeSceneCommand : Command {
        [Inject]
        public IRoutineRunner Runner { get; set; }

        public override void Execute() {
        }
    }
}
