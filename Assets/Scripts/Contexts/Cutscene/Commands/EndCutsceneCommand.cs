using System.Collections.Generic;
using Assets.Contexts.Application.Signals;
using Contexts.Common.Model;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.Cutscene.Commands {
    public class EndCutsceneCommand : Command {
        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public ChangeSceneMultiSignal ChangeSceneSignal { get; set; }

        [Inject]
        public ApplicationState ApplicationState { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject View { get; set; }

        public override void Execute() {
            var save = SaveGameService.CurrentSave;

            ApplicationState.EndCurrentCutscene();
            var nextCutscene = ApplicationState.GetCurrentCutscene();
            if (nextCutscene == null) {
                ApplicationState.CurrentCutsceneSequence = null;
                var nextConfig = BattleConfigRepository.GetConfigByIndex(save.ChapterNumber);
                var nextSceneName = nextConfig.InitialSceneName;
                ChangeSceneSignal.Dispatch(View, new List<string> { nextSceneName });
            } else {
                ChangeSceneSignal.Dispatch(View, new List<string> { "Cutscene" });
            }
        }
    }
}
