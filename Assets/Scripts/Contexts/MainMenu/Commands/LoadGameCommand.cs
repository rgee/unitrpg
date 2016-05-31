using Contexts.Global.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.MainMenu.Commands {
    public class LoadGameCommand : Command {
        [Inject]
        public ChangeSceneSignal ChangeSceneSignal { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject View { get; set; }

        public override void Execute() {
            ChangeSceneSignal.Dispatch(View, "LoadGame");
        }
    }
}
