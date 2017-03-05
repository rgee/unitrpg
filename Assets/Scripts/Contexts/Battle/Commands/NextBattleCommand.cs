using Contexts.Common.Model;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.Battle.Commands
{
    public class NextBattleCommand : Command {
        [Inject]
        public ApplicationState ApplicationState { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject Context { get; set; }

        [Inject]
        public NextStoryboardSceneSignal NextStoryboardSceneSignal { get; set; }

        public override void Execute() {
            NextStoryboardSceneSignal.Dispatch(Context);
        }
    }
}