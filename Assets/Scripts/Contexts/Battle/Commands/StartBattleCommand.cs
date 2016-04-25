using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class StartBattleCommand : Command {
        public override void Execute() {
            ApplicationEventBus.SceneStart.Dispatch();
        }
    }
}
