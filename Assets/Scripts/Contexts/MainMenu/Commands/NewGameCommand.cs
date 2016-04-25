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
        private static readonly string CutsceneSceneName = "Cutscene";

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public ChangeSceneSignal ChangeSceneSignal { get; set; }

        [Inject]
        public ApplicationState ApplicationState { get; set; }

        [Inject]
        public ICutsceneLoader CutsceneLoader { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject Context { get; set; }

        public override void Execute() {
            SaveGameService.CreateNewGame();

            var battleConfig = BattleConfigRepository.GetConfigByIndex(0);
            var preBattleCutscenes = battleConfig.DialogueResourceSequence;
            if (preBattleCutscenes.Count > 0) {
                var parsedCutscenes = preBattleCutscenes.Select(name => CutsceneLoader.Load(name)).ToList();
                ApplicationState.CurrentCutsceneSequence = parsedCutscenes;
                ChangeSceneSignal.Dispatch(Context, CutsceneSceneName);
            } else {
                ChangeSceneSignal.Dispatch(Context, battleConfig.InitialSceneName);
            }
        }
    }
}
