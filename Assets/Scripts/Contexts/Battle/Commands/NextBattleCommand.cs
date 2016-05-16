using Assets.Contexts.Common.Services;
using Contexts.Battle.Models;
using Contexts.Common.Model;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class NextBattleCommand : Command {
        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public ChangeSceneSignal ChangeSceneSignal { get; set; }

        [Inject]
        public ApplicationState ApplicationState { get; set; }

        [Inject]
        public ICutsceneLoader CutsceneLoader { get; set; }

        [Inject]
        public BattleViewState BattleViewState { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject Context { get; set; }


        public override void Execute() {
            var nextBattleIndex = BattleViewState.ChapterIndex + 1;
            var battleConfig = BattleConfigRepository.GetConfigByIndex(nextBattleIndex);
            var cutscenes = battleConfig.DialogueResourceSequence;
            if (cutscenes.Count > 0) {
                ChangeSceneSignal.Dispatch(Context, "Cutscene");
            } else {
                ChangeSceneSignal.Dispatch(Context, battleConfig.InitialSceneName);
            }
        }
    }
}