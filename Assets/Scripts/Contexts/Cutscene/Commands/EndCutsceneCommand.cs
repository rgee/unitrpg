using Contexts.Global.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.Cutscene.Commands {
    public class EndCutsceneCommand : Command {
        [Inject]
        public NextStoryboardSceneSignal NextStoryboardSceneSignal { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject View { get; set; }

        public override void Execute() {
            NextStoryboardSceneSignal.Dispatch(View);
        }
    }
}
