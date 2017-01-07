using System.Linq;
using Assets.Contexts.Common.Services;
using Contexts.Common.Model;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.MainMenu.Commands {
    public class NewGameCommand : Command {

        [Inject]
        public NextStoryboardSceneSignal NextStoryboardSceneSignal { get; set; }

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject Context { get; set; }

        public override void Execute() {
            SaveGameService.CreateNewGame();
            NextStoryboardSceneSignal.Dispatch(Context);
        }
    }
}
