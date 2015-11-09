using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.BattlePrep.Commands {
    public class RemoveContextCommand : Command {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject ContextView { get; set; }

        public override void Execute() {
            GameObject.Destroy(ContextView);
        }
    }
}
